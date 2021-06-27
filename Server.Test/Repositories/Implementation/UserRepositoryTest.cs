using Baka.Hipster.Burger.Server.Helper.Interfaces;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;
using System;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Server.Test.Repositories.Implementation
{
    [TestClass]
    public class UserRepositoryTest
    {
        //LoginTest5
        [TestMethod]
        public async Task GetAll_ThrowsException_Fails()
        {
            var sessionMock = new Mock<ISession>();
            sessionMock
                .Setup(s => s.QueryOver<User>())
                .Throws(new Exception());

            var nhibernateHelperMock = new Mock<INHibernateHelper>();
            nhibernateHelperMock
                .Setup(n => n.OpenSession())
                .Returns(sessionMock.Object);


            var userRepository = new UserRepository(nhibernateHelperMock.Object);
            var result = await userRepository.GetAll();


            sessionMock.VerifyAll();
            nhibernateHelperMock.VerifyAll();
            Assert.IsNull(result);
        }
    }
}
