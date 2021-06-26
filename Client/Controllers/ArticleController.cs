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
    public class ArticleController: ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private readonly ArticleViewModel _viewModel;
        private readonly App _app;
        private readonly ArticleProto.ArticleProtoClient _articleProtoClient;

        public ArticleController(ArticleControl view, ArticleViewModel viewModel, App app, GrpcChannel channel)
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
            _articleProtoClient = new ArticleProto.ArticleProtoClient(channel);
        }

        public void ExecuteAddCommand(object o)
        {
            _viewModel.SelectedModel = new Article();
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

            if (_viewModel.SelectedModel.Name is null ||
                _viewModel.SelectedModel.ArticleNumber is null ||
                _viewModel.SelectedModel.Description is null ||
                _viewModel.SelectedModel.Price <= 0)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("Please make sure to fill in all data!");
                return;
            }

            if (_viewModel.NewItem)
            {
                IdMessage idMessage;
                try
                {
                    idMessage = _articleProtoClient.Add(new ArticleRequest
                    {
                        Name = _viewModel.SelectedModel.Name,
                        Description = _viewModel.SelectedModel.Description,
                        ArticleNumber = _viewModel.SelectedModel.ArticleNumber,
                        Id = _viewModel.SelectedModel.Id,
                        Price = _viewModel.SelectedModel.Price
                    }, headers);
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
                boolRespone = _articleProtoClient.Update(new ArticleRequest
                {
                    Name = _viewModel.SelectedModel.Name,
                    Description = _viewModel.SelectedModel.Description,
                    ArticleNumber = _viewModel.SelectedModel.ArticleNumber,
                    Id = _viewModel.SelectedModel.Id,
                    Price = _viewModel.SelectedModel.Price
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
                result = _articleProtoClient.Delete(new IdMessage { Id = _viewModel.SelectedModel.Id }, headers);
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

            ArticleResponses result;

            try
            {
                result = _articleProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
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

            foreach (var message in result.Articles)
            {
                _viewModel.Models.Add(new Article
                {
                    Name = message.Name,
                    ArticleNumber = message.ArticleNumber,
                    Description = message.Description,
                    Id = message.Id,
                    Price = message.Price
                });
            }
        }
    }
}
