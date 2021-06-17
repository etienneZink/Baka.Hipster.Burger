using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class AreaService: AreaProto.AreaProtoBase
    {
        //ToDo implement
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(AreaMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(AreaMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<AreaMessage> Get(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<AreaMessages> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}