using System;
using System.Linq;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using FluentNHibernate.Conventions;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class EmployeeService: EmployeeProto.EmployeeProtoBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IAreaRepository _areaRepository;

        private readonly IOrderRepository _orderRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IAreaRepository areaRepository, IOrderRepository orderRepository)
        {
            _employeeRepository = employeeRepository;
            _areaRepository = areaRepository;
            _orderRepository = orderRepository;
        }
        
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(EmployeeRequest request, ServerCallContext context)
        {
            if (request?.Areas is null) return new IdMessage { Id = -1 };

            var employee = new Employee
            {
               EmployeeNumber = request.EmployeeNumber,
               FirstName = request.FirstName ?? string.Empty,
               LastName = request.LastName ?? string.Empty
            };

            foreach (var areaId in request.Areas)
            {
                var area = await _areaRepository.Get(areaId.Id);
                if (area is not null) employee.Areas.Add(area);
            }

            return await _employeeRepository.NewOrUpdate(employee) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = employee.Id };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };
            if (!await IsDeletable(request.Id)) return new BoolResponse { Result = false };

            return new BoolResponse { Result = await _employeeRepository.Delete(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> CanDelete(IdMessage request, ServerCallContext context)
        {
            return request is null ? new BoolResponse { Result = false } : new BoolResponse { Result = await IsDeletable(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(EmployeeRequest request, ServerCallContext context)
        {
            if (request?.Areas is null) return new BoolResponse { Result = false };

            var employee = await _employeeRepository.Get(request.Id);
            if (employee is null) return new BoolResponse { Result = false };

            employee.EmployeeNumber = request.EmployeeNumber;
            employee.FirstName = request.FirstName ?? string.Empty;
            employee.LastName = request.LastName ?? string.Empty;
            employee.Areas.Clear();//ToDo check if it works

            foreach (var areaId in request.Areas)
            {
                var area = await _areaRepository.Get(areaId.Id);
                if (area is not null) employee.Areas.Add(area);
            }

            return await _employeeRepository.NewOrUpdate(employee) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [Authorize("Admin")]
        public override async Task<EmployeeResponse> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new EmployeeResponse { Status = Shared.Protos.Status.Failed };

            var employee = await _employeeRepository.Get(request.Id);
            if (employee is null) return new EmployeeResponse { Status = Shared.Protos.Status.Failed };

            var employeeMessage = new EmployeeResponse()
            {
                EmployeeNumber = employee.EmployeeNumber,
                FirstName = employee.FirstName ?? string.Empty,
                LastName = employee.LastName ?? string.Empty,
                Id = employee.Id,
            };

            foreach (var area in employee.Areas)
            {
                employeeMessage.Areas.Add(new IdMessage { Id = area.Id });
            }

            employeeMessage.Status = Shared.Protos.Status.Ok;
            return employeeMessage;
        }

        [Authorize("Admin")]
        public override async Task<EmployeeResponses> GetAll(Empty request, ServerCallContext context)
        {
            var employeeMessages = new EmployeeResponses { Status = Shared.Protos.Status.Failed };

            if (request is null) return employeeMessages;

            var employees = await _employeeRepository.GetAll();
            if (employees is null) return employeeMessages;

            foreach (var employee in employees)
            {
                var employeeMessage = new EmployeeResponse()
                {
                    EmployeeNumber = employee.EmployeeNumber,
                    FirstName = employee.FirstName ?? string.Empty,
                    LastName = employee.LastName ?? string.Empty,
                    Id = employee.Id,
                    Status = Shared.Protos.Status.Ok
                };

                foreach (var area in employee.Areas)
                {
                    employeeMessage.Areas.Add(new IdMessage { Id = area.Id });
                }
                
                employeeMessages.Employees.Add(employeeMessage);
            }

            employeeMessages.Status = Shared.Protos.Status.Ok;
            return employeeMessages;
        }

        private async Task<bool> IsDeletable(int Id)
        {
            var employee = await _employeeRepository.Get(Id);
            if (employee is null) return false;

            var orders = await _orderRepository.GetAll();
            if (orders is null) return false;

            return orders.All(x => x.Employee.Id != Id) && employee.Areas.IsEmpty();
        }
    }
}