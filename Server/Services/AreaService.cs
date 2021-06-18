using System;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Shared.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using FluentNHibernate.Conventions;

namespace Baka.Hipster.Burger.Server.Services
{
    public class AreaService: AreaProto.AreaProtoBase
    {
        private readonly AreaRepository _areaRepository;
        private readonly EmployeeRepository _employeeRepository;

        public AreaService(AreaRepository areaRepository, EmployeeRepository employeeRepository)
        {
            _areaRepository = areaRepository;
            _employeeRepository = employeeRepository;
        }
        
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(AreaMessage request, ServerCallContext context)
        {
            var area = new Area
            {
                Description = request?.Description,
                PostCode = request.PostCode,
            };

            Employee employee;
            foreach (var employeeId in request.Employees)
            {
                employee = await _employeeRepository.Get(employeeId.Id);
                if(employee is not null) area.Employees.Add(employee);
            }

            if (await _areaRepository.NewOrUpdate(area) < 0) return new IdMessage { Id = -1 };

            return new IdMessage { Id = area.Id };
            
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            if (!await IsDeletable(request.Id)) return new BoolResponse { Result = false };
            return new BoolResponse { Result = await _areaRepository.Delete(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> CanDelete(IdMessage request, ServerCallContext context)
        {
            return new BoolResponse { Result = await IsDeletable(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(AreaMessage request, ServerCallContext context)
        {
            var area = await _areaRepository.Get(request.Id);
            if (area is null) return new BoolResponse { Result = false };

            area.Description = request?.Description;
            area.PostCode = area.PostCode;
            area.Employees.Clear();

            foreach (var employeeId in request.Employees)
            {
                var employee = await _employeeRepository.Get(employeeId.Id);
                if (employee is not null) area.Employees.Add(employee);
            }

            if (await _areaRepository.NewOrUpdate(area) < 0) return new BoolResponse { Result = false };

            return new BoolResponse { Result = true };
        }

        [Authorize("Admin")]
        public override async Task<AreaMessage> Get(IdMessage request, ServerCallContext context)
        {
            var area = await _areaRepository.Get(request.Id);
            if (area is null) return new AreaMessage();

            var areaMessage = new AreaMessage
            {
                Id = area.Id,
                Description = area.Description,
                PostCode = area.PostCode
            };

            foreach (var employee in area.Employees)
            {
                areaMessage.Employees.Add(new IdMessage { Id = employee.Id });
            }

            return areaMessage;
        }

        [Authorize("Admin")]
        public override async Task<AreaMessages> GetAll(Empty request, ServerCallContext context)
        {
            var areaMessages = new AreaMessages();

            var areas = await _areaRepository.GetAll();
            if (areas is null) return areaMessages;

            foreach (var area in areas)
            {
                var areaMessage = new AreaMessage
                {
                    Id = area.Id,
                    Description = area.Description,
                    PostCode = area.PostCode,
                };

                foreach (var employee in area.Employees)
                {
                    areaMessage.Employees.Add(new IdMessage { Id = employee.Id });
                }

                areaMessages.Areas.Add(areaMessage);
            }

            return areaMessages;
        }

        private async Task<bool> IsDeletable(int Id)
        {
            var area = await _areaRepository.Get(Id);
            if (area is null) return false;

            return area.Employees.IsEmpty();
        }
    }
}