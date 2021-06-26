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

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class UserController: ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private readonly UserViewModel _viewModel;
        private readonly App _app;
        private readonly UserProto.UserProtoClient _userProtoClient;

        private bool _newItem;

        public UserController(UserControlClass view, UserViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _viewModel = viewModel;

            _viewModel.AddCommand = new RelayCommand(ExecuteAddCommand);
            _viewModel.EditCommand = new RelayCommand(ExecuteEditCommand, CanExecuteSelectedCommand);
            _viewModel.SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            _viewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteSelectedCommand);

            View.DataContext = _viewModel;
            _app = app;
            _userProtoClient = new UserProto.UserProtoClient(channel);
        }

        public void ExecuteAddCommand(object o)
        {
            _viewModel.SelectedModel = new User();
            _viewModel.ViewDetail = true;
            _newItem = true;
            _viewModel.NotOwnUser = true;
        }

        public void ExecuteEditCommand(object o)
        {
            _viewModel.ViewDetail = true;
            _newItem = false;
            _viewModel.NotOwnUser = true;
            if (_viewModel.SelectedModel.Id == MainWindowController.UserId) _viewModel.NotOwnUser = false;
        }

        public void ExecuteSaveCommand(object o)
        {
            if (_viewModel.SelectedModel is null)
            {
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            if (_viewModel.SelectedModel.Username is null ||
                _viewModel.SelectedModel.Firstname is null ||
                _viewModel.SelectedModel.Lastname is null)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("Please make sure to fill in all data!");
                return;
            }

            if (_newItem)
            {
                IdMessage idMessage;
                try
                {
                    idMessage = _userProtoClient.Add(new FullUserRequest
                    {
                        User = new UserRequest
                        {
                            Username = _viewModel.SelectedModel.Username,
                            Firstname = _viewModel.SelectedModel.Firstname,
                            Lastname = _viewModel.SelectedModel.Lastname,
                            Id = _viewModel.SelectedModel.Id,
                            IsAdmin = _viewModel.SelectedModel.IsAdmin
                        },
                        Password = _viewModel.SelectedModel.Password ??= "geheim"
                    }, headers);
                }
                catch (Exception)
                {
                    var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                    _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                    return;
                }

                if (idMessage is null || idMessage.Id < 0)
                {
                    var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                    _popupWindowController.DisplayText("The data coundn't be saved. Please make sure to satisfy your unique constraints!");
                    return;
                }
                LoadNewData();
                _viewModel.ViewDetail = false;
                return;
            }

            BoolResponse boolRespone;
            try
            {
                boolRespone = _userProtoClient.Update(new UserRequest
                {
                    Username = _viewModel.SelectedModel.Username,
                    Firstname = _viewModel.SelectedModel.Firstname,
                    Lastname = _viewModel.SelectedModel.Lastname,
                    Id = _viewModel.SelectedModel.Id,
                    IsAdmin = _viewModel.SelectedModel.IsAdmin
                }, headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (boolRespone is null || !boolRespone.Result)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("The data coundn't be saved. Please make sure to satisfy your unique constraints!");
                return;
            }
            _viewModel.ViewDetail = false;
            LoadNewData();
        }

        public void ExecuteDeleteCommand(object o)
        {
            if (_viewModel.SelectedModel is null)
            {
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            BoolResponse result;

            try
            {
                result = _userProtoClient.Delete(new IdMessage { Id = _viewModel.SelectedModel.Id }, headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || !result.Result)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("An error accured and the data couldn't be deleted. Please try again laiter and make sure, there is no related data to this entry! Also considere you can't delete an admin user. First demote the user to a normal user!");
                return;
            }

            LoadNewData();
        }

        public bool CanExecuteSelectedCommand(object o)
        {
            return _viewModel.ItemSelected;
        }

        public bool CanExecuteSaveCommand(object o)
        {
            return _viewModel.ViewDetail;
        }

        public void LoadNewData()
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            UserResponses result;

            try
            {
                result = _userProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || result.Status is Shared.Protos.Status.Failed)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            _viewModel.Models.Clear();

            foreach (var message in result.User)
            {
                _viewModel.Models.Add(new User
                {
                    Username = message.Username,
                    Firstname = message.Firstname,
                    Lastname = message.Lastname,
                    IsAdmin = message.IsAdmin,
                    Id = message.Id
                });
            }
        }
    }
}
