using Baka.Hipster.Burger.Shared.Models;
using FluentNHibernate.Mapping;

namespace Baka.Hipster.Burger.Server.Mappings
{
    public class UserMap: ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(c => c.Id)
                .GeneratedBy.Native()
                .Default(0)
                .Not.Nullable();

            Map(u => u.Username)
                .Length(20)
                .Unique()
                .Not.Nullable();
            Map(u => u.Firstname)
                .Length(50);
            Map(u => u.Lastname)
                .Length(50)
                .Not.Nullable();
            Map(u => u.Password)
                .Length(60)
                .Not.Nullable();
            Map(u => u.IsAdmin)
                .Not.Nullable();

            Version(c => c.Version)
                .Not.Nullable();
            OptimisticLock.Version();

            DynamicUpdate();
        }
    }
}