using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class LoginController: ControllerBase
    {
        private App _app;
        private LoginViewModel _loginViewModel;

        private readonly UserProto.UserProtoClient _userProtoClient;

        private string Token { get; set; }
        private User User { get; set; }

        public LoginController(Login view, LoginViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;
            _loginViewModel = viewModel;
            _loginViewModel.LoginCommand = new RelayCommand(ExecuteLoginCommand);

            view.DataContext = viewModel;
            _app = app;

            _userProtoClient = new UserProto.UserProtoClient(channel);
        }

        public void ExecuteLoginCommand(object o)
        {
            if (_loginViewModel.Username is null || _loginViewModel.Password is null)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("Please enter a username and password!");
                return;
            }

            TokenMessage result;
            try
            {
                result = _userProtoClient.LogIn(new UserLogin
                {
                    Username = _loginViewModel.Username,
                    Password = _loginViewModel.Password
                });
            } catch (Exception e)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("An error accured! Please try again later.");
                return;
            }
            

            if (result?.User is null || result.Status is Shared.Protos.Status.Failed)
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

            /*
            var header = new Metadata();
            header.Add("Authorization", $"Bearer {Token}");

            _userProtoClient.Add(new FullUserRequest
            {
                Password = "geheim",
                User = new UserRequest
                {
                    Firstname = "Theo",
                    Lastname = "Test",
                    IsAdmin = false,
                    Username = "theo"
                }
            },
            headers: header);
            */
            var _popupWindowControllerTest = _app.Container.Resolve<PopupWindowController>();
            _popupWindowControllerTest.DisplayText($"Token: {Token}\nUsername: {User.Username} ");
        }
    }
}
