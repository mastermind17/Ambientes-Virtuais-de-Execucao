using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Jsonzai.Reflect;

namespace Jsonzai.Instr
{
    public interface IEmmiterCriteria
    {
        void EmmiterCode(TypeBuilder typeBuilder, MethodBuilder mb, object obj);
    }
}
