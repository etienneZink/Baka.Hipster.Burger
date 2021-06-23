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
    public class ArticleController: ControllerBase
    {
        //ToDo
        public MainWindowController MainWindowController { get; set; }

        private readonly ArticleViewModel _articleViewModel;
        private readonly App _app;
        private readonly ArticleProto.ArticleProtoClient _articleProtoClient;

        public ArticleController(ArticleControl view, ArticleViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _articleViewModel = viewModel;

            View.DataContext = _articleViewModel;
            _app = app;
            _articleProtoClient = new ArticleProto.ArticleProtoClient(channel);
        }

        public void LoadNewData()
        {
            //ToDo
        }
    }
}
