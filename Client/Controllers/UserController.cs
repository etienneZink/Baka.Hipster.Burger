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
    public class UserController: ControllerBase
    {
        //ToDo
        public MainWindowController MainWindowController { get; set; }

        private readonly UserViewModel _userViewModel;
        private readonly App _app;
        private readonly UserProto.UserProtoClient _userProtoClient;

        public UserController(UserControlClass view, UserViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _userViewModel = viewModel;

            View.DataContext = _userViewModel;
            _app = app;
            _userProtoClient = new UserProto.UserProtoClient(channel);
        }

        public void LoadNewData()
        {
            //ToDo
        }
    }
}
