namespace Baka.Hipster.Burger.Shared.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } //ToDo Länge 50
        public string LastName { get; set; } //ToDo Länge 50
        public int EmployeeNumber { get; set; } //ToDo Unique constraint in DB
        public int Version { get; set; }
    }
}