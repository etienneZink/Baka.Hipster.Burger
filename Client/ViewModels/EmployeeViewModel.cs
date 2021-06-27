using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.Helper;
using Baka.Hipster.Burger.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class EmployeeViewModel: ViewModelBase
    {
        public bool NewItem { get; set; }

        public string FirstName
        {
            get => SelectedModel?.FirstName;
            set
            {
                if (SelectedModel is null || SelectedModel.FirstName == value) return;
                SelectedModel.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => SelectedModel?.LastName;
            set
            {
                if (SelectedModel is null || SelectedModel.LastName == value) return;
                SelectedModel.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public int EmployeeNumber
        {
            get
            {
                if (SelectedModel is null) return 0;
                return SelectedModel.EmployeeNumber;
            }
            set
            {
                if (SelectedModel is null || SelectedModel.EmployeeNumber == value) return;
                SelectedModel.EmployeeNumber = value;
                OnPropertyChanged(nameof(EmployeeNumber));
            }
        }

        public ObservableCollection<AreaHelper> Areas { get; set; } = new ObservableCollection<AreaHelper>();

        public ObservableCollection<Employee> Models { get; set; } = new ObservableCollection<Employee>();

        private Employee _selectedModel;
        public Employee SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel == value) return;
                _selectedModel = value;
                ViewDetail = false;
                NewItem = false;
                OnPropertyChanged(nameof(SelectedModel));
                OnPropertyChanged(nameof(ItemSelected));
                OnPropertyChanged(nameof(ViewDetail));
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(EmployeeNumber));
            }
        }

        private bool _viewDetail;

        public bool ViewDetail
        {
            get => _viewDetail;
            set
            {
                if (_viewDetail == value) return;
                _viewDetail = value;
                OnPropertyChanged(nameof(ViewDetail));
            }
        }

        public bool ItemSelected => SelectedModel != null;

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
    }
}
