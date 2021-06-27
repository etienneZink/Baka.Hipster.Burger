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
    public class EmployeeServiceTests
    {
        //manageEmployee1
        [TestMethod]
        public async Task Delete_RequestNull_Fails()
        {
            var employeeService = new EmployeeService(null, null, null);
            var result = await employeeService.Delete(null, null);


            Assert.IsFalse(result.Result);
        }

        //manageEmployee2
        [TestMethod]
        public async Task Delete_NotDeletable_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Employee)null);


            var employeeService = new EmployeeService(employeeRepositoryMock.Object, null, null);
            var result = await employeeService.Delete(requestMock, null);


            Assert.IsFalse(result.Result);
        }

        //manageAreas3
        [TestMethod]
        public async Task IsDeletable_EmployeeNull_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync((Employee)null);


            var employeeService = new EmployeeService(employeeRepositoryMock.Object, null, null);
            var result = await employeeService.CanDelete(requestMock, null);


            employeeRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageAreas4
        [TestMethod]
        public async Task IsDeletable_OrdersNull_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var employeeMock = new Employee
            {
                Id = idMock
            };

            var areaMock = new Area();

            employeeMock.Areas.Add(areaMock);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(employeeMock);

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync((ICollection<Order>) null);


            var employeeService = new EmployeeService(employeeRepositoryMock.Object, null, orderRepositoryMock.Object);
            var result = await employeeService.CanDelete(requestMock, null);


            orderRepositoryMock.VerifyAll();
            employeeRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }

        //manageEmployee5
        [TestMethod]
        public async Task IsDeletable_AreasNotEmpty_Fails()
        {
            var idMock = 1;

            var requestMock = new IdMessage
            {
                Id = idMock
            };

            var employeeMock = new Employee
            {
                Id = idMock
            };

            var areaMock = new Area();

            employeeMock.Areas.Add(areaMock);

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock
                .Setup(m => m.Get(idMock))
                .ReturnsAsync(employeeMock);

            var ordersMock = new List<Order>();

            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .ReturnsAsync(ordersMock);


            var employeeService = new EmployeeService(employeeRepositoryMock.Object, null, orderRepositoryMock.Object);
            var result = await employeeService.CanDelete(requestMock, null);


            employeeRepositoryMock.VerifyAll();
            Assert.IsFalse(result.Result);
        }
    }
}
