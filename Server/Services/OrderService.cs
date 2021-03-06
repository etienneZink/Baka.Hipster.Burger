using System;
using System.Linq;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class OrderService: OrderProto.OrderProtoBase
    {
        private readonly IOrderRepository _orderRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IOrderLineRepository _orderLineRepository;

        public OrderService(
            IOrderRepository orderRepository, ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository, IOrderLineRepository orderLineRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _orderLineRepository = orderLineRepository;
        }

        [Authorize]
        public override async Task<IdMessage> Add(OrderRequest request, ServerCallContext context)
        {
            if (request?.Customer is null || request.Employee is null || request.OrderDate is null) return new IdMessage { Id = -1 };

            var customer = await _customerRepository.Get(request.Customer.Id);
            var employee = await _employeeRepository.Get(request.Employee.Id);
            var orderDate = new DateTime(request.OrderDate.Year, request.OrderDate.Month, request.OrderDate.Day,
                                         request.OrderDate.Hour, request.OrderDate.Minute, request.OrderDate.Second,
                                         request.OrderDate.Millisecond);

            if (customer is null || employee is null) return new IdMessage { Id = -1 };

            var order = new Order
            {
                Description = request.Description ?? string.Empty,
                OrderNumber = request.OrderNumber ?? string.Empty,
                Customer = customer,
                Employee = employee,
                OrderDate = orderDate
            };
            
            return await _orderRepository.NewOrUpdate(order) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = order.Id };
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            
            if (request is null) return new BoolResponse { Result = false };

            var order = await _orderRepository.Get(request.Id);
            if (order is null || order.OrderLines is null) return new BoolResponse { Result = false };

            var orderLines = await _orderLineRepository.GetAll();
            if (orderLines is null) return new BoolResponse { Result = false };

            foreach (var orderLine in orderLines)
            {
                if (order.Id != orderLine.Order.Id) continue;
                await _orderLineRepository.Delete(orderLine.Id);
            }

            return new BoolResponse { Result = await _orderRepository.Delete(request.Id) };
        }

        [Authorize]
        public override async Task<BoolResponse> Update(OrderRequest request, ServerCallContext context)
        {
            if (request?.Customer is null || request.Employee is null || request.OrderDate is null) return new BoolResponse { Result = false };

            var order = await _orderRepository.Get(request.Id);
            if (order is null) return new BoolResponse { Result = false };

            var customer = await _customerRepository.Get(request.Customer.Id);
            var employee = await _employeeRepository.Get(request.Employee.Id);
            var orderDate = new DateTime(request.OrderDate.Year, request.OrderDate.Month, request.OrderDate.Day,
                request.OrderDate.Hour, request.OrderDate.Minute, request.OrderDate.Second);

            if (customer is null || employee is null) return new BoolResponse { Result = false };

            order.Description = request.Description ?? string.Empty;
            order.OrderNumber = request.OrderNumber ?? string.Empty;
            order.Customer = customer;
            order.Employee = employee;
            order.OrderDate = orderDate;

            return await _orderRepository.NewOrUpdate(order) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true } ;
        }

        [Authorize]
        public override async Task<OrderResponse> Get(IdMessage request, ServerCallContext context)
        {
            var orderMessageResponse = new OrderResponse { Status = Shared.Protos.Status.Failed};

            if (request is null) return orderMessageResponse;

            var order = await _orderRepository.Get(request.Id);
            if (order is null) return orderMessageResponse;

            var customer = await _customerRepository.Get(order.Customer.Id);
            var employee = await _employeeRepository.Get(order.Employee.Id);
            if (customer is null || employee is null) return orderMessageResponse;

            orderMessageResponse.Description = order.Description ?? string.Empty;
            orderMessageResponse.Id = order.Id;
            orderMessageResponse.OrderNumber = order.OrderNumber ?? string.Empty;
            orderMessageResponse.OrderDate = new DateTimeMessage
            {
                Year = order.OrderDate.Year,
                Month = order.OrderDate.Month,
                Day = order.OrderDate.Day,
                Hour = order.OrderDate.Hour,
                Minute = order.OrderDate.Minute,
                Second = order.OrderDate.Second,
                Millisecond = order.OrderDate.Millisecond
            };
            orderMessageResponse.Customer = new IdMessage { Id = customer.Id };
            orderMessageResponse.Employee = new IdMessage { Id = employee.Id };

            foreach (var orderOrderLine in order.OrderLines)
            {
                orderMessageResponse.OrderLines.Add(new IdMessage { Id = orderOrderLine.Id });
            }

            orderMessageResponse.Status = Shared.Protos.Status.Ok;
            return orderMessageResponse;
        }

        [Authorize]
        public override async Task<OrderResponses> GetAll(Empty request, ServerCallContext context)
        {
            var orderMessageResponses = new OrderResponses { Status = Shared.Protos.Status.Failed };

            if (request is null) return orderMessageResponses;

            var orders = await _orderRepository.GetAll();
            if (orders is null) return orderMessageResponses;

            foreach (var order in orders)
            {
                var customer = await _customerRepository.Get(order.Customer.Id);
                var employee = await _employeeRepository.Get(order.Employee.Id);
                if (customer is null || employee is null) continue;

                var orderMessageResponse = new OrderResponse
                {
                    Description = order.Description ?? string.Empty,
                    Id = order.Id,
                    OrderNumber = order.OrderNumber ?? string.Empty,
                    OrderDate = new DateTimeMessage
                    {
                        Year = order.OrderDate.Year,
                        Month = order.OrderDate.Month,
                        Day = order.OrderDate.Day,
                        Hour = order.OrderDate.Hour,
                        Minute = order.OrderDate.Minute,
                        Second = order.OrderDate.Second,
                        Millisecond = order.OrderDate.Millisecond
                    },
                    Customer = new IdMessage {Id = customer.Id},
                    Employee = new IdMessage {Id = employee.Id},
                    Status = Shared.Protos.Status.Ok
                };

                foreach (var orderOrderLine in order.OrderLines)
                {
                    orderMessageResponse.OrderLines.Add(new IdMessage { Id = orderOrderLine.Id });
                }

                orderMessageResponses.Orders.Add(orderMessageResponse);
            }

            orderMessageResponses.Status = Shared.Protos.Status.Ok;
            return orderMessageResponses;
        }
    }
}