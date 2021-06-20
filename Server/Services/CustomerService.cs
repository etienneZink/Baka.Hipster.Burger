using System;
using System.Linq;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class CustomerService: CustomerProto.CustomerProtoBase
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IOrderRepository _orderRepository;

        public CustomerService(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }
        
        [Authorize]
        public override async Task<IdMessage> Add(CustomerRequest request, ServerCallContext context)
        {
            if (request is null) return new IdMessage { Id = -1 };

            var customer = new Customer
            {
                City = request.City,
                Firstname = request.Firstname,
                Name = request.Name,
                Phone = request.Phone,
                PostalCode = request.PostalCode,
                Street = request.Street,
                StreetNumber = request.StreetNumber,
            };

            return await _customerRepository.NewOrUpdate(customer) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = customer.Id };
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };
            if (!await IsDeletable(request.Id)) return new BoolResponse { Result = false };

            return new BoolResponse { Result = await _customerRepository.Delete(request.Id) };
        }

        [Authorize]
        public override async Task<BoolResponse> CanDelete(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };

            return new BoolResponse { Result = await IsDeletable(request.Id) };
        }

        [Authorize]
        public override async Task<BoolResponse> Update(CustomerRequest request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };

            var customer = await _customerRepository.Get(request.Id);
            if (customer is null) return new BoolResponse { Result = false };

            customer.City = request.City;
            customer.Firstname = request.Firstname;
            customer.Name = request.Name;
            customer.Phone = request.Phone;
            customer.PostalCode = request.PostalCode;
            customer.Street = request.Street;
            customer.StreetNumber = request.StreetNumber;

            return await _customerRepository.NewOrUpdate(customer) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [Authorize]
        public override async Task<CustomerResponse> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new CustomerResponse { Status = Shared.Protos.Status.Failed };

            var customer = await _customerRepository.Get(request.Id);
            if (customer is null) return new CustomerResponse { Status = Shared.Protos.Status.Failed };

            return new CustomerResponse
            {
                City = customer.City,
                Firstname = customer.Firstname,
                Name = customer.Name,
                Phone = customer.Phone,
                PostalCode = customer.PostalCode,
                Street = customer.Street,
                StreetNumber = customer.StreetNumber,
                Id = customer.Id,
                Status = Shared.Protos.Status.Ok
            };
        }

        [Authorize]
        public override async Task<CustomerResponses> GetAll(Empty request, ServerCallContext context)
        {
            var customerMessages = new CustomerResponses { Status = Shared.Protos.Status.Failed };

            if (request is null) return customerMessages;

            var customers = await _customerRepository.GetAll();
            if (customers is null) return customerMessages;

            foreach (var customer in customers)
            {
                customerMessages.Customers.Add(new CustomerResponse
                {
                    City = customer.City,
                    Firstname = customer.Firstname,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    PostalCode = customer.PostalCode,
                    Street = customer.Street,
                    StreetNumber = customer.StreetNumber,
                    Id = customer.Id,
                    Status = Shared.Protos.Status.Ok
                });
            }

            customerMessages.Status = Shared.Protos.Status.Ok;
            return customerMessages;
        }

        private async Task<bool> IsDeletable(int Id)
        {
            var customer = await _customerRepository.Get(Id);
            if (customer is null) return false;

            var orders = await _orderRepository.GetAll();
            if (orders is null) return false;

            return orders.All(x => x.Customer.Id != Id);
        }
    }
}