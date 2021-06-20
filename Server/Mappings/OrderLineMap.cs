using Baka.Hipster.Burger.Shared.Models;
using FluentNHibernate.Mapping;

namespace Baka.Hipster.Burger.Server.Mappings
{
    public class OrderLineMap: ClassMap<OrderLine>
    {
        public OrderLineMap()
        {
            Table("OrderLines");
            Id(c => c.Id)
                .GeneratedBy.Native()
                .Default(0)
                .Not.Nullable()
                .UniqueKey("Position");

            Map(o => o.Amount)
                .Not.Nullable();
            Map(o => o.Position)
                .Not.Nullable()
                .UniqueKey("Position")
                .Generated.Insert();
            /*References(o => o.Order)
                .Column("OrderId")
                .LazyLoad()
                .Cascade.All()
                .Not.Nullable();*/
            References(o => o.Article)
                .Column("ArticleId")
                .LazyLoad()
                .Cascade.All()
                .Not.Nullable();

            DynamicUpdate();
            
        }
    }
}