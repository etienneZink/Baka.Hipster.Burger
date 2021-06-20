using Baka.Hipster.Burger.Client.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class PopupViewModel: ViewModelBase
    {
        private string _popupText;

        public string PopupText
        {
            get => _popupText;
            set
            {
                if (_popupText == value) return;
                _popupText = value;
                OnPropertyChanged(nameof(_popupText));
            }
        }
    }
}
