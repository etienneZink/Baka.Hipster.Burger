namespace Baka.Hipster.Burger.Shared.Models
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual string ArticleNumber { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual int Version { get; set; }
    }
}