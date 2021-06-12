using Baka.Hipster.Burger.Shared.Models;
using FluentNHibernate.Mapping;

namespace Baka.Hipster.Burger.Server.Mappings
{
    public class EmployeeMap: ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employees");
            Id(c => c.Id).GeneratedBy.Native().Default(0).Not.Nullable();

            Map(e => e.FirstName)
                .Length(50)
                .Not.Nullable();
            Map(e => e.LastName)
                .Length(50)
                .Not.Nullable();
            Map(e => e.EmployeeNumber)
                .Unique()
                .Not.Nullable();

            HasManyToMany(e => e.Areas)
                .Table("EmployeeToAreaRelations")
                .ParentKeyColumn("EmployeeId")
                .ChildKeyColumn("AreaId")
                .LazyLoad()
                .Cascade.SaveUpdate();

            Version(c => c.Version)
                .Not.Nullable();
            OptimisticLock.Version();

            DynamicUpdate();
        }
    }
}