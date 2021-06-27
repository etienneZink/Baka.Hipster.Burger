using Baka.Hipster.Burger.Server.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
