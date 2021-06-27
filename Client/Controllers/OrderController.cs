﻿using Autofac;
using Baka.Hipster.Burger.Client.Framework;
using Baka.Hipster.Burger.Client.Helper;
using Baka.Hipster.Burger.Client.ViewModels;
using Baka.Hipster.Burger.Client.Views;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Controllers
{
    public class OrderController: ControllerBase
    {
        public MainWindowController MainWindowController { get; set; }

        private readonly OrderViewModel _viewModel;
        private readonly App _app;
        private readonly OrderProto.OrderProtoClient _orderProtoClient;
        private readonly EmployeeProto.EmployeeProtoClient _employeeProtoClient;
        private readonly CustomerProto.CustomerProtoClient _customerProtoClient;
        private readonly OrderLineProto.OrderLineProtoClient _orderLineProtoClient;
        private readonly ArticleProto.ArticleProtoClient _articleProtoClient;

        public OrderController(OrderControl view, OrderViewModel viewModel, App app, GrpcChannel channel)
        {
            View = view;
            ViewModel = viewModel;

            _viewModel = viewModel;

            _viewModel.AddCommand = new RelayCommand(ExecuteAddCommand);
            _viewModel.EditCommand = new RelayCommand(ExecuteEditCommand, CanExecuteSelectedCommand);
            _viewModel.SaveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            _viewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteSelectedCommand);

            _viewModel.AddOrderLineCommand = new RelayCommand(ExecuteAddOrderLineCommand);
            _viewModel.DeleteOrderLineCommand = new RelayCommand(ExecuteDeleteOrderLineCommand, CanExecuteOrderLineCommand);

            View.DataContext = _viewModel;
            _app = app;
            _orderProtoClient = new OrderProto.OrderProtoClient(channel);
            _employeeProtoClient = new EmployeeProto.EmployeeProtoClient(channel);
            _customerProtoClient = new CustomerProto.CustomerProtoClient(channel);
            _orderLineProtoClient = new OrderLineProto.OrderLineProtoClient(channel);
            _articleProtoClient = new ArticleProto.ArticleProtoClient(channel);
        }

        public void ExecuteAddCommand(object o)
        {
            _viewModel.SelectedModel = new Order();
            _viewModel.ViewDetail = true;
            _viewModel.ViewOrderLineDetail = false;
            _viewModel.NewItem = true;

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            CustomerResponses customerResult;
            EmployeeResponses employeeResponses;

            try
            {
                customerResult = _customerProtoClient.GetAll(new Empty(), headers);
                employeeResponses = _employeeProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (customerResult is null || customerResult.Status is Shared.Protos.Status.Failed)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            _viewModel.Customers.Clear();
            _viewModel.Employees.Clear();

            foreach (var message in customerResult.Customers)
            {
                _viewModel.Customers.Add(new Customer
                {
                    Name = message.Name,
                    City = message.City,
                    Firstname = message.Firstname,
                    Id = message.Id,
                    Phone = message.Phone,
                    PostalCode = message.PostalCode,
                    Street = message.Street,
                    StreetNumber = message.StreetNumber
                });
            }

            foreach (var message in employeeResponses.Employees)
            {
                _viewModel.Employees.Add(new Employee
                {
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    EmployeeNumber = message.EmployeeNumber,
                    Id = message.Id
                });
            }
        }

        public void ExecuteEditCommand(object o)
        {
            _viewModel.ViewDetail = true;
            _viewModel.ViewOrderLineDetail = true;
            _viewModel.NewItem = false;

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            CustomerResponses customerResult;
            EmployeeResponses employeeResponses;

            try
            {
                customerResult = _customerProtoClient.GetAll(new Empty(), headers);
                employeeResponses = _employeeProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (customerResult is null || customerResult.Status is Shared.Protos.Status.Failed)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            var selectedEmployee = _viewModel.Employee;
            var selectedCustomer = _viewModel.Customer;

            _viewModel.Customers.Clear();
            _viewModel.Employees.Clear();

            foreach (var message in customerResult.Customers)
            {
                _viewModel.Customers.Add(new Customer
                {
                    Name = message.Name,
                    City = message.City,
                    Firstname = message.Firstname,
                    Id = message.Id,
                    Phone = message.Phone,
                    PostalCode = message.PostalCode,
                    Street = message.Street,
                    StreetNumber = message.StreetNumber
                });
            }

            foreach (var message in employeeResponses.Employees)
            {
                _viewModel.Employees.Add(new Employee
                {
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    EmployeeNumber = message.EmployeeNumber,
                    Id = message.Id
                });
            }

            _viewModel.Employee = selectedEmployee;
            _viewModel.Customer = selectedCustomer;

            LoadNewOrderLineData();
        }

        public void ExecuteSaveCommand(object o)
        {
            if (_viewModel.SelectedModel is null)
            {
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            if (_viewModel.Customer is null ||
                _viewModel.Employee is null ||
                _viewModel.Description is null ||
                 _viewModel.OrderNumber is null ||
                 _viewModel.OrderDate is null ||
                 _viewModel.OrderDate == string.Empty)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("Please make sure to fill in all data!");
                return;
            }

            if (_viewModel.NewItem)
            {
                IdMessage idMessage;
                try
                {
                    idMessage = _orderProtoClient.Add(new OrderRequest
                    {
                        OrderDate = new DateTimeMessage
                        {
                            Year = _viewModel.SelectedModel.OrderDate.Year,
                            Month = _viewModel.SelectedModel.OrderDate.Month,
                            Day = _viewModel.SelectedModel.OrderDate.Day,
                            Hour = _viewModel.SelectedModel.OrderDate.Hour,
                            Second = _viewModel.SelectedModel.OrderDate.Second,
                            Minute = _viewModel.SelectedModel.OrderDate.Minute,
                            Millisecond = 0
                        },
                        Description = _viewModel.SelectedModel.Description,
                        OrderNumber = _viewModel.SelectedModel.OrderNumber,
                        Id = _viewModel.SelectedModel.Id,
                        Customer = new IdMessage { Id = _viewModel.Customer.Id},
                        Employee = new IdMessage { Id = _viewModel.Employee.Id }
                    }, headers);
                }
                catch (Exception)
                {
                    var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                    _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                    return;
                }

                if (idMessage is null || idMessage.Id < 0)
                {
                    var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                    _popupWindowController.DisplayText("The data coundn't be saved. Please make sure to satisfy your unique constraints!");
                    return;
                }
                LoadNewData();
                _viewModel.ViewOrderLineDetail = false;
                _viewModel.ViewDetail = false;
                return;
            }

            BoolResponse boolRespone;
            try
            {
                boolRespone = _orderProtoClient.Update(new OrderRequest
                {
                    OrderDate = new DateTimeMessage
                    {
                        Year = _viewModel.SelectedModel.OrderDate.Year,
                        Month = _viewModel.SelectedModel.OrderDate.Month,
                        Day = _viewModel.SelectedModel.OrderDate.Day,
                        Hour = _viewModel.SelectedModel.OrderDate.Hour,
                        Second = _viewModel.SelectedModel.OrderDate.Second,
                        Minute = _viewModel.SelectedModel.OrderDate.Minute,
                        Millisecond = 0
                    },
                    Description = _viewModel.Description,
                    OrderNumber = _viewModel.OrderNumber,
                    Id = _viewModel.SelectedModel.Id,
                    Customer = new IdMessage { Id = _viewModel.Customer.Id },
                    Employee = new IdMessage { Id = _viewModel.Employee.Id }
                }, headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (boolRespone is null || !boolRespone.Result)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("The data coundn't be saved. Please make sure to satisfy your unique constraints!");
                return;
            }
            _viewModel.ViewDetail = false;
            _viewModel.ViewOrderLineDetail = false;
            LoadNewData();
        }

        public void ExecuteDeleteCommand(object o)
        {
            if (_viewModel.SelectedModel is null)
            {
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            BoolResponse result;

            try
            {
                result = _orderProtoClient.Delete(new IdMessage { Id = _viewModel.SelectedModel.Id }, headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || !result.Result)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("An error accured and the data couldn't be deleted. Please try again laiter and make sure, there is no related data to this entry!");
                return;
            }

            LoadNewData();
        }

        public void ExecuteDeleteOrderLineCommand(object o)
        {
            if (_viewModel.SelecteOrderLine is null)
            {
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            BoolResponse result;

            try
            {
                result = _orderLineProtoClient.Delete(new IdMessage { Id = _viewModel.SelecteOrderLine.Id }, headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || !result.Result)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("An error accured and the data couldn't be deleted. Please try again laiter and make sure, there is no related data to this entry!");
                return;
            }

            LoadNewOrderLineData();
        }

        public void ExecuteAddOrderLineCommand(object o)
        {

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var orderLineController = _app.Container.Resolve<OrderLineController>();
            var newOrderLine = orderLineController.AddOrderLine(MainWindowController.Token);

            if (newOrderLine is null) 
            {
                _popupWindowController.DisplayText("You haven't selected an article. This orderline wasn't saved!");
                return;
            }

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            var position = _viewModel.OrderLines.
                OrderBy(o => o.Position)
                .LastOrDefault()?.Position + 1;

            IdMessage idMessage;
            try
            {
                idMessage = _orderLineProtoClient.Add(new OrderLineRequest
                {
                    Amount = newOrderLine.Amount,
                    Id = newOrderLine.Id,
                    Article = new IdMessage { Id = newOrderLine.Article.Id },
                    Order = new IdMessage { Id = _viewModel.SelectedModel.Id },
                    Position = position.GetValueOrDefault()
                }, headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (idMessage is null || idMessage.Id < 0)
            {
                _popupWindowController.DisplayText("The data coundn't be saved. Please make sure to satisfy your unique constraints!");
                return;
            }

            LoadNewOrderLineData();
        }

        public bool CanExecuteOrderLineCommand(object o)
        {
            return _viewModel.OrderLineSelected;
        }

        public bool CanExecuteSelectedCommand(object o)
        {
            return _viewModel.ItemSelected && !_viewModel.NewItem;
        }

        public bool CanExecuteSaveCommand(object o)
        {
            return _viewModel.ViewDetail;
        }

        public void LoadNewData()
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            OrderResponses result;

            try
            {
                result = _orderProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || result.Status is Shared.Protos.Status.Failed)
            {
                var _popupWindowController = _app.Container.Resolve<PopupWindowController>();
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            _viewModel.Models.Clear();

            foreach (var message in result.Orders)
            {
                try
                {
                    var customer = _customerProtoClient.Get(message.Customer, headers);
                    var employee = _employeeProtoClient.Get(message.Employee, headers);

                    if (customer is null || employee is null || customer.Status is Shared.Protos.Status.Failed || employee.Status is Shared.Protos.Status.Failed)
                    {
                        continue;
                    }

                    _viewModel.Models.Add(new Order
                    {
                        Customer = new Customer
                        {
                            Name = customer.Name,
                            City = customer.City,
                            Firstname = customer.Firstname,
                            Id = customer.Id,
                            Phone = customer.Phone,
                            PostalCode = customer.PostalCode,
                            Street = customer.Street,
                            StreetNumber = customer.StreetNumber
                        },
                        Employee = new Employee
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            EmployeeNumber = employee.EmployeeNumber,
                            Id = employee.Id
                        },
                        Description = message.Description,
                        Id = message.Id,
                        OrderNumber = message.OrderNumber,
                        OrderDate = new DateTime
                            (
                                message.OrderDate.Year,
                                message.OrderDate.Month,
                                message.OrderDate.Day,
                                message.OrderDate.Hour,
                                message.OrderDate.Minute,
                                message.OrderDate.Second
                            )
                    });
                } catch (Exception)
                {
                    continue;
                }
            }
        }

        private void LoadNewOrderLineData()
        {

            var _popupWindowController = _app.Container.Resolve<PopupWindowController>();

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {MainWindowController.Token}");

            OrderLineResponses result;
            ArticleResponses articles;

            try
            {
                result = _orderLineProtoClient.GetAll(new Empty(), headers);
                articles = _articleProtoClient.GetAll(new Empty(), headers);
            }
            catch (Exception)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            if (result is null || result.Status is Shared.Protos.Status.Failed)
            {
                _popupWindowController.DisplayText("A server error accured. Please try again laiter!");
                return;
            }

            _viewModel.OrderLines.Clear();

            foreach (var message in result.OrderLines)
            {
                if (message.Order.Id != _viewModel.SelectedModel.Id) continue;

                var article = new Article();

                foreach (var a in articles.Articles)
                {
                    if (a.Id == message.Article.Id)
                    {
                        article = new Article
                        {
                            Id = a.Id,
                            ArticleNumber = a.ArticleNumber,
                            Description = a.Description,
                            Name = a.Name,
                            Price = a.Price
                        };
                        break;
                    }
                }

                _viewModel.OrderLines.Add(new OrderLineHelper
                {
                    Article = article,
                    Amount = message.Amount,
                    Id = message.Id,
                    Position = message.Position
                }); ;
            }
        }
    }
}
