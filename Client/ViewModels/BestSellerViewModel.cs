using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.Helper;
using System.Collections.ObjectModel;

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
