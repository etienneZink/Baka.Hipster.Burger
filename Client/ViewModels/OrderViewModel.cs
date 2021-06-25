using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.Helper;
using Baka.Hipster.Burger.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Baka.Hipster.Burger.Client.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        public Customer Customer
        {
            get => SelectedModel?.Customer;
            set
            {
                if (SelectedModel is null || SelectedModel.Customer == value) return;
                SelectedModel.Customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        public Employee Employee 
        {
            get => SelectedModel?.Employee;
            set
            {
                if (SelectedModel is null || SelectedModel.Employee == value) return;
                SelectedModel.Employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public ObservableCollection<OrderLineHelper> OrderLines { get; set; } = new ObservableCollection<OrderLineHelper>();

        private OrderLineHelper _selectedOrderLine;
        public OrderLineHelper SelecteOrderLine
        {
            get => _selectedOrderLine;
            set
            {
                if (_selectedOrderLine == value) return;
                _selectedOrderLine = value;
                OnPropertyChanged(nameof(SelecteOrderLine));
            }
        }

        public ObservableCollection<Order> Models { get; set; } = new ObservableCollection<Order>();

        private Order _selectedModel;
        public Order SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel == value) return;
                _selectedModel = value;
                ViewDetail = false;
                ViewOrderLineDetail = false;
                OnPropertyChanged(nameof(SelectedModel));
                OnPropertyChanged(nameof(ItemSelected));
                OnPropertyChanged(nameof(ViewDetail));
                OnPropertyChanged(nameof(ViewOrderLineDetail));
                OnPropertyChanged(nameof(Customer));
                OnPropertyChanged(nameof(Employee));
                OnPropertyChanged(nameof(OrderNumber));
                OnPropertyChanged(nameof(OrderDate));
                OnPropertyChanged(nameof(Description));
            }
        }

        public string OrderNumber 
        {
            get => SelectedModel?.OrderNumber;
            set
            {
                if (SelectedModel is null || SelectedModel.OrderNumber == value) return;
                SelectedModel.OrderNumber = value;
                OnPropertyChanged(nameof(OrderNumber));
            }
        }        

        public string OrderDate 
        {
            get 
            {
                if (SelectedModel?.OrderDate is null || SelectedModel.OrderDate == new DateTime()) return string.Empty;
                return SelectedModel.OrderDate.ToString(new CultureInfo("en-US"));
            }
            set
            {
                if (SelectedModel is null || OrderDate == value || !DateTime.TryParse(value, new CultureInfo("en-US"), DateTimeStyles.None, out var result)) return;
                SelectedModel.OrderDate = result;
                OnPropertyChanged(nameof(OrderDate));
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

        private bool _viewOrderLineDetail;

        public bool ViewOrderLineDetail
        {
            get => _viewOrderLineDetail;
            set
            {
                if (_viewOrderLineDetail == value) return;
                _viewOrderLineDetail = value;
                OnPropertyChanged(nameof(ViewOrderLineDetail));
            }
        }

        public bool ItemSelected => SelectedModel != null;

        public bool OrderLineSelected => SelecteOrderLine != null;

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ICommand AddOrderLineCommand { get; set; }
        public ICommand DeleteOrderLineCommand { get; set; }
    }
}
