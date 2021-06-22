using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Client.ViewModels ;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class PopupWindowController
    {
        private readonly PopupWindow _view;
        private readonly PopupViewModel _viewModel;

        public PopupWindowController(PopupWindow popupWindow, PopupViewModel popupViewModel)
        {
            _view = popupWindow;
            _viewModel = popupViewModel;
            _view.DataContext = _viewModel;
        }

        public void DisplayText (string text)
        {
            _viewModel.PopupText = text;
            _view.ShowDialog();
        }
    }
}
