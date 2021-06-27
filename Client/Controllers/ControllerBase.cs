using Baka.Hipster.Burger.Client.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class ControllerBase
    {
        public UserControl View { get; init; }
        public ViewModelBase ViewModel { get; init; }
    }
}
