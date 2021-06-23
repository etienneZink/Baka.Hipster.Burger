using System;

namespace Baka.Hipster.Burger.Shared.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual int Version { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is User a)
            {
                return a.Id == Id;
            }
            return base.Equals(obj);
        }
    }
}