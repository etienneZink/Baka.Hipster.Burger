using System;

namespace Baka.Hipster.Burger.Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } //ToDo Länge 50
        public DateTime OrderDate { get; set; }
        public string Description { get; set; } //ToDo Länge 100 ToDo Nullable
        public Employee Employee { get; set; } //ToDo in DB EmployeeId als int und FK
        public Customer Customer { get; set; } //ToDo in DB CustomerId als int und FK
        public int Version { get; set; }
    }
}