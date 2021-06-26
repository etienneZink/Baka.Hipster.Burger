using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.Helper;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Linq;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class EmployeeController: ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private readonly EmployeeViewModel _viewModel;
        private readonly App _app;
        private readonly EmployeeProto.EmployeeProtoClient _employeeProtoClient;
        private readonly AreaProto.AreaProtoClient _areaProtoClient;

        public EmployeeController(EmployeeControl view, EmployeeViewModel viewModel, App app, GrpcChannel channel)
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
            _employeeProtoClient = new EmployeeProto.EmployeeProtoClient(channel);
            _areaProtoClient = new AreaProto.AreaProtoClient(channel);
        }

        public void ExecuteAddCommand(object o)
        {
            _viewModel.SelectedModel = new Employee();
            _viewModel.ViewDetail = true;
            _viewModel.NewItem = true;

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            AreaResponses areaResult;

            try
            {
                areaResult = _areaProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (areaResult is null || areaResult.Status is Shared.Protos.Status.Failed)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            _viewModel.Areas.Clear();

            foreach (var message in areaResult.Areas)
            {
                _viewModel.Areas.Add(new AreaHelper
                {
                    InEmployee = message.Employees.Any(x => x.Id == _viewModel.SelectedModel.Id),

                    Area = new Area
                    {
                        Description = message.Description,
                        Id = message.Id,
                        PostCode = message.PostCode
                    }
                });
            }
        }

        public void ExecuteEditCommand(object o)
        {
            _viewModel.ViewDetail = true;
            _viewModel.NewItem = false;

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            AreaResponses areaResult;

            try
            {
                areaResult = _areaProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (areaResult is null || areaResult.Status is Shared.Protos.Status.Failed)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            _viewModel.Areas.Clear();
            
            foreach (var message in areaResult.Areas)
            {
                _viewModel.Areas.Add(new AreaHelper
                {
                    InEmployee = message.Employees.Any(x => x.Id == _viewModel.SelectedModel.Id),

                    Area = new Area
                    {
                        Description = message.Description,
                        Id = message.Id,
                        PostCode = message.PostCode
                    }
                });
            }
        }

        public void ExecuteSaveCommand(object o)
        {
            if (_viewModel.SelectedModel is null)
            {
                return;
            }

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            if (_viewModel.SelectedModel.FirstName is null ||
                _viewModel.SelectedModel.LastName is null ||
                _viewModel.SelectedModel.EmployeeNumber <= 0)
            {
                _popupWindowController.DisplayText("Please make sure to fill in all data!");
                return;
            }

            if (_viewModel.NewItem)
            {
                IdMessage idMessage;
                try
                {
                    var employee = new EmployeeRequest
                    {
                        FirstName = _viewModel.SelectedModel.FirstName,
                        EmployeeNumber = _viewModel.SelectedModel.EmployeeNumber,
                        LastName = _viewModel.SelectedModel.LastName,
                        Id = _viewModel.SelectedModel.Id
                    };

                    foreach (var area in _viewModel.Areas)
                    {
                        if (area.InEmployee) employee.Areas.Add(new IdMessage { Id = area.Area.Id });
                    }

                    idMessage = _employeeProtoClient.Add(employee, headers);
                }
                catch (Exception)
                {
                    _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                    return;
                }

                if (idMessage is null || idMessage.Id < 0)
                {
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
                var employee = new EmployeeRequest
                {
                    FirstName = _viewModel.SelectedModel.FirstName,
                    LastName = _viewModel.SelectedModel.LastName,
                    EmployeeNumber = _viewModel.SelectedModel.EmployeeNumber,
                    Id = _viewModel.SelectedModel.Id
                };

                foreach (var area in _viewModel.Areas)
                {
                    if (area.InEmployee) employee.Areas.Add(new IdMessage { Id = area.Area.Id });
                }

                boolRespone = _employeeProtoClient.Update(employee, headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (boolRespone is null || !boolRespone.Result)
            {
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

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            BoolResponse result;

            try
            {
                result = _employeeProtoClient.Delete(new IdMessage { Id = _viewModel.SelectedModel.Id }, headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || !result.Result)
            {
                _popupWindowController.DisplayText("An error accured and the data couldn't be deleted. Please try again laiter and make sure, there is no related data to this entry!");
                return;
            }

            LoadNewData();
        }

        public bool CanExecuteSelectedCommand(object o)
        {
            return _viewModel.ItemSelected && !_viewModel.NewItem;
        }

        public bool CanExecuteSaveCommand(object o)
        {
            return _viewModel.ViewDetail;
        }

        public void LoadNewData()
        {
            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            EmployeeResponses result;

            try
            {
                result = _employeeProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || result.Status is Shared.Protos.Status.Failed)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            _viewModel.Models.Clear();
            

            foreach (var message in result.Employees)
            {
                _viewModel.Models.Add(new Employee
                {
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    EmployeeNumber = message.EmployeeNumber,
                    Id = message.Id
                });
            }
        }
    }
}
