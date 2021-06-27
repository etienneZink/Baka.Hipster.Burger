using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Server.Services;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Server.Test.Services
{
    [TestClass]
    public class UserServiceTest
    {
        //LoginTest
        [TestMethod]
        public async Task LoginRequestNull_Fails()
        {
            var userService = new UserService(null,null);
            var result = await userService.LogIn(null,null);


            Assert.AreEqual(result.Status, Status.Failed);
        }

        //LoginTest
        [TestMethod]
        public async Task LoginUsersNull_Fails()
        {
            var passwordMock = "Password";
            var userNameMock = "UserName";
            var requestMock = new UserLogin
            {
                Password = passwordMock,
                Username = userNameMock
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<User>) null);


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.LogIn(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.AreEqual(result.Status, Status.Failed);
        }

        //LoginTest
        [TestMethod]
        public async Task LoginUserNull_Fails()
        {
            var passwordMock = "Password";
            var userNameMock = "UserName";
            var requestMock = new UserLogin
            {
                Password = passwordMock,
                Username = userNameMock
            };

            var userMockName = "Mock";
            var userMockPassword = passwordMock;
            var userMock = new User
            {
                Username = userMockName,
                Password = BCrypt.Net.BCrypt.HashPassword(userMockPassword)
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync(new List<User> { userMock });


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.LogIn(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.AreEqual(result.Status, Status.Failed);
        }

        //LoginTest
        [TestMethod]
        public async Task LoginUserNameCaseUnsensitive_Success()
        {
            var passwordMock = "Password";
            var userNameMock = "UserName";
            var requestMock = new UserLogin
            {
                Password = passwordMock,
                Username = userNameMock
            };

            var userMockName = userNameMock;
            var userMockPassword = passwordMock;
            var userMockId = 1;
            var userMock = new User
            {
                Username = userMockName,
                Password = BCrypt.Net.BCrypt.HashPassword(userMockPassword),
                Id = userMockId
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync(new List<User> { userMock });

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .SetupGet(c => c[It.IsAny<string>()])
                .Returns("This is a demo secret, please change in production!");
            

            var userService = new UserService(userRepositoryMock.Object, configurationMock.Object);
            var result = await userService.LogIn(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.AreEqual(result.Status, Status.Ok);
            Assert.AreEqual(result.User.Username, userMockName);
            Assert.AreEqual(result.User.Id, userMockId);
            Assert.IsNotNull(result.Token);
        }
    }
}
