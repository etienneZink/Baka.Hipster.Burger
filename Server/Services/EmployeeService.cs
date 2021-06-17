using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class EmployeeService: EmployeeProto.EmployeeProtoBase
    {
        //ToDo implement
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(EmployeeMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(EmployeeMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<EmployeeMessage> Get(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<EmployeeMessages> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}