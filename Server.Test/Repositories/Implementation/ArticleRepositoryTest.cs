using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Server.Test.Repositories.Implementation
{
    [TestClass]
    public class ArticleRepositoryTest
    {
        //bestseller4
        [TestMethod]
        public async Task GetAll_ThrowsException_Fails()
        {
            var sessionMock = new Mock<ISession>();
            sessionMock
                .Setup(s => s.QueryOver<Article>())
                .Throws(new Exception());

            var nhibernateHelperMock = new Mock<INHibernateHelper>();
            nhibernateHelperMock
                .Setup(n => n.OpenSession())
                .Returns(sessionMock.Object);


            var articleRepository = new ArticleRepository(nhibernateHelperMock.Object);
            var result = await articleRepository.GetAll();


            sessionMock.VerifyAll();
            nhibernateHelperMock.VerifyAll();
            Assert.IsNull(result);
        }
    }
}
