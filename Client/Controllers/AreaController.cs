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
    public class AreaController: ControllerBase
    {
        //ToDo
        public MainWindowController MainWindowController { get; set; }

        private readonly AreaViewModel _areaViewModel;
        private readonly App _app;
        private readonly AreaProto.AreaProtoClient _areaProtoClient;

        public AreaController(AreaControl view, AreaViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _areaViewModel = viewModel;

            View.DataContext = _areaViewModel;
            _app = app;
            _areaProtoClient = new AreaProto.AreaProtoClient(channel);
        }

        public void LoadNewData()
        {
            //ToDo
        }
    }
}
