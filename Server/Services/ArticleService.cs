using System;
using System.Linq;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class ArticleService: ArticleProto.ArticleProtoBase
    {
        private readonly IArticleRepository _articleRepository;

        private readonly IOrderLineRepository _orderLineRepository;

        public ArticleService(IArticleRepository articleRepository, IOrderLineRepository orderLineRepository)
        {
            _articleRepository = articleRepository;
            _orderLineRepository = orderLineRepository;
        }
        
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(ArticleRequest request, ServerCallContext context)
        {
            if (request is null) return new IdMessage { Id = -1 };

            var article = new Article
            {
                Description = request.Description ?? string.Empty,
                ArticleNumber = request.ArticleNumber ?? string.Empty,
                Name = request.Name ?? string.Empty,
                Price = request.Price
            };

            return await _articleRepository.NewOrUpdate(article) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = article.Id };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse{ Result = false };
            if (! await IsDeletable(request.Id)) return new BoolResponse { Result = false };

            return new BoolResponse { Result = await _articleRepository.Delete(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> CanDelete(IdMessage request, ServerCallContext context)
        {
            return request is null ? new BoolResponse { Result = false } : new BoolResponse { Result = await IsDeletable(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(ArticleRequest request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };

            var article = await _articleRepository.Get(request.Id);
            if (article is null) return new BoolResponse { Result = false };

            article.Description = request.Description ?? string.Empty;
            article.ArticleNumber = request.ArticleNumber ?? string.Empty;
            article.Name = request.Name ?? string.Empty;
            article.Price = request.Price;

            return await _articleRepository.NewOrUpdate(article) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [Authorize]
        public override async Task<ArticleResponse> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new ArticleResponse { Status = Shared.Protos.Status.Failed };

            var article = await _articleRepository.Get(request.Id);
            if (article is null) return new ArticleResponse { Status = Shared.Protos.Status.Failed };

            return new ArticleResponse
            {
                ArticleNumber = article.ArticleNumber ?? string.Empty,
                Description = article.Description ?? string.Empty,
                Id = article.Id,
                Price = article.Price,
                Name = article.Name ?? string.Empty,
                Status = Shared.Protos.Status.Ok
            };
        }

        [Authorize]
        public override async Task<ArticleResponses> GetAll(Empty request, ServerCallContext context)
        {
            var articleMessages = new ArticleResponses { Status = Shared.Protos.Status.Failed };
            if (request is null) return articleMessages;

            var articles = await _articleRepository.GetAll();
            if (articles is null) return articleMessages;

            foreach (var article in articles)
            {
                articleMessages.Articles.Add(new ArticleResponse
                {
                    ArticleNumber = article.ArticleNumber ?? string.Empty,
                    Description = article.Description ?? string.Empty,
                    Id = article.Id,
                    Price = article.Price,
                    Name = article.Name ?? string.Empty,
                    Status = Shared.Protos.Status.Ok
                });
            }

            articleMessages.Status = Shared.Protos.Status.Ok;
            return articleMessages;
        }

        private async Task<bool> IsDeletable(int Id)
        {
            var article = await _articleRepository.Get(Id);
            if (article is null) return false;

            var orderLines = await _orderLineRepository.GetAll();
            if (orderLines is null) return false;

            return orderLines.All(x => x.Article.Id != Id);
        }
    }
}