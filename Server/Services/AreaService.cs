﻿using System;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Shared.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using FluentNHibernate.Conventions;

namespace Baka.Hipster.Burger.Server.Services
{
    public class AreaService: AreaProto.AreaProtoBase
    {
        private readonly IAreaRepository _areaRepository;

        private readonly IEmployeeRepository _employeeRepository;

        public AreaService(IAreaRepository areaRepository, IEmployeeRepository employeeRepository)
        {
            _areaRepository = areaRepository;
            _employeeRepository = employeeRepository;
        }
        
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(AreaMessage request, ServerCallContext context)
        {
            if(request?.Employees is null) return new IdMessage { Id = -1 };

            var area = new Area
            {
                Description = request.Description,
                PostCode = request.PostCode,
            };
            
            foreach (var employeeId in request.Employees)
            {
                var employee = await _employeeRepository.Get(employeeId.Id);
                if(employee is not null) area.Employees.Add(employee);
            }

            return await _areaRepository.NewOrUpdate(area) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = area.Id };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };
            if (!await IsDeletable(request.Id)) return new BoolResponse { Result = false };

            return new BoolResponse { Result = await _areaRepository.Delete(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> CanDelete(IdMessage request, ServerCallContext context)
        {
            return request is null ? new BoolResponse { Result = false } : new BoolResponse { Result = await IsDeletable(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(AreaMessage request, ServerCallContext context)
        {
            if (request?.Employees is null) return new BoolResponse { Result = false };

            var area = await _areaRepository.Get(request.Id);
            if (area is null) return new BoolResponse { Result = false };

            area.Description = request.Description;
            area.PostCode = area.PostCode;
            area.Employees.Clear();//ToDo check if it works

            foreach (var employeeId in request.Employees)
            {
                var employee = await _employeeRepository.Get(employeeId.Id);
                if (employee is not null) area.Employees.Add(employee);
            }

            return await _areaRepository.NewOrUpdate(area) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [Authorize("Admin")]
        public override async Task<AreaMessage> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new AreaMessage();

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

            if (request is null) return areaMessages;
            
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
            return area is not null && area.Employees.IsEmpty();
        }
    }
}