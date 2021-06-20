using System;
using Aufgabe_14_Client.Views;
using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class MainWindowController
    {
        /*private readonly MainWindow mView;
        private readonly MainWindowViewModel mViewModel;
        private readonly App mApplication;
        private readonly CustomerServiceClient mCustomerServiceClient;

        public MainWindowController(MainWindow view, MainWindowViewModel viewModel, App app, CustomerServiceClient customerServiceClient)
        {
            mCustomerServiceClient = customerServiceClient;
            mView = view;
            mViewModel = viewModel;
            mViewModel.AddCommand = new RelayCommand(ExecuteAddCommand);
            mViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            mViewModel.EmptyCommand = new RelayCommand(ExecuteEmptyCommand);
            mViewModel.LoadCommand = new RelayCommand(ExecuteLoadCommand);
            mViewModel.SearchCommand = new RelayCommand(ExecuteSearch);
            mApplication = app;
            mApplication.MainWindow = mView;
            mView.DataContext = mViewModel;
        }

        public void ExecuteAddCommand(object o)
        {
            
            var mWindowAddController = mApplication.Container.Resolve<WindowAddController>();
            var customer = mWindowAddController.AddCustomer();
            if (customer != null)
            {
                if (mCustomerServiceClient.AddCustomer(customer))
                {
                    mViewModel.Models.Add(customer);
                }
            }
        }

        public void ExecuteDeleteCommand(object o)
        {
            var customer = mViewModel.SelectedModel;
            if (customer != null)
            {
                if (mCustomerServiceClient.RemoveCustomer(customer))
                {
                    mViewModel.Models.Clear();
                    mCustomerServiceClient
                        .GetAllCustomers()
                        .ForEach(x => mViewModel.Models.Add(x));
                }
            }
        }

        public void ExecuteEmptyCommand(object o)
        {
            mViewModel.Models.Clear();
        }

        public void ExecuteLoadCommand(object o)
        {
            mViewModel.Models.Clear();
            mCustomerServiceClient
                .GetAllCustomers()
                .ForEach(x => mViewModel.Models.Add(x));
        }

        public void ExecuteSearch(object o)
        {
            mViewModel.Models.Clear();
            mCustomerServiceClient
                .GetCustomers(mViewModel.SearchingLastName)
                .ForEach(x => mViewModel.Models.Add(x));
        }

        public bool CanExecuteDeleteCommand(object o)
        {
            return mViewModel.SelectedModel != null;
        }

        public void Initialize()
        {
            mView.ShowDialog();
        }*/
    }
}