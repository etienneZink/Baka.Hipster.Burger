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
    public class ArticleViewModel: ViewModelBase
    {
        public string ArticleNumber 
        { 
            get => SelectedModel?.ArticleNumber;
            set
            {
                if (SelectedModel is null || SelectedModel.ArticleNumber == value) return;
                SelectedModel.ArticleNumber = value;
                OnPropertyChanged(nameof(ArticleNumber));
            } 
        }

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

        public string Description
        {
            get => SelectedModel?.Description;
            set
            {
                if (SelectedModel is null || SelectedModel.Description == value) return;
                SelectedModel.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public double Price 
        {
            get
            {
                if (SelectedModel is null) return 0;
                return SelectedModel.Price;
            }
            set
            {
                if (SelectedModel is null || SelectedModel.Price == value) return;
                SelectedModel.Price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public ObservableCollection<Article> Models { get; set; } = new ObservableCollection<Article>();

        private Article _selectedModel;
        public Article SelectedModel
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
                OnPropertyChanged(nameof(ArticleNumber));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Price));
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
