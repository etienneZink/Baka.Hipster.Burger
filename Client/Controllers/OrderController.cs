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
    public class OrderController: ControllerBase
    {
        //ToDo
        public MainWindowController MainWindowController { get; set; }

        private readonly OrderViewModel _orderViewModel;
        private readonly App _app;
        private readonly OrderProto.OrderProtoClient _orderProtoClient;

        public OrderController(OrderControl view, OrderViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _orderViewModel = viewModel;

            View.DataContext = _orderViewModel;
            _app = app;
            _orderProtoClient = new OrderProto.OrderProtoClient(channel);
        }

        public void LoadNewData()
        {
            //ToDo
        }
    }
}
