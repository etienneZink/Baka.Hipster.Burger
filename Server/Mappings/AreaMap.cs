using Baka.Hipster.Burger.Shared.Models;
using FluentNHibernate.Mapping;

namespace Baka.Hipster.Burger.Server.Mappings
{
    public class AreaMap: ClassMap<Area>
    {
        public AreaMap()
        {
            Table("Areas");
            Id(a  => a.Id)
                .GeneratedBy.Native()
                .Default(0)
                .Not.Nullable();

            Map(a => a.Description)
                .Length(50)
                .Not.Nullable();
            Map(a => a.PostCode)
                .Unique()
                .Not.Nullable();
            
            HasManyToMany(a => a.Employees)
                .Table("EmployeeToAreaRelations")
                .ParentKeyColumn("EmployeeId")
                .ChildKeyColumn("AreaId")
                .Inverse();

            Version(a => a.Version)
                .Not.Nullable();

            OptimisticLock.Version();
        }
    }
}