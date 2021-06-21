using Baka.Hipster.Burger.Client.Controllers;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Shared.Models;
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
        private string _token;
        public string Token 
        { 
            get => _token;
            set
            {
                if (_token == value) return;
                _token = value;
                OnPropertyChanged(nameof(Token));
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsLoggedOut));
                OnPropertyChanged(nameof(View));
            }
        }

        private User _user;

        public User User
        { 
            get => _user; 
            set 
            {
                if (_user == value) return;
                _user = value;
                OnPropertyChanged(nameof(User));
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsLoggedOut));
                OnPropertyChanged(nameof(View));
            } 
        }

        private ControllerBase _selectedController;

        public ControllerBase SelectedController
        {
            get => _selectedController;
            set
            {
                if (_selectedController == value) return;
                _selectedController = value;
                OnPropertyChanged(nameof(ViewModel));
                OnPropertyChanged(nameof(SelectedController));
                OnPropertyChanged(nameof(View));
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

        public bool IsAdmin
        {
            get => !(User is null) && User.IsAdmin;
        }

        public bool IsLoggedIn
        {
            get => Token is not null 
                && Token != string.Empty
                && User?.Username is not null
                && User.Username != string.Empty;
        }

        public bool IsLoggedOut
        {
            get => !IsLoggedIn;
        }

        public ICommand StartCommand { get; set; }
        public ICommand CustomerCommand { get; set; }

        public ICommand OrderCommand { get; set; }

        public ICommand BestSellerCommand { get; set; }

        public ICommand RankingCommand { get; set; }

        public ICommand ArticleCommand { get; set; }
        public ICommand EmployeeCommand { get; set; }
        public ICommand AreaCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
    }
}
