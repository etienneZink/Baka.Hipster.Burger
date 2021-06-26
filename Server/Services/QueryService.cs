using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Server.Services
{
    public class QueryService: QueryProto.QueryProtoBase
    {

        private readonly IArticleRepository _articleRepository;
        private readonly IOrderLineRepository _orderLineRepository;
         private readonly IAreaRepository _areaRepository;

        public QueryService(IArticleRepository articleRepository, IAreaRepository areaRepository, 
                            IOrderLineRepository orderLineRepository)
        {
            _articleRepository = articleRepository;
            _areaRepository = areaRepository;
            _orderLineRepository = orderLineRepository;
        }


        [Authorize]
        public override async Task<AreaRanking> GetAreaRanking(Empty request, ServerCallContext context)
        {
            var areaRankingMessages = new AreaRanking { Status = Shared.Protos.Status.Failed };
            if (request is null) return areaRankingMessages;

            var articles = await _articleRepository.GetAll();
            var orderLines = await _orderLineRepository.GetAll();
            var areas = await _areaRepository.GetAll();

            if (articles is null || orderLines is null || areas is null) return areaRankingMessages;

            var orders = orderLines
                .GroupBy(o => o.Order)
                .Select(x => new
                {
                    Order = x.Key,
                    Turnover = x.Sum(e => e.Amount * e.Article.Price),
                    Employee = x.Key.Employee
                });

            var employees = orders
                .GroupBy(x => x.Employee)
                .Select(x => new { 
                    Employee = x.Key,
                    Turnover = x.Sum(x => x.Turnover)
                })
                .ToArray();

            var areaQuery = areas
                .Select(a => new AreaQuery
                {
                    Description = a.Description,
                    PostCode = a.PostCode,
                    Status = Shared.Protos.Status.Ok,
                    Turnover = 0.0
                })
                .ToList();
            
            foreach (var employee in employees)
            {
                foreach (var area in employee.Employee.Areas) 
                {
                    var a = areaQuery.FirstOrDefault(x => x.PostCode == area.PostCode);
                    if (a is null) continue;
                    a.Turnover += (employee.Turnover / employee.Employee.Areas.Count);
                }
            }

            var resultQuery = areaQuery
                .OrderByDescending(a => a.Turnover);

            int i = 0;
            foreach (var q in resultQuery) 
            {
                q.Rank = ++i;
                areaRankingMessages.Areas.Add(q);
            }

            areaRankingMessages.Status = Shared.Protos.Status.Ok;
            return areaRankingMessages;
        }


        [Authorize]
        public override async Task<BestSeller> GetBestSeller(Empty request, ServerCallContext context)
        {
            var bestSellerMessages = new BestSeller { Status = Shared.Protos.Status.Failed };
            if (request is null) return bestSellerMessages;

            var articles = await _articleRepository.GetAll();
            var orderLines = await _orderLineRepository.GetAll();
            if (articles is null || orderLines is null) return bestSellerMessages;

            var query = orderLines
                .GroupBy(x => x.Article)
                .Select(x => new
                {
                    Name = x.Key.Name,
                    ArticleNumber = x.Key.ArticleNumber,
                    Amount = x.Sum(e => e.Amount),
                    Turnover = x.Sum(e => e.Amount) * x.Key.Price
                })
                .OrderByDescending(q => q.Amount)
                .ToArray();

            int i = 0;
            foreach (var q in query)
            {
                bestSellerMessages.Articles.Add(new ArticleQuery
                {
                    Status = Shared.Protos.Status.Ok,
                    Name = q.Name,
                    ArticleNumber = q.ArticleNumber,
                    Amount = q.Amount,
                    Turnover = q.Turnover,
                    Rank = ++i
                });
            }

            bestSellerMessages.Status = Shared.Protos.Status.Ok;
            return bestSellerMessages;
        }
    }
}
