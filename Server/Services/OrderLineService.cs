using System;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
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
        public override async Task<IdMessage> Add(OrderLineMessage request, ServerCallContext context)
        {
            if (request?.Article is null || request.Order is null) return new IdMessage { Id = -1 };

            var article = await _articleRepository.Get(request.Article.Id);
            var order = await _orderRepository.Get(request.Order.Id);
            if(article is null || order is null) return new IdMessage { Id = -1 };

            var orderLine = new OrderLine
            {
                Amount = request.Amount,
                Article = article,
                Order = order
            };

            return await _orderLineRepository.NewOrUpdate(orderLine) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = orderLine.Id };
        }

        [Authorize]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            return request is null ? new BoolResponse { Result = false } : new BoolResponse { Result = await _orderLineRepository.Delete(request.Id) };
        }

        [Authorize]
        public override async Task<OrderLineMessage> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new OrderLineMessage();

            var orderLine = await _orderLineRepository.Get(request.Id);
            if (orderLine is null) return new OrderLineMessage();

            var orderLineMessage = new OrderLineMessage 
            {
                Id = orderLine.Id,
                Position = orderLine.Position,
                Article = new IdMessage { Id = orderLine.Article.Id },
                Order = new IdMessage { Id = orderLine.Order.Id},
                Amount = orderLine.Amount
            };

            return orderLineMessage;
        }

        [Authorize]
        public override async Task<OrderLineMessages> GetAll(Empty request, ServerCallContext context)
        {
            var orderLineMessages = new OrderLineMessages();

            if (request is null) return orderLineMessages;

            var orderLines = await _orderLineRepository.GetAll();
            if (orderLines is null) return orderLineMessages;

            foreach (var orderLine in orderLines)
            {
                orderLineMessages.OrderLines.Add(new OrderLineMessage
                {
                    Id = orderLine.Id,
                    Position = orderLine.Position,
                    Article = new IdMessage { Id = orderLine.Article.Id },
                    Order = new IdMessage { Id = orderLine.Order.Id },
                    Amount = orderLine.Amount
                });
            }

            return orderLineMessages;
        }
    }
}