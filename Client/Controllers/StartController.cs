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
        public MainWindowController MainWindowController { get; set; }

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
            var _passwordResetController = _app.Container.Resolve<PasswordResetController>();
            _passwordResetController.Show();
        }
    }
}
