using System;
using System.Linq;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Shared.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Baka.Hipster.Burger.Server.Services
{
    public class ArticleService: ArticleProto.ArticleProtoBase
    {
        private readonly ArticleRepository _articleRepository;

        private readonly OrderLineRepository _orderLineRepository;

        public ArticleService(ArticleRepository articleRepository, OrderLineRepository orderLineRepository)
        {
            _articleRepository = articleRepository;
            _orderLineRepository = orderLineRepository;
        }
        
        [Authorize("Admin")]
        public override async Task<IdMessage> Add(ArticleMessage request, ServerCallContext context)
        {
            if (request is null) return new IdMessage { Id = -1 };

            var article = new Article
            {
                Description = request?.Description,
                ArticleNumber = request?.ArticleNumber,
                Name = request?.Name,
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
            if (request is null) return new BoolResponse { Result = false };
            return new BoolResponse { Result = await IsDeletable(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(ArticleMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };

            var article = await _articleRepository.Get(request.Id);
            if (article is null) return new BoolResponse { Result = false };

            article.Description = request?.Description;
            article.ArticleNumber = request?.ArticleNumber;
            article.Name = request?.Name;
            article.Price = request.Price;

            return await _articleRepository.NewOrUpdate(article) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [Authorize]
        public override async Task<ArticleMessage> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new ArticleMessage();

            var article = await _articleRepository.Get(request.Id);
            if (article is null) return new ArticleMessage();

            return new ArticleMessage
            {
                ArticleNumber = article.ArticleNumber,
                Description = article.Description,
                Id = article.Id,
                Price = article.Price,
                Name = article.Name
            };
        }

        [Authorize]
        public override async Task<ArticleMessages> GetAll(Empty request, ServerCallContext context)
        {
            var articleMessages = new ArticleMessages();
            if (request is null) return articleMessages;

            var articles = await _articleRepository.GetAll();
            if (articles is null) return articleMessages;

            foreach (var article in articles)
            {
                articleMessages.Articles.Add(new ArticleMessage
                {
                    ArticleNumber = article.ArticleNumber,
                    Description = article.Description,
                    Id = article.Id,
                    Price = article.Price,
                    Name = article.Name
                });
            }

            return articleMessages;
        }

        private async Task<bool> IsDeletable(int Id)
        {
            var article = await _articleRepository.Get(Id);
            if (article is null) return false;

            var orderLines = await _orderLineRepository.GetAll();
            if (orderLines is null) return false;

            return orderLines.Any(x => x.Article.Id == Id);
        }
    }
}