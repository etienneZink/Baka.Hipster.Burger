using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Client.ViewModels ;
using System.Windows.Media;
using Autofac;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class PopupWindowController
    {
        private readonly App _app;

        public PopupWindowController( App app)
        {
            _app = app;
        }

        public void DisplayText (string text)
        {
            var view = _app.Container.Resolve<PopupWindow>();
            var viewModel = _app.Container.Resolve<PopupViewModel>(); ;
            view.DataContext = viewModel;
            viewModel.PopupText = text;
            view.ShowDialog();
        }
    }
}
