﻿using Baka.Hipster.Burger.Server.Repositories.Interfaces;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAreaRepository _areaRepository;

        public QueryService(IArticleRepository articleRepository, IAreaRepository areaRepository, 
                            IOrderLineRepository orderLineRepository, IOrderRepository orderRepository, 
                            IEmployeeRepository employeeRepository)
        {
            _articleRepository = articleRepository;
            _areaRepository = areaRepository;
            _orderLineRepository = orderLineRepository;
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
        }


        [Authorize]
        public override async Task<AreaRanking> GetAreaRanking(Empty request, ServerCallContext context)
        {
            throw new NotImplementedException();
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
