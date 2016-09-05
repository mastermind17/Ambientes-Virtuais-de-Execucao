using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Instr
{
    public class EmitterProcessParametlessNonVoidMethod : IEmmiterCriteria
    {
        public void EmmiterCode(TypeBuilder typeBuilder, MethodBuilder mb, object obj)
        {
            var il = mb.GetILGenerator();
            var concatMethod = typeof(string).GetMethod("Concat", BindingFlags.Public | BindingFlags.Static,
                                                               Type.DefaultBinder,
                                                               new Type[] { typeof(string), typeof(string) },
                                                               null);
            var processType = typeof(Jsonzai.Reflect.Jsonfier).GetMethod("ToJsonDictonary", BindingFlags.Public | BindingFlags.Static,
                                                       Type.DefaultBinder,
                                                       new Type[] { typeof(object) },
                                                       null);
            var mis = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var mi in mis)
            {
                if (mi.ReturnType == typeof(void) || mi.GetParameters().Length != 0) continue;
                il.Emit(OpCodes.Ldstr, "\"" + mi.Name + "\"");
                if (!mi.IsStatic)
                    il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Call, mi);
                if (mi.ReturnType.IsValueType)
                    il.Emit(OpCodes.Box, mi.ReturnType);
                il.Emit(OpCodes.Call, processType);
                il.Emit(OpCodes.Call, concatMethod);
            }
            il.Emit(OpCodes.Call, concatMethod);
            il.Emit(OpCodes.Ret);
        }
    }
}
