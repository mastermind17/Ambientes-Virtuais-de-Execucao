using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    public class PropertyAssociation<TSrc>
    {
        public PropertyInfo PropDest { get; set; }
        public Func<TSrc, object> function;

        public PropertyAssociation(PropertyInfo propDest, Func<TSrc, object> func)
        {
            PropDest = propDest;
            function = func;
        }
    }
}
