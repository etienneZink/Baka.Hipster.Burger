using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Net.Client;
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
        private readonly UserProto.UserProtoClient _userProtoClient;

        private string Token { get; set; }
        private User User { get; set; }
        
        public LoginWindowController(LoginWindow loginWindow, LoginWindowViewModel loginWindowViewModel, App app, GrpcChannel channel)
        {
            _loginWindow = loginWindow;
            _loginWindowViewModel = loginWindowViewModel;
            _loginWindowViewModel.LoginCommand = new RelayCommand(ExecuteLoginCommand);
            
            _app = app;
            _app.MainWindow = _loginWindow;
            _loginWindow.DataContext = _loginWindowViewModel;

            _userProtoClient = new UserProto.UserProtoClient(channel);
        }

        public void ExecuteLoginCommand(object o)
        {
            if (_loginWindowViewModel.Username is null || _loginWindowViewModel.Password is null)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("Please enter a username and password!");
                return;
            }

            var result = _userProtoClient.LogIn(new UserLogin
            {
                Username = _loginWindowViewModel.Username,
                Password = _loginWindowViewModel.Password
            });

            if (result?.User is null || result.Status is Status.Failed)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("The username or password was wrong. Please try again!");
                return;
            }
            Token = result.Token;
            User = new User
            {
                Firstname = result.User.Firstname,
                Id = result.User.Id,
                IsAdmin = result.User.IsAdmin,
                Lastname = result.User.Lastname,
                Username = result.User.Username
            };
            //ToDo
            var _popupWindowControllerTest = _app.Container.Resolve<PopupWindowController>();
            
            _popupWindowControllerTest.DisplayText($"Token: {Token}\nUsername: {User.Username} ");
        }

        public void Initialize()
        {
            _loginWindow.ShowDialog();
        }
    }
}
