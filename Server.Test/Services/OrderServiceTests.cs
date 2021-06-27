using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Server.Services;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Server.Test.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        //manageOrders1
        [TestMethod]
        public async Task Delete_RequestNull_Fails()
        {
            var orderService = new OrderService(null,null,null,null);
            var result = await orderService.Delete(null, null);


            Assert.IsFalse(result.Result);
        }

        //manageOrders2
        [TestMethod]
        public async Task Delete_OrderNull_Fails()
        {
            var idMock = 1;
            var request = new IdMessage { Id = idMock };

            var orderRepository = new Mock<IOrderRepository>();
            orderRepository
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Order) null);


            var orderService = new OrderService(orderRepository.Object, null, null, null);
            var result = await orderService.Delete(request, null);


            orderRepository.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageOrders3
        [TestMethod]
        public async Task Delete_OrderLinesNull_Fails()
        {
            var idMock = 1;
            var request = new IdMessage { Id = idMock };

            var orderRepository = new Mock<IOrderRepository>();
            orderRepository
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(new Order());

            var orderLineRepository = new Mock<IOrderLineRepository>();
            orderLineRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<OrderLine>) null);


            var orderService = new OrderService(orderRepository.Object, null, null, orderLineRepository.Object);
            var result = await orderService.Delete(request, null);


            orderRepository.VerifyAll();
            orderLineRepository.VerifyAll();
            Assert.IsFalse(result.Result);
        }
    }
}
