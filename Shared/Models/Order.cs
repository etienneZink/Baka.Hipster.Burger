using System;
using System.Collections.Generic;

namespace Baka.Hipster.Burger.Shared.Models
{
    public class Order
    {
        public virtual int Id { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual DateTime OrderDate { get; set; }
        public virtual string Description { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual int Version { get; set; }

        private IList<OrderLine> _orderLines;

        public virtual IList<OrderLine> OrderLines => _orderLines ??= new List<OrderLine>();
    }
}