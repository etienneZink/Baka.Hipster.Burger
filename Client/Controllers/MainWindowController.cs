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
        private readonly MainWindow _view;
        private readonly MainViewModel _viewModel;
        private readonly App _app;

        //controllers
        private readonly LoginController _loginController;
        private readonly StartController _startController;

        public MainWindowController(MainWindow view, MainViewModel viewModel, App app,
            LoginController loginController, StartController startController)
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
            _startController = startController;

            _loginController.MainWindowController = this;
            _viewModel.SelectedController = _loginController;
        }

        public void ExecuteStartCommand(object o)
        {
            _viewModel.SelectedController = _startController;
        }

        public void ExecuteCustomerCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("Customer!");
        }

        public void ExecuteOrderCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("Order!");
        }

        public void ExecuteBestSellerCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("BestSeller!");
        }

        public void ExecuteRankingCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("Ranking!");
        }

        public void ExecuteArticleCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("Article!");
        }

        public void ExecuteEmployeeCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("Employee!");
        }

        public void ExecuteAreaCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("Area!");
        }

        public void ExecuteUserCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("User!");
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
