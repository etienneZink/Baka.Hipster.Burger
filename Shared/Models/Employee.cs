using System.Collections;
using System.Collections.Generic;

namespace Baka.Hipster.Burger.Shared.Models
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int EmployeeNumber { get; set; }
        public virtual int Version { get; set; }

        private IList<Area> _areas;
        public virtual IList<Area> Areas => _areas ??= new List<Area>();

        public override bool Equals(object obj)
        {
            if (obj is Employee a)
            {
                return a.Id == Id;
            }
            return base.Equals(obj);
        }

        public override string ToString() => LastName + " " + FirstName;
    }
}