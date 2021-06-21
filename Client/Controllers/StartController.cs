using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class StartController: ControllerBase
    {
        private App _app;

        public StartController(Start view, StartViewModel viewModel, App app)
        {
            View = view;
            ViewModel = viewModel;
            viewModel.ResetPasswordCommand = new RelayCommand(ExecuteResetPasswordCommand);

            view.DataContext = viewModel;
            _app = app;
        }

        public void ExecuteResetPasswordCommand(object o)
        {
            //ToDo
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
            _popupWindowController.DisplayText("Password Reset!");
        }
    }
}
