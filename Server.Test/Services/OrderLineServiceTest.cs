using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Server.Services;
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
    public class OrderLineServiceTest
    {
        //manageOrders4
        [TestMethod]
        public async Task Delete_RequestNull_Fails()
        {
            var orderLineService = new OrderLineService(null, null, null);
            var result = await orderLineService.Delete(null, null);


            Assert.IsFalse(result.Result);
        }

        //manageOrders5
        [TestMethod]
        public async Task Delete_SaveFails_Fails()
        {
            var idMock = 1;
            var request = new IdMessage { Id = idMock };

            var orderLineRepository = new Mock<IOrderLineRepository>();
            orderLineRepository
                .Setup(m => m.Delete(idMock))
                .ReturnsAsync(false);


            var orderLineService = new OrderLineService(orderLineRepository.Object, null, null);
            var result = await orderLineService.Delete(request, null);


            orderLineRepository.VerifyAll();
            Assert.IsFalse(result.Result);
        }
    }
}
