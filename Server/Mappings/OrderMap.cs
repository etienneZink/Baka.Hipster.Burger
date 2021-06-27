using Baka.Hipster.Burger.Shared.Models;
using FluentNHibernate.Mapping;

namespace Baka.Hipster.Burger.Server.Mappings
{
    public class OrderMap: ClassMap<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            Id(c => c.Id)
                .GeneratedBy.Native()
                .Default(0)
                .Not.Nullable();

            Map(o => o.OrderNumber)
                .Length(50)
                .Unique()
                .Not.Nullable();

            Map(o => o.OrderDate)
                .Not.Nullable();

            Map(o => o.Description)
                .Length(100);
           
            References(o => o.Employee)
                .Column("EmployeeId")
                .LazyLoad()
                .Cascade.SaveUpdate()
                .Not.Nullable();

            References(o => o.Customer)
                .Column("CustomerId")
                .LazyLoad()
                .Cascade.SaveUpdate()
                .Not.Nullable();

            HasMany(o => o.OrderLines)
                .Cascade.All()
                .Cascade.DeleteOrphan()
                .Not.KeyNullable()
                .KeyColumn("Id")
                .Inverse();

            Version(c => c.Version).Not.Nullable();
            OptimisticLock.Version();

            DynamicUpdate();
        }
    }
}