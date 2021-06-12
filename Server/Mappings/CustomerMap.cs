using Baka.Hipster.Burger.Shared.Models;
using FluentNHibernate.Mapping;

namespace Baka.Hipster.Burger.Server.Mappings
{
    public class CustomerMap: ClassMap<Customer>
    {
        public CustomerMap()
        {
            Table("Customers");
            Id(c =>c.Id)
                .GeneratedBy.Native()
                .Default(0)
                .Not.Nullable();

            Map(c => c.Type);
            Map(c => c.Name)
                .Length(50)
                .Not.Nullable();
            Map(c => c.Firstname)
                .Length(50);
            Map(c => c.Phone)
                .Unique()
                .Length(50)
                .Not.Nullable();
            Map(c => c.Street)
                .Length(50)
                .Not.Nullable();
            Map(c => c.StreetNumber)
                .Length(5)
                .Not.Nullable();
            Map(c => c.PostalCode)
                .Not.Nullable();
            Map(c => c.City)
                .Length(50)
                .Not.Nullable();

            Version(c => c.Version)
                .Not.Nullable();
            OptimisticLock.Version();

            DynamicUpdate();
        }
    }
}