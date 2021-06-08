namespace Baka.Hipster.Burger.Shared.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int Position { get; set; }
        public Order Order { get; set; } //ToDo in DB OrderId als int und FK
        public Article Article { get; set; } //ToDo in DB ArticleId als int und FK
    }
}