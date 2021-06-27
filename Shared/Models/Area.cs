using System.Collections;
using System.Collections.Generic;

namespace Baka.Hipster.Burger.Shared.Models
{
    public class Area
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual int PostCode { get; set; }
        public virtual int Version { get; set; }

        private IList<Employee> _employees;
        public virtual IList<Employee> Employees => _employees ??= new List<Employee>();

        public override bool Equals(object obj)
        {
            if (obj is Area a)
            {
                return a.Id == Id;
            }
            return base.Equals(obj);
        }
    }
}