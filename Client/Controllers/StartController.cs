using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class StartController: ControllerBase
    {
        public StartController(Start view, StartViewModel viewModel)
        {
            View = view;
            ViewModel = viewModel;
            view.DataContext = viewModel;
        }
    }
}
