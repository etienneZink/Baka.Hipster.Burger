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
    public class QueryServiceTest
    {
        //ranking1
        [TestMethod]
        public async Task GetAreaRanking_RequestNull_Fails()
        {
            var queryService = new QueryService(null,null,null);
            var result = await queryService.GetAreaRanking(null,null);


            Assert.AreEqual(result.Status, Status.Failed);
        }

        //ranking2
        [TestMethod]
        public async Task GetAreaRanking_ArticlesNull_Fails()
        {
            var articleRepository = new Mock<IArticleRepository>();
            articleRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<Article>) null);

            var orderLineRepository = new Mock<IOrderLineRepository>();
            orderLineRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<OrderLine>)null);

            var areaRepository = new Mock<IAreaRepository>();
            areaRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<Area>)null);


            var queryService = new QueryService(articleRepository.Object, areaRepository.Object, orderLineRepository.Object);
            var result = await queryService.GetAreaRanking(new Empty(), null);


            articleRepository.VerifyAll();
            orderLineRepository.VerifyAll();
            areaRepository.VerifyAll();
            Assert.AreEqual(result.Status, Status.Failed);
        }

        //ranking3
        [TestMethod]
        public async Task GetAreaRanking_OrderLinesNull_Fails()
        {
            var articleRepository = new Mock<IArticleRepository>();
            articleRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync(new List<Article>());

            var orderLineRepository = new Mock<IOrderLineRepository>();
            orderLineRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<OrderLine>) null);

            var areaRepository = new Mock<IAreaRepository>();
            areaRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<Area>)null);


            var queryService = new QueryService(articleRepository.Object, areaRepository.Object, orderLineRepository.Object);
            var result = await queryService.GetAreaRanking(new Empty(), null);


            articleRepository.VerifyAll();
            orderLineRepository.VerifyAll();
            areaRepository.VerifyAll();
            Assert.AreEqual(result.Status, Status.Failed);
        }

        //ranking4
        [TestMethod]
        public async Task GetAreaRanking_AreasNull_Fails()
        {
            var articleRepository = new Mock<IArticleRepository>();
            articleRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync(new List<Article>());

            var orderLineRepository = new Mock<IOrderLineRepository>();
            orderLineRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync(new List<OrderLine>());

            var areaRepository = new Mock<IAreaRepository>();
            areaRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<Area>)null);


            var queryService = new QueryService(articleRepository.Object, areaRepository.Object, orderLineRepository.Object);
            var result = await queryService.GetAreaRanking(new Empty(), null);


            articleRepository.VerifyAll();
            orderLineRepository.VerifyAll();
            areaRepository.VerifyAll();
            Assert.AreEqual(result.Status, Status.Failed);
        }

        //bestseller1
        [TestMethod]
        public async Task GetBestSeller_RequestNull_Fails()
        {
            var queryService = new QueryService(null, null, null);
            var result = await queryService.GetBestSeller(null, null);


            Assert.AreEqual(result.Status, Status.Failed);
        }

        //bestseller2
        [TestMethod]
        public async Task GetBestSeller_ArticlesNull_Fails()
        {
            var articleRepository = new Mock<IArticleRepository>();
            articleRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<Article>)null);

            var orderLineRepository = new Mock<IOrderLineRepository>();
            orderLineRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<OrderLine>)null);


            var queryService = new QueryService(articleRepository.Object, null,  orderLineRepository.Object);
            var result = await queryService.GetBestSeller(new Empty(), null);


            articleRepository.VerifyAll();
            orderLineRepository.VerifyAll();
            Assert.AreEqual(result.Status, Status.Failed);
        }

        //bestseller3
        [TestMethod]
        public async Task GetBestSeller_OrderLinesNull_Fails()
        {
            var articleRepository = new Mock<IArticleRepository>();
            articleRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync(new List<Article>());

            var orderLineRepository = new Mock<IOrderLineRepository>();
            orderLineRepository
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<OrderLine>)null);


            var queryService = new QueryService(articleRepository.Object, null, orderLineRepository.Object);
            var result = await queryService.GetBestSeller(new Empty(), null);


            articleRepository.VerifyAll();
            orderLineRepository.VerifyAll();
            Assert.AreEqual(result.Status, Status.Failed);
        }
    }
}
