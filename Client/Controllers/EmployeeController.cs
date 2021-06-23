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
    public class EmployeeController: ControllerBase
    {
        //ToDo
        public MainWindowController MainWindowController { get; set; }

        private readonly EmployeeViewModel _employeeViewModel;
        private readonly App _app;
        private readonly EmployeeProto.EmployeeProtoClient _employeeProtoClient;

        public EmployeeController(EmployeeControl view, EmployeeViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _employeeViewModel = viewModel;

            View.DataContext = _employeeViewModel;
            _app = app;
            _employeeProtoClient = new EmployeeProto.EmployeeProtoClient(channel);
        }

        public void LoadNewData()
        {
            //ToDo
        }
    }
}
