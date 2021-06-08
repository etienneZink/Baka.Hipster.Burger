namespace Baka.Hipster.Burger.Shared.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string ArticleNumber { get; set; } //ToDo Länge 50
        public string Name { get; set; } //ToDo Länge 50
        public string Description { get; set; } //ToDo Länge 100 ToDo Nullable
        public decimal Price { get; set; } //ToDo (16,2) in DB --> heißt?!
        public int Version { get; set; }
    }
}