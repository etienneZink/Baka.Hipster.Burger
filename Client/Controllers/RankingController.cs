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
    public class RankingController: ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private readonly RankingViewModel _viewModel;
        private readonly App _app;
        private readonly QueryProto.QueryProtoClient _queryProtoClient;

        public RankingController(RankingControl view, RankingViewModel viewModel, App app, GrpcChannel channel)
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

            AreaRanking result;

            try
            {
                result = _queryProtoClient.GetAreaRanking(new Empty(), headers);
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

            ObservableCollection<AreaQueryHelper> models = new ObservableCollection<AreaQueryHelper>();

            foreach (var message in result.Areas)
            {
                models.Add(new AreaQueryHelper
                {
                    Description = message.Description,
                    Turnover = message.Turnover,
                    Rank = message.Rank,
                    PostCode = message.PostCode
                });
            }

            _viewModel.Models.Clear();

            models.OrderBy(x => x.Rank)
                .ToList()
                .ForEach(x => _viewModel.Models.Add(x));
        }
    }
}
