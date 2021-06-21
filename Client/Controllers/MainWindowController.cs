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
            _viewModel.ShowLoginCommand = new RelayCommand(ExecuteLoginCommand);
            _viewModel.StartCommand = new RelayCommand(ExecuteStartCommand);

            _view.DataContext = _viewModel;
            _app = app;

            _loginController = loginController;
            _startController = startController;

            _loginController.MainWindowController = this;
            _viewModel.SelectedController = _loginController;
        }

        public void ExecuteLoginCommand(object o)
        {
            _viewModel.SelectedController = _loginController;
        }

        public void ExecuteStartCommand(object o)
        {
            _viewModel.SelectedController = _startController;
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
