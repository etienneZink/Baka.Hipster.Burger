using Autofac;
using Baka.Hipster.Burger.Client.Helper;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class BestSellerController: ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private readonly BestSellerViewModel _viewModel;
        private readonly App _app;
        private readonly QueryProto.QueryProtoClient _queryProtoClient;

        public BestSellerController(BestSellerControl view, BestSellerViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _viewModel = viewModel;

            View.DataContext = _viewModel;
            _app = app;
            _queryProtoClient = new QueryProto.QueryProtoClient(channel);
        }

        public void LoadNewData()
        {

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            BestSeller result;

            try
            {
                result = _queryProtoClient.GetBestSeller(new Empty(), headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                return;
            }

            if (result is null || result.Status is Shared.Protos.Status.Failed)
            {
                _popupWindowController.DisplayText("A server error occured. Please try again laiter!");
                return;
            }

            ObservableCollection<ArticleQueryHelper> models = new ObservableCollection<ArticleQueryHelper>(); 

            foreach (var message in result.Articles)
            {
                models.Add(new ArticleQueryHelper
                {
                    Name = message.Name,
                    ArticleNumber = message.ArticleNumber,
                    Turnover = message.Turnover,
                    Amount = message.Amount,
                    Rank = message.Rank
                });
            }

            _viewModel.Models.Clear();

            models.OrderBy(x => x.Rank)
                .ToList()
                .ForEach(x => _viewModel.Models.Add(x));

        }
    }
}
