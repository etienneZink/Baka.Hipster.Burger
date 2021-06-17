using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class OrderService: OrderProto.OrderProtoBase
    {
        //ToDo implement
        [Authorize]
        public override async Task<IdMessage> Add(OrderMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> Update(OrderMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<OrderMessage> Get(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<OrderMessages> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}