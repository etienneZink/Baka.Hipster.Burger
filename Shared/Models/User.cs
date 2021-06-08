using System;

namespace Baka.Hipster.Burger.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } //ToDo Länge 20 ToDo Unique
        public string Firstname { get; set; } //ToDo Länge 50 ToDo Nullable
        public string Lastname { get; set; } //ToDo Länge 50
        public string Password { get; set; } //ToDo Länge 60  ToDo Hash in DB speichern!
        public bool IsAdmin { get; set; }
        public int Version { get; set; }
    }
}