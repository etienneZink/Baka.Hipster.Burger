using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using System;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class AreaController: ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private readonly AreaViewModel _viewModel;
        private readonly App _app;
        private readonly AreaProto.AreaProtoClient _areaProtoClient;

        public AreaController(AreaControl view, AreaViewModel viewModel, App app, GrpcChannel channel)
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
            _areaProtoClient = new AreaProto.AreaProtoClient(channel);
        }

        public void ExecuteAddCommand(object o)
        {
            _viewModel.SelectedModel = new Area();
            _viewModel.ViewDetail = true;
            _viewModel.NewItem = true;
        }

        public void ExecuteEditCommand(object o)
        {
            _viewModel.ViewDetail = true;
            _viewModel.NewItem = false;
        }

        public void ExecuteSaveCommand(object o)
        {
            if (_viewModel.SelectedModel is null)
            {
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            if (_viewModel.SelectedModel.Description is null)            
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("Please make sure to fill in all data!");
                return;
            } else if (_viewModel.SelectedModel.PostCode <= 0)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("Please make sure to fill in a positive number as the postcode!");
                _viewModel.SelectedModel.PostCode = 0;
                return;
            }

            if (_viewModel.NewItem)
            {
                IdMessage idMessage;
                try
                {
                    idMessage = _areaProtoClient.Add(new AreaRequest
                    {
                        PostCode = _viewModel.SelectedModel.PostCode,
                        Description = _viewModel.SelectedModel.Description,
                        Id = _viewModel.SelectedModel.Id,
                    }, headers);
                }
                catch (Exception)
                {
                    var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                    _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                    return;
                }

                if (idMessage is null || idMessage.Id < 0)
                {
                    var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                    _popupWindowController.DisplayText("The data coundn't be saved. Please make sure that the postcode is unique!");
                    return;
                }
                LoadNewData();
                _viewModel.ViewDetail = false;
                return;
            }

            BoolResponse boolRespone;
            try
            {
                boolRespone = _areaProtoClient.Update(new AreaRequest
                {
                    PostCode = _viewModel.SelectedModel.PostCode,
                    Description = _viewModel.SelectedModel.Description,
                    Id = _viewModel.SelectedModel.Id,
                }, headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                return;
            }

            if (boolRespone is null || !boolRespone.Result)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("The data coundn't be saved. Please make sure that the postcode is unique!");
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
                result = _areaProtoClient.Delete(new IdMessage { Id = _viewModel.SelectedModel.Id }, headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                return;
            }

            if (result is null || !result.Result)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("An error occured and the data couldn't be deleted. Please make sure that the area is not related to an employee!");
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
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            AreaResponses result;

            try
            {
                result = _areaProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                return;
            }

            if (result is null || result.Status is Shared.Protos.Status.Failed)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                return;
            }

            _viewModel.Models.Clear();

            foreach (var message in result.Areas)
            {
                _viewModel.Models.Add(new Area
                {
                    PostCode = message.PostCode,
                    Description = message.Description,
                    Id = message.Id,
                });
            }
        }
    }
}
