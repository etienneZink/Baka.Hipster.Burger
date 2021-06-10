namespace Baka.Hipster.Burger.Shared.Models
{
    public class OrderLine
    {
        public virtual int Id { get; set; }
        public virtual int Amount { get; set; }
        public virtual int Position { get; set; }
        public virtual Order Order { get; set; }
        public virtual Article Article { get; set; }
    }
}