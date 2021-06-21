using Baka.Hipster.Burger.Client.Controllers;
using Baka.Hipster.Burger.Client.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private ControllerBase _selectedController;

        public ControllerBase SelectedController
        {
            get => _selectedController;
            set
            {
                if (_selectedController == value) return;
                _selectedController = value;
                OnPropertyChanged(nameof(View));
                OnPropertyChanged(nameof(ViewModel));
                OnPropertyChanged(nameof(SelectedController));
            }
        }

        public ViewModelBase ViewModel 
        {
            get => SelectedController.ViewModel;
        }

        public UserControl View
        {
            get => SelectedController.View;
        }

        public ICommand ShowLoginCommand { get; set; }
        public ICommand StartCommand { get; set; }
    }
}
