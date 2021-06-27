using Baka.Hipster.Burger.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Helper
{
    public class OrderLineHelper
    {
        private OrderLine _orderLine;

        public OrderLine OrderLine
        {
            get => _orderLine ??= new OrderLine();
            set => _orderLine = value;
        }

        public int Id 
        {
            get => OrderLine.Id;
            set => OrderLine.Id = value; 
        }

        public int Amount 
        {
            get => OrderLine.Amount;
            set => OrderLine.Amount = value; 
        }

        public int Position 
        {
            get => OrderLine.Position;
            set => OrderLine.Position = value; 
        }

        public Order Order 
        {
            get => OrderLine.Order;
            set => OrderLine.Order = value; 
        }

        public Article Article 
        {
            get => OrderLine.Article;
            set => OrderLine.Article = value; 
        }

        public string ArticleNumber => Article.ArticleNumber;

        public string ArticleName => Article.Name;

        public double ArticlePrice => Article.Price;
    }
}
