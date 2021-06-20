using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class LoginWindowController
    {
        private readonly LoginWindow _loginWindow;
        private readonly LoginWindowViewModel _loginWindowViewModel;
        private readonly App _app;

        private string Token { get; set; }
        private User User { get; set; }
        //add userserviceclient
        public LoginWindowController(LoginWindow loginWindow, LoginWindowViewModel loginWindowViewModel, App app)
        {
            _loginWindow = loginWindow;
            _loginWindowViewModel = loginWindowViewModel;
            _loginWindowViewModel.LoginCommand = new RelayCommand(ExecuteLoginCommand);
            
            _app = app;
            _app.MainWindow = _loginWindow;
            _loginWindow.DataContext = _loginWindowViewModel;
        }

        public void ExecuteLoginCommand(object o)
        {
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("The username or password was wrong. Please try again!");
        }

        public void Initialize()
        {
            _loginWindow.ShowDialog();
        }
    }
}
