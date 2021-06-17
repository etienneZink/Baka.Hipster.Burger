using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class ArticleService: ArticleProto.ArticleProtoBase
    {
        //ToDo implement
        [Authorize]
        public override async Task<IdMessage> Add(ArticleMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<BoolResponse> Update(ArticleMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<ArticleMessage> Get(IdMessage request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public override async Task<ArticleMessages> GetAll(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}