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
        //LoginTest1
        [TestMethod]
        public async Task Login_RequestNull_Fails()
        {
            var userService = new UserService(null,null);
            var result = await userService.LogIn(null,null);


            Assert.AreEqual(result.Status, Status.Failed);
        }

        //LoginTest2
        [TestMethod]
        public async Task Login_UsersNull_Fails()
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

        //LoginTest3
        [TestMethod]
        public async Task Login_UserNull_Fails()
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

        //LoginTest4
        [TestMethod]
        public async Task Login_UserNameCaseUnsensitive_Success()
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

        //changePassword1
        [TestMethod]
        public async Task ChangePassword_RequestNull_Fails()
        {
            var userService = new UserService(null, null);
            var result = await userService.ChangePassword(null, null);


            Assert.IsFalse(result.Result);
        }

        //changePassword2
        [TestMethod]
        public async Task ChangePassword_UserNull_Fails()
        {
            var newPasswordMock = "NewPassword";
            var oldPasswordMock = "OldPassword";
            var idMock = 1;
            var requestMock = new ChangePasswordRequest
            {
                Id = idMock,
                NewPassword = newPasswordMock,
                OldPassword = oldPasswordMock
                
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((User) null);


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.ChangePassword(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //changePassword3
        [TestMethod]
        public async Task ChangePassword_WrongPassword_Fails()
        {
            var newPasswordMock = "NewPassword";
            var oldPasswordMock = "OldPassword";
            var idMock = 1;
            var requestMock = new ChangePasswordRequest
            {
                Id = idMock,
                NewPassword = newPasswordMock,
                OldPassword = oldPasswordMock

            };

            var userPasswordMock = "Password";     
            var userMock = new User
            {
                Id = idMock,
                Password = BCrypt.Net.BCrypt.HashPassword(userPasswordMock)
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(userMock);


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.ChangePassword(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //changePassword4
        [TestMethod]
        public async Task ChangePassword_UpdateFails_Fails()
        {
            var newPasswordMock = "NewPassword";
            var oldPasswordMock = "OldPassword";
            var idMock = 1;
            var requestMock = new ChangePasswordRequest
            {
                Id = idMock,
                NewPassword = newPasswordMock,
                OldPassword = oldPasswordMock

            };

            var userPasswordMock = "OldPassword";
            var userMock = new User
            {
                Id = idMock,
                Password = BCrypt.Net.BCrypt.HashPassword(userPasswordMock)
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(userMock);

            userRepositoryMock
                .Setup(m => m.NewOrUpdate(userMock))
                .ReturnsAsync(-1);


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.ChangePassword(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //changePassword5
        [TestMethod]
        public async Task ChangePassword_UpdateSuccess_Succeeds()
        {
            var newPasswordMock = "NewPassword";
            var oldPasswordMock = "OldPassword";
            var idMock = 1;
            var requestMock = new ChangePasswordRequest
            {
                Id = idMock,
                NewPassword = newPasswordMock,
                OldPassword = oldPasswordMock

            };

            var userPasswordMock = "OldPassword";
            var userMock = new User
            {
                Id = idMock,
                Password = BCrypt.Net.BCrypt.HashPassword(userPasswordMock)
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(userMock);

            userRepositoryMock
                .Setup(m => m.NewOrUpdate(userMock))
                .ReturnsAsync(idMock);


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.ChangePassword(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.IsTrue(result.Result);
        }

        //manageUser1
        [TestMethod]
        public async Task AddUser_RequestNull_Fails()
        {
            var userService = new UserService(null, null);
            var result = await userService.Add(null, null);


            Assert.AreEqual(result.Id, -1);
        }

        //manageUser2
        [TestMethod]
        public async Task AddUser_UserNull_Fails()
        {
            var passwordMock = "Password";
            var requestMock = new FullUserRequest
            {
                User = null,
                Password = passwordMock

            };


            var userService = new UserService(null, null);
            var result = await userService.Add(requestMock, null);


            Assert.AreEqual(result.Id, -1);
        }

        //manageUser3
        [TestMethod]
        public async Task AddUser_SaveFails_Fails()
        {
            var usernameMock = "Username";
            var adminMock = false;

            var userMock = new User
            {
                Username = usernameMock,
                IsAdmin = adminMock
            };

            var passwordMock = "Password";
            var requestMock = new FullUserRequest
            {
                User = new UserRequest
                {
                    Username = usernameMock,
                    IsAdmin = adminMock
                },
                Password = passwordMock

            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.NewOrUpdate(userMock))
                .ReturnsAsync(-1);


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.Add(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.AreEqual(result.Id, -1);
        }

        //manageUser4
        [TestMethod]
        public async Task AddUser_SaveSuccess_Succeeds()
        {
            var usernameMock = "Username";
            var adminMock = false;
            var idMock = 0;

            var userMock = new User
            {
                Username = usernameMock,
                IsAdmin = adminMock
            };

            var passwordMock = "Password";
            var requestMock = new FullUserRequest
            {
                User = new UserRequest
                {
                    Username = usernameMock,
                    IsAdmin = adminMock,
                },
                Password = passwordMock

            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(m => m.NewOrUpdate(userMock))
                .ReturnsAsync(idMock);


            var userService = new UserService(userRepositoryMock.Object, null);
            var result = await userService.Add(requestMock, null);


            userRepositoryMock.VerifyAll();
            Assert.AreEqual(result.Id, idMock);
        }

        //manageUser5
        [TestMethod]
        public async Task DeleteUser_RequestNull_Fails()
        {
            var userService = new UserService(null, null);
            var result = await userService.Delete(null, null);


            Assert.IsFalse(result.Result);
        }
    }
}
