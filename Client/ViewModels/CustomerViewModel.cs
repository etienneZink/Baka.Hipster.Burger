using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class CustomerViewModel: ViewModelBase
    {
        public string Name 
        { 
            get => SelectedModel?.Name; 
            set
            {
                if (SelectedModel is null || SelectedModel.Name == value) return;
                SelectedModel.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Firstname
        {
            get => SelectedModel?.Firstname;
            set
            {
                if (SelectedModel is null || SelectedModel.Firstname == value) return;
                SelectedModel.Firstname = value;
                OnPropertyChanged(nameof(Firstname));
            }
        }

        public string Phone
        {
            get => SelectedModel?.Phone;
            set
            {
                if (SelectedModel is null || SelectedModel.Phone == value) return;
                SelectedModel.Phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string Street
        {
            get => SelectedModel?.Street;
            set
            {
                if (SelectedModel is null || SelectedModel.Street == value) return;
                SelectedModel.Street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        public string StreetNumber
        {
            get => SelectedModel?.StreetNumber;
            set
            {
                if (SelectedModel is null || SelectedModel.StreetNumber == value) return;
                SelectedModel.StreetNumber = value;
                OnPropertyChanged(nameof(StreetNumber));
            }
        }

        public int PostalCode
        {
            get
            {
                if (SelectedModel is null) return 0;
                return SelectedModel.PostalCode;
            }
            set
            {
                if (SelectedModel is null || SelectedModel.PostalCode == value) return;
                SelectedModel.PostalCode = value;
                OnPropertyChanged(nameof(PostalCode));
            }
        }

        public string City
        {
            get => SelectedModel?.City;
            set
            {
                if (SelectedModel is null || SelectedModel.City == value) return;
                SelectedModel.City = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public ObservableCollection<Customer> Models { get; set; } = new ObservableCollection<Customer>();

        private Customer _selectedModel;
        public Customer SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel == value) return;
                _selectedModel = value;
                ViewDetail = false;
                OnPropertyChanged(nameof(SelectedModel));
                OnPropertyChanged(nameof(ItemSelected));
                OnPropertyChanged(nameof(ViewDetail));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Firstname));
                OnPropertyChanged(nameof(Phone));
                OnPropertyChanged(nameof(Street));
                OnPropertyChanged(nameof(StreetNumber));
                OnPropertyChanged(nameof(PostalCode));
                OnPropertyChanged(nameof(City));
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
