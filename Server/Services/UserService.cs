using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class UserService: UserProto.UserProtoBase
    {
        //ToDo implement
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(FullUser request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(UserMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<UserMessage> Get(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        public override async Task<UserMessages> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> ChangePassword(FullUser request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        public override async Task<TokenMessage> LogIn(UserLogin request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        public override async Task<BoolResponse> Register(FullUser request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}