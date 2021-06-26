using System;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using NHibernate.Criterion;

namespace Baka.Hipster.Burger.Server.Services
{
    public class OrderLineService: OrderLineProto.OrderLineProtoBase
    {

        private readonly IOrderLineRepository _orderLineRepository;

        private readonly IArticleRepository _articleRepository;

        private readonly IOrderRepository _orderRepository;

        public OrderLineService(IOrderLineRepository orderLineRepository, IArticleRepository articleRepository, IOrderRepository orderRepository)
        {
            _orderLineRepository = orderLineRepository;
            _articleRepository = articleRepository;
            _orderRepository = orderRepository;
        }

        [Authorize]
        public override async Task<IdMessage> Add(OrderLineRequest request, ServerCallContext context)
        {
            if (request?.Article is null || request.Order is null) return new IdMessage { Id = -1 };

            var article = await _articleRepository.Get(request.Article.Id);
            var order = await _orderRepository.Get(request.Order.Id);
            if(article is null || order is null) return new IdMessage { Id = -1 };

            var orderLine = new OrderLine
            {
                Amount = request.Amount,
                Article = article,
                Order = order,
                Position = request.Position
            };

            return await _orderLineRepository.NewOrUpdate(orderLine) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = orderLine.Id };
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            return request is null ? new BoolResponse { Result = false } : new BoolResponse { Result = await _orderLineRepository.Delete(request.Id) };
        }

        [Authorize]
        public override async Task<OrderLineResponse> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new OrderLineResponse { Status = Shared.Protos.Status.Failed };

            var orderLine = await _orderLineRepository.Get(request.Id);
            if (orderLine is null) return new OrderLineResponse { Status = Shared.Protos.Status.Failed };

            var orderLineMessage = new OrderLineResponse 
            {
                Id = orderLine.Id,
                Position = orderLine.Position,
                Article = new IdMessage { Id = orderLine.Article.Id },
                Order = new IdMessage { Id = orderLine.Order.Id},
                Amount = orderLine.Amount,
                Status = Shared.Protos.Status.Ok
            };

            return orderLineMessage;
        }

        [Authorize]
        public override async Task<OrderLineResponses> GetAll(Empty request, ServerCallContext context)
        {
            var orderLineMessages = new OrderLineResponses { Status = Shared.Protos.Status.Failed };

            if (request is null) return orderLineMessages;

            var orderLines = await _orderLineRepository.GetAll();
            if (orderLines is null) return orderLineMessages;

            foreach (var orderLine in orderLines)
            {
                orderLineMessages.OrderLines.Add(new OrderLineResponse
                {
                    Id = orderLine.Id,
                    Position = orderLine.Position,
                    Article = new IdMessage { Id = orderLine.Article.Id },
                    Order = new IdMessage { Id = orderLine.Order.Id },
                    Amount = orderLine.Amount,
                    Status = Shared.Protos.Status.Ok
                });
            }

            orderLineMessages.Status = Shared.Protos.Status.Ok;
            return orderLineMessages;
        }
    }
}