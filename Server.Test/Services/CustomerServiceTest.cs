using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Server.Services;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Server.Test.Services
{
    [TestClass]
    public class CustomerServiceTest
    {
        //manageCustomer1
        [TestMethod]
        public async Task Delete_RequestNull_Fails()
        {
            var customerService = new CustomerService(null, null);
            var result = await customerService.Delete(null, null);


            Assert.IsFalse(result.Result);
        }

        //manageCustomer2
        [TestMethod]
        public async Task Delete_NotDeletable_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Customer)null);


            var customerService = new CustomerService(customerRepositoryMock.Object, null);
            var result = await customerService.Delete(requestMock, null);


            customerRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageCustomer3
        [TestMethod]
        public async Task IsDeletable_CustomerNull_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Customer)null);


            var customerService = new CustomerService(customerRepositoryMock.Object, null);
            var result = await customerService.CanDelete(requestMock, null);


            customerRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageCustomer4
        [TestMethod]
        public async Task IsDeletable_OrdersNull_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var customerMock = new Customer
            {
                Id = idMock
            };

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(customerMock);

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<Order>)null);


            var articleService = new CustomerService(customerRepositoryMock.Object, orderRepositoryMock.Object);
            var result = await articleService.CanDelete(requestMock, null);


            customerRepositoryMock.VerifyAll();
            orderRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageCustomer5
        [TestMethod]
        public async Task IsDeletable_OrderWithCustomer_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var customerMock = new Customer
            {
                Id = idMock
            };

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(customerMock);

            var orderLinesMock = new List<Order>
            {
                new Order
                {
                    Customer = customerMock
                }
            };

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync(orderLinesMock);


            var articleService = new CustomerService(customerRepositoryMock.Object, orderRepositoryMock.Object);
            var result = await articleService.CanDelete(requestMock, null);


            customerRepositoryMock.VerifyAll();
            orderRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }
    }
}
