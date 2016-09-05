using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Reflect
{
    public interface ICriteria
    {
        string Execute(object src);
    }
}
