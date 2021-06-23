using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class RankingController: ControllerBase
    {
        //ToDo
        public MainWindowController MainWindowController { get; set; }

        private readonly RankingViewModel _rankingViewModel;
        private readonly App _app;
        private readonly OrderProto.OrderProtoClient _orderProtoClient;
        private readonly OrderLineProto.OrderLineProtoClient _orderLineProtoClient;
        private readonly ArticleProto.ArticleProtoClient _articleProtoClient;
        private readonly EmployeeProto.EmployeeProtoClient _employeeProtoClient;
        private readonly AreaProto.AreaProtoClient _areaProtoClient;

        public RankingController(RankingControl view, RankingViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _rankingViewModel = viewModel;

            View.DataContext = _rankingViewModel;
            _app = app;
            _orderProtoClient = new OrderProto.OrderProtoClient(channel);
            _orderLineProtoClient = new OrderLineProto.OrderLineProtoClient(channel);
            _articleProtoClient = new ArticleProto.ArticleProtoClient(channel);
            _employeeProtoClient = new EmployeeProto.EmployeeProtoClient(channel);
            _areaProtoClient = new AreaProto.AreaProtoClient(channel);
        }

        public void LoadNewData()
        {
            //ToDo
        }
    }
}
