namespace Baka.Hipster.Burger.Shared.Models
{
    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual int Type { get; set; }
        public virtual string Name { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Street { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual int PostalCode { get; set; }
        public virtual string City { get; set; }
        public virtual int Version { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Customer a)
            {
                return a.Id == Id;
            }
            return base.Equals(obj);
        }
    }
}