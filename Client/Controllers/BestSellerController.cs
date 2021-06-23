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
    public class BestSellerController: ControllerBase
    {
        //ToDo
        public MainWindowController MainWindowController { get; set; }

        private readonly BestSellerViewModel _bestSellerViewModel;
        private readonly App _app;
        private readonly OrderLineProto.OrderLineProtoClient _orderLineProtoClient;
        private readonly ArticleProto.ArticleProtoClient _articleProtoClient;

        public BestSellerController(BestSellerControl view, BestSellerViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _bestSellerViewModel = viewModel;

            View.DataContext = _bestSellerViewModel;
            _app = app;
            _orderLineProtoClient = new OrderLineProto.OrderLineProtoClient(channel);
            _articleProtoClient = new ArticleProto.ArticleProtoClient(channel);
        }

        public void LoadNewData()
        {
            //ToDo
        }
    }
}
