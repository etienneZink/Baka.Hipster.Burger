namespace Baka.Hipster.Burger.Shared.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Description { get; set; } //ToDo Länge 50
        public int PostCode { get; set; } //ToDo Unique
        public int Version { get; set; }
    }
}