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
    public class ArticleServiceTests
    {
        //manageArticle1
        [TestMethod]
        public async Task Delete_RequestNull_Fails()
        {
            var articleService = new ArticleService(null, null);
            var result = await articleService.Delete(null, null);


            Assert.IsFalse(result.Result);
        }

        //manageArticle2
        [TestMethod]
        public async Task Delete_NotDeletable_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var articleRepositoryMock = new Mock<IArticleRepository>();
            articleRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Article)null);


            var articleService = new ArticleService(articleRepositoryMock.Object, null);
            var result = await articleService.Delete(requestMock, null);


            articleRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageArticle3
        [TestMethod]
        public async Task IsDeletable_ArticleNull_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var articleRepositoryMock = new Mock<IArticleRepository>();
            articleRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Article)null);


            var articleService = new ArticleService(articleRepositoryMock.Object, null);
            var result = await articleService.CanDelete(requestMock, null);


            articleRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageArticle4
        [TestMethod]
        public async Task IsDeletable_OrderLinesNull_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var articleMock = new Article
            {
                Id = idMock
            };

            var articlesRepositoryMock = new Mock<IArticleRepository>();
            articlesRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(articleMock);

            var orderLineRepositoryMock = new Mock<IOrderLineRepository>();
            orderLineRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<OrderLine>)null);


            var articleService = new ArticleService(articlesRepositoryMock.Object, orderLineRepositoryMock.Object);
            var result = await articleService.CanDelete(requestMock, null);


            articlesRepositoryMock.VerifyAll();
            orderLineRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageArticle5
        [TestMethod]
        public async Task IsDeletable_OrderLineWithArticle_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var articleMock = new Article
            {
                Id = idMock
            };

            var articlesRepositoryMock = new Mock<IArticleRepository>();
            articlesRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(articleMock);

            var orderLinesMock = new List<OrderLine>
            {
                new OrderLine 
                { 
                    Article = articleMock
                }
            };

            var orderLineRepositoryMock = new Mock<IOrderLineRepository>();
            orderLineRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync(orderLinesMock);


            var articleService = new ArticleService(articlesRepositoryMock.Object, orderLineRepositoryMock.Object);
            var result = await articleService.CanDelete(requestMock, null);


            articlesRepositoryMock.VerifyAll();
            orderLineRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }
    }
}
