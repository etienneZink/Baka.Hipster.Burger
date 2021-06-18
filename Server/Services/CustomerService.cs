using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class CustomerService: CustomerProto.CustomerProtoBase
    {
        //ToDo implement
        [Authorize]
        public override async Task<IdMessage> Add(CustomerMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> CanDelete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> Update(CustomerMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<CustomerMessage> Get(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<CustomerMessages> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}