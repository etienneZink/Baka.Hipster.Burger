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
    public class CustomerController : ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private CustomerViewModel _viewModel;
        private App _app;
        private CustomerProto.CustomerProtoClient _customerProtoClient;

        private bool _newItem;

        public CustomerController(CustomerControl view, CustomerViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;
            _viewModel = viewModel;

            _viewModel.AddCommand = new RelayCommand(ExecuteAddCommand);
            _viewModel.EditCommand = new RelayCommand(ExecuteEditCommand, CanExecuteSelectedCommand);
            _viewModel.SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            _viewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteSelectedCommand);            

            View.DataContext = ViewModel;
            _app = app;

            _customerProtoClient = new CustomerProto.CustomerProtoClient(channel);
        }

        public void ExecuteAddCommand(object o)
        {
            _viewModel.SelectedModel = new Customer();
            _viewModel.ViewDetail = true;
            _newItem = true;
        }

        public void ExecuteEditCommand(object o)
        {
            _viewModel.ViewDetail = true;
            _newItem = false;
        }

        public void ExecuteSaveCommand(object o)
        {
            if (_viewModel.SelectedModel is null)
            {
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            if (_viewModel.SelectedModel.Name is null ||
                _viewModel.SelectedModel.City is null ||
                _viewModel.SelectedModel.Firstname is null ||
                _viewModel.SelectedModel.Phone is null ||
                _viewModel.SelectedModel.PostalCode <= 0 ||
                _viewModel.SelectedModel.Street is null ||
                _viewModel.SelectedModel.StreetNumber is null)
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
                    idMessage = _customerProtoClient.Add(new CustomerRequest
                    {
                        Name = _viewModel.SelectedModel.Name,
                        City = _viewModel.SelectedModel.City,
                        Firstname = _viewModel.SelectedModel.Firstname,
                        Id = _viewModel.SelectedModel.Id,
                        Phone = _viewModel.SelectedModel.Phone,
                        PostalCode = _viewModel.SelectedModel.PostalCode,
                        Street = _viewModel.SelectedModel.Street,
                        StreetNumber = _viewModel.SelectedModel.StreetNumber
                    },headers);
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
                boolRespone = _customerProtoClient.Update(new CustomerRequest
                {
                    Name = _viewModel.SelectedModel.Name,
                    City = _viewModel.SelectedModel.City,
                    Firstname = _viewModel.SelectedModel.Firstname,
                    Id = _viewModel.SelectedModel.Id,
                    Phone = _viewModel.SelectedModel.Phone,
                    PostalCode = _viewModel.SelectedModel.PostalCode ,
                    Street = _viewModel.SelectedModel.Street,
                    StreetNumber = _viewModel.SelectedModel.StreetNumber
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
                result = _customerProtoClient.Delete(new IdMessage { Id = _viewModel.SelectedModel.Id}, headers);
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
                _popupWindowController.DisplayText("An error accured and the data couldn't be deleted. Please try again laiter and make sure, there is no related data to this entry!");
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

            CustomerResponses result;
            
            try
            {
                result = _customerProtoClient.GetAll(new Empty(), headers);
            } catch (Exception)
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

            foreach (var message in result.Customers)
            {
                _viewModel.Models.Add(new Customer
                {
                    Name = message.Name,
                    City = message.City,
                    Firstname = message.Firstname,
                    Id = message.Id,
                    Phone = message.Phone,
                    PostalCode = message.PostalCode,
                    Street = message.Street,
                    StreetNumber = message.StreetNumber
                });
            }
        }
    }
}
