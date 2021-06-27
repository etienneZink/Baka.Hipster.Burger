using Baka.Hipster.Burger.Client.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class StartViewModel: ViewModelBase
    {
        public ICommand ResetPasswordCommand { get; set; }
    }
}
