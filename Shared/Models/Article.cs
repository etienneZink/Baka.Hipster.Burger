namespace Baka.Hipster.Burger.Shared.Models
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual string ArticleNumber { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual double Price { get; set; }
        public virtual int Version { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Article a)
            {
                return a.Id == Id;
            }
            return base.Equals(obj);
        }
    }
}