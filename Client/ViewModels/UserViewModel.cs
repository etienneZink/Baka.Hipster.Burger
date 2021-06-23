﻿using Baka.Hipster.Burger.Client.Framework;
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
        //ToDo
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
                //OnPropertyChanged(nameof(Name));
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
