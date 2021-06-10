using System;

namespace Baka.Hipster.Burger.Shared.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; } //ToDo Länge 20 ToDo Unique
        public virtual string Firstname { get; set; } //ToDo Länge 50 ToDo Nullable
        public virtual string Lastname { get; set; } //ToDo Länge 50
        public virtual string Password { get; set; } //ToDo Länge 60  ToDo Hash in DB speichern!
        public virtual bool IsAdmin { get; set; }
        public virtual int Version { get; set; }
    }
}