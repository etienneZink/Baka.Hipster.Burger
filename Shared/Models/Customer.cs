namespace Baka.Hipster.Burger.Shared.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int Type { get; set; } //ToDo schauen ob nicht Enum mit int-type
        public string Name { get; set; } //ToDo Länge 50
        public string Firstname { get; set; } //ToDo Länge 50 ToDo Nullable
        public string Phone { get; set; } //ToDo Länge 50
        public string Street { get; set; } //ToDo Länge 50
        public string StreetNumber { get; set; } //ToDo max. Länge ist 5!
        public int PostalCode { get; set; }
        public string City { get; set; } //ToDo Länge 50
        public int Version { get; set; }
    }
}