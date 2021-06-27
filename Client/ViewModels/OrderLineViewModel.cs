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
    public class OrderLineViewModel : ViewModelBase
    {
        private OrderLineHelper _orderLineHelper;
        public OrderLineHelper OrderLineHelper => _orderLineHelper ??= new OrderLineHelper();

        public ObservableCollection<Article> Articles { get; set; } = new ObservableCollection<Article>();

        
        public Article SelectedArticle
        {
            get => OrderLineHelper.Article;
            set
            {
                if (OrderLineHelper is null || OrderLineHelper.Article == value) return;
                OrderLineHelper.Article = value;
                OnPropertyChanged(nameof(SelectedArticle));
            }
        }

        public int Amount 
        {
            get => OrderLineHelper.Amount;
            set
            {
                if (OrderLineHelper.Amount == value) return;
                OrderLineHelper.Amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
    }
}
