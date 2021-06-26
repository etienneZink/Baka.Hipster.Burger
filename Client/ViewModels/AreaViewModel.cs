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
    public class AreaViewModel: ViewModelBase
    {
        public bool NewItem { get; set; }
        public string Description
        {
            get => SelectedModel?.Description;
            set
            {
                if (SelectedModel is null || SelectedModel.Description == value) return;
                SelectedModel.Description = value;
                NewItem = false;
                OnPropertyChanged(nameof(Description));
            }
        }

        public int PostCode
        {
            get
            {
                if (SelectedModel is null) return 0;
                return SelectedModel.PostCode;
            }
            set
            {
                if (SelectedModel is null || SelectedModel.PostCode == value) return;
                SelectedModel.PostCode = value;
                OnPropertyChanged(nameof(PostCode));
            }
        }

        public ObservableCollection<Area> Models { get; set; } = new ObservableCollection<Area>();

        private Area _selectedModel;
        public Area SelectedModel
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
                OnPropertyChanged(nameof(PostCode));
                OnPropertyChanged(nameof(Description));
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
