using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Client.ViewModels ;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class PopupWindowController
    {
        private readonly PopupWindow _popupWindow;
        private readonly PopupViewModel _popupViewModel;

        public PopupWindowController(PopupWindow popupWindow, PopupViewModel popupViewModel)
        {
            _popupWindow = popupWindow;
            _popupViewModel = popupViewModel;
            _popupWindow.DataContext = _popupViewModel;
        }

        public void DisplayText (string text)
        {
            _popupViewModel.PopupText = text;
            _popupWindow.ShowDialog();
        }
    }
}
