using Baka.Hipster.Burger.Client.Framework;
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
    public class UserViewModel: ViewModelBase
    {
        public string Username
        {
            get => SelectedModel?.Username;
            set
            {
                if (SelectedModel is null || SelectedModel.Username == value) return;
                SelectedModel.Username = value;
                OnPropertyChanged(nameof(Username));
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

        public string Lastname
        {
            get => SelectedModel?.Lastname;
            set
            {
                if (SelectedModel is null || SelectedModel.Lastname == value) return;
                SelectedModel.Lastname = value;
                OnPropertyChanged(nameof(Lastname));
            }
        }
        public bool IsAdmin
        {
            get
            {
                if (SelectedModel is null) return false;
                return SelectedModel.IsAdmin;
            }
            set
            {
                if (SelectedModel is null || SelectedModel.IsAdmin == value) return;
                SelectedModel.IsAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        private bool _notOwnUser;
        public bool NotOwnUser
        {
            get => _notOwnUser;
            set
            {
                if (_notOwnUser == value) return;
                _notOwnUser = value;
                OnPropertyChanged(nameof(NotOwnUser));
            }
        }

        public ObservableCollection<User> Models { get; set; } = new ObservableCollection<User>();

        private User _selectedModel;
        public User SelectedModel
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
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(Firstname));
                OnPropertyChanged(nameof(Lastname));
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(NotOwnUser));
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
