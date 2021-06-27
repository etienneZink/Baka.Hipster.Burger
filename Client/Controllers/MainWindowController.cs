using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class MainWindowController
    {
        public string Token => _viewModel.Token;
        public int UserId => _viewModel.User.Id;

        private readonly MainWindow _view;
        private readonly MainViewModel _viewModel;
        private readonly App _app;

        //controllers
        private readonly LoginController _loginController;
        private readonly StartController _startController;
        private readonly CustomerController _customerController;
        private readonly OrderController _orderController;
        private readonly BestSellerController _bestSellerController;
        private readonly RankingController _rankingController;
        private readonly ArticleController _articleController;
        private readonly EmployeeController _employeeController;
        private readonly AreaController _areaController;
        private readonly UserController _userController;

        public MainWindowController(MainWindow view, MainViewModel viewModel, App app,
            LoginController loginController, StartController startController,
            CustomerController customerController, OrderController orderController,
            BestSellerController bestSellerController, RankingController rankingController,
            ArticleController articleController, EmployeeController employeeController,
            AreaController areaController, UserController userController)
        {
            _view = view;
            _viewModel = viewModel;
            _viewModel.StartCommand = new RelayCommand(ExecuteStartCommand);
            _viewModel.OrderCommand = new RelayCommand(ExecuteOrderCommand);
            _viewModel.CustomerCommand = new RelayCommand(ExecuteCustomerCommand);
            _viewModel.BestSellerCommand = new RelayCommand(ExecuteBestSellerCommand);
            _viewModel.RankingCommand = new RelayCommand(ExecuteRankingCommand);
            _viewModel.ArticleCommand = new RelayCommand(ExecuteArticleCommand);
            _viewModel.EmployeeCommand = new RelayCommand(ExecuteEmployeeCommand);
            _viewModel.AreaCommand = new RelayCommand(ExecuteAreaCommand);
            _viewModel.UserCommand = new RelayCommand(ExecuteUserCommand);
            _viewModel.LogOutCommand = new RelayCommand(ExecuteLogOutCommand);

            _view.DataContext = _viewModel;
            _app = app;

            _loginController = loginController;
            _loginController.MainWindowController = this;

            _startController = startController;
            _startController.MainWindowController = this;

            _customerController = customerController;
            _customerController.MainWindowController = this;

            _orderController = orderController;
            _orderController.MainWindowController = this;

            _bestSellerController = bestSellerController;
            _bestSellerController.MainWindowController = this;

            _rankingController = rankingController;
            _rankingController.MainWindowController = this;

            _articleController = articleController;
            _articleController.MainWindowController = this;

            _employeeController = employeeController;
            _employeeController.MainWindowController = this;

            _areaController = areaController;
            _areaController.MainWindowController = this;

            _userController = userController;
            _userController.MainWindowController = this;


            PasswordResetController.MainWindowController = this;
            _viewModel.SelectedController = _loginController;
        }

        public void ExecuteStartCommand(object o)
        {
            _viewModel.SelectedController = _startController;
        }

        public void ExecuteCustomerCommand(object o)
        {
            _viewModel.SelectedController = _customerController;
            _customerController.LoadNewData();
        }

        public void ExecuteOrderCommand(object o)
        {
            _viewModel.SelectedController = _orderController;
            _orderController.LoadNewData();
        }

        public void ExecuteBestSellerCommand(object o)
        {
            _viewModel.SelectedController = _bestSellerController;
            _bestSellerController.LoadNewData();
        }

        public void ExecuteRankingCommand(object o)
        {
            _viewModel.SelectedController = _rankingController;
            _rankingController.LoadNewData();
        }

        public void ExecuteArticleCommand(object o)
        {
            _viewModel.SelectedController = _articleController;
            _articleController.LoadNewData();
        }

        public void ExecuteEmployeeCommand(object o)
        {
            _viewModel.SelectedController = _employeeController;
            _employeeController.LoadNewData();
        }

        public void ExecuteAreaCommand(object o)
        {
            _viewModel.SelectedController = _areaController;
            _areaController.LoadNewData();
        }

        public void ExecuteUserCommand(object o)
        {
            _viewModel.SelectedController = _userController;
            _userController.LoadNewData();
        }

        public void ExecuteLogOutCommand(object o)
        {
            _viewModel.Token = string.Empty;
            _viewModel.User = null;
            _viewModel.SelectedController = _loginController;
        }

        public void LogIn(string token, User user)
        {
            _viewModel.Token = token;
            _viewModel.User = user;
            _viewModel.SelectedController = _startController;
        }

        public void Initialize()
        {
            _view.ShowDialog();
        }
    }
}
