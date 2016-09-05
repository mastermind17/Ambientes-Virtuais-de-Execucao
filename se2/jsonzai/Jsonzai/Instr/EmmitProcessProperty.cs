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
    public class EmmitProcessProperty : IEmmiterCriteria
    {
        public void EmmiterCode(TypeBuilder typeBuilder, MethodBuilder mb, object obj)
        {
            ILGenerator il = mb.GetILGenerator();
            MethodInfo concatMethod = typeof(string).GetMethod("Concat", BindingFlags.Public | BindingFlags.Static,
                                                               Type.DefaultBinder,
                                                               new Type[] { typeof(string), typeof(string) },
                                                               null);
            MethodInfo processType = typeof(Jsonfier).GetMethod("ToJsonDictonary", BindingFlags.Public | BindingFlags.Static,
                                                       Type.DefaultBinder,
                                                       new Type[] { typeof(object)},
                                                       null);

            foreach (PropertyInfo p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                il.Emit(OpCodes.Ldstr, "\"" + p.Name + "\"");
                PopulateMyMethod(il, typeBuilder, p.GetGetMethod(), obj);
                if (p.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, p.PropertyType);
                il.Emit(OpCodes.Call, processType);
                il.Emit(OpCodes.Call, concatMethod);

            }
            int numberOfProperty = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Length - 1;
            /*for (int i = 0; i < numberOfProperty; i++)*/
                il.Emit(OpCodes.Call, concatMethod);
            il.Emit(OpCodes.Ret);
        }

        private static void PopulateMyMethod(ILGenerator il, TypeBuilder typeBuilder, MethodInfo methodInfo, object obj)
        {
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Castclass, obj.GetType());
                il.Emit(OpCodes.Callvirt, methodInfo);
            }
        }
    }
}
