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
    public class OrderLineController
    {
        private readonly OrderLineWindow _view;
        private readonly OrderLineViewModel _viewModel;
        private readonly App _app;

        private readonly ArticleProto.ArticleProtoClient _articleProtoClient;
        public OrderLineController(OrderLineWindow view, OrderLineViewModel viewModel, App app, GrpcChannel channel)
        {
            _view = view;
            _viewModel = viewModel;

            _viewModel.OkCommand = new RelayCommand(ExecuteOkCommand);
            _viewModel.CancelCommand = new RelayCommand(ExecuteCancelCommand);

            _view.DataContext = _viewModel;
            _app = app;
            _articleProtoClient = new ArticleProto.ArticleProtoClient(channel);
        }

        public void ExecuteOkCommand(object o)
        {
            _view.DialogResult = true;
            _view.Close();
        }

        public void ExecuteCancelCommand(object o)
        {
            _view.DialogResult = false;
            _view.Close();
        }

        public OrderLine AddOrderLine(string Token)
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {Token}");

            ArticleResponses result;

            try
            {
                result = _articleProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return null;
            }

            if (result is null || result.Status is Shared.Protos.Status.Failed)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return null;
            }

            _viewModel.Articles.Clear();

            foreach (var message in result.Articles)
            {
                _viewModel.Articles.Add(new Article
                {
                    Name = message.Name,
                    ArticleNumber = message.ArticleNumber,
                    Description = message.Description,
                    Id = message.Id,
                    Price = message.Price
                });
            }

            return _view.ShowDialog().GetValueOrDefault() && _viewModel.SelectedArticle is not null && _viewModel.Amount > 0 ? _viewModel.OrderLineHelper.OrderLine: null;
        }
    }
}
