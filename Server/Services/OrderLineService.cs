using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class OrderLineService: OrderLineProto.OrderLineProtoBase
    {
        //ToDo implement
        [Authorize]
        public override async Task<IdMessage> Add(OrderLineMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<OrderLineMessage> Get(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<OrderLineMessages> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}