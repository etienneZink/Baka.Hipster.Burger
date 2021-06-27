using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
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
    public class PasswordResetController
    {
        public static MainWindowController MainWindowController { get; set; }

        private ResetPasswordWindow _view;
        private ResetPasswordViewModel _viewModel;
        private readonly UserProto.UserProtoClient _userProtoClient;
        private readonly App _app;


        public PasswordResetController(ResetPasswordWindow view, ResetPasswordViewModel viewModel, GrpcChannel channel, App app)
        {
            _view = view;
            _viewModel = viewModel;
            _viewModel.SubmitPasswordResetCommand = new RelayCommand(ExecuteSubmitPasswordResetCommand);

            _view.DataContext = _viewModel;
            _app = app;

            _userProtoClient = new UserProto.UserProtoClient(channel);
        }

        public void ExecuteSubmitPasswordResetCommand(object o)
        {
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            if (_viewModel.OldPassword is null ||
                _viewModel.NewPasswordOne is null ||
                _viewModel.NewPasswordTwo is null ||
                _viewModel.OldPassword == string.Empty ||
                _viewModel.NewPasswordOne == string.Empty ||
                _viewModel.NewPasswordTwo == string.Empty)
            {
                _popupWindowController.DisplayText("Please enter all passwords!");
                return;
            }

            if (_viewModel.NewPasswordOne != _viewModel.NewPasswordTwo)
            {
                _popupWindowController.DisplayText("The two new passwords doesn't match each other!");
                _viewModel.NewPasswordOne = string.Empty;
                _viewModel.NewPasswordTwo = string.Empty;
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            BoolResponse result;

            try
            {
                result = _userProtoClient.ChangePassword(new ChangePasswordRequest
                {
                    Id = MainWindowController.UserId,
                    NewPassword = _viewModel.NewPasswordOne,
                    OldPassword = _viewModel.OldPassword
                }, headers);
            } catch (Exception)
            {
                _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                return;
            }

            if (result.Result)
            {
                _popupWindowController.DisplayText("Password successfully changed!");
                _view.Close();
                return;
            }
           
            _popupWindowController.DisplayText("The old password was not correct. Please try again!");
            _viewModel.OldPassword = string.Empty;
        }

        public void Show()
        {
            _view.ShowDialog();
        }
    }
}
