using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class BestSellerViewModel: ViewModelBase
    {
        public ObservableCollection<ArticleQueryHelper> Models { get; set; } = new ObservableCollection<ArticleQueryHelper>();

        private ArticleQueryHelper _selectedModel;
        public ArticleQueryHelper SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel == value) return;
                _selectedModel = value;
                OnPropertyChanged(nameof(SelectedModel));
            }
        }
    }
}
