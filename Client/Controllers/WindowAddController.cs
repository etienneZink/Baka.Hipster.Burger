using Aufgabe_14_Client.Views;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class WindowAddController
    {
        /*private WindowAdd mView;
        private WindowAddViewModel mViewModel;

        public WindowAddController(WindowAdd view, WindowAddViewModel viewModel)
        {
            mView = view;
            mViewModel = viewModel;
            mViewModel.OkCommand = new RelayCommand(ExecuteOkCommand);
            mViewModel.CancelCommand = new RelayCommand(ExecuteCancelCommand);
            mView.DataContext = mViewModel;
        }

        public void ExecuteOkCommand(object o)
        {
            mView.DialogResult = true;
            mView.Close();
        }

        public void ExecuteCancelCommand(object o)
        {
            mView.DialogResult = false;
            mView.Close();
        }

        public Customer AddCustomer()
        {
            return mView.ShowDialog().GetValueOrDefault() ? mViewModel.Model : null;
        }*/
    }
}