using Baka.Hipster.Burger.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baka.Hipster.Burger.Client.Helper
{
    public class AreaHelper
    {
        public bool InEmployee { get; set; }
        public Area Area { get; set; }

        public int PostCode => Area.PostCode;

        public string Description => Area.Description;
    }
}
