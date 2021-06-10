using Baka.Hipster.Burger.Shared.Models;
using FluentNHibernate.Mapping;

namespace Baka.Hipster.Burger.Server.Mappings
{
    public class ArticleMap: ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");
            Id(a => a.Id)
                .GeneratedBy.Native()
                .Default(0)
                .Not.Nullable();

            Map(a => a.ArticleNumber)
                .Length(50)
                .Unique()
                .Not.Nullable();
            Map(a => a.Name)
                .Length(50)
                .Not.Nullable();
            Map(a => a.Description)
                .Length(100);
            Map(a => a.Price)
                .Not.Nullable();
            Map(a => a.Version)
                .Not.Nullable();

            Version(a => a.Version)
                .Not.Nullable();
            OptimisticLock.Version();

            DynamicUpdate();
        }
    }
}