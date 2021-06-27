using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Server.Services;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Server.Test.Services
{
    [TestClass]
    public class AreaServiceTest
    {
        //manageAreas1
        [TestMethod]
        public async Task Delete_RequestNull_Fails()
        {
            var areaService = new AreaService(null, null);
            var result = await areaService.Delete(null, null);


            Assert.IsFalse(result.Result);
        }

        //manageAreas2
        [TestMethod]
        public async Task Delete_NotDeletable_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var areaRepositoryMock = new Mock<IAreaRepository>();
            areaRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Area)null);


            var areaService = new AreaService(areaRepositoryMock.Object, null);
            var result = await areaService.Delete(requestMock, null);


            areaRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageAreas3
        [TestMethod]
        public async Task IsDeletable_AreaNull_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var areaRepositoryMock = new Mock<IAreaRepository>();
            areaRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Area) null);


            var areaService = new AreaService(areaRepositoryMock.Object, null);
            var result = await areaService.Delete(requestMock, null);


            areaRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageAreas4
        [TestMethod]
        public async Task IsDeletable_EmployeesNotEmpty_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var employeeMock = new Employee();

            var areaMock = new Area
            {
                Id = idMock
            };

            areaMock.Employees.Add(employeeMock);

            var areaRepositoryMock = new Mock<IAreaRepository>();
            areaRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(areaMock);


            var areaService = new AreaService(areaRepositoryMock.Object, null);
            var result = await areaService.Delete(requestMock, null);


            areaRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageAreas5
        [TestMethod]
        public async Task IsDeletable_Succeeds()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var areaMock = new Area
            {
                Id = idMock
            };

            var areaRepositoryMock = new Mock<IAreaRepository>();
            areaRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(areaMock);


            var areaService = new AreaService(areaRepositoryMock.Object, null);
            var result = await areaService.Delete(requestMock, null);


            areaRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }
    }
}
