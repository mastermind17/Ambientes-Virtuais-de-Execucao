using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Jsonzai.Instr
{
    public class EmitterProcessFields : IEmmiterCriteria
    {
        public void EmmiterCode(TypeBuilder tb, MethodBuilder mb, object obj)
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
            var fis = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fi in fis)
            {
                var fb = tb.DefineField(fi.Name, fi.FieldType, fi.Attributes);
                il.Emit(OpCodes.Ldstr, "\"" + fi.Name + "\"");
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Castclass, obj.GetType());
                il.Emit(OpCodes.Ldfld, fb);
                if (fi.FieldType.IsValueType)
                    il.Emit(OpCodes.Box, fi.FieldType);
                il.Emit(OpCodes.Call, processType);
                il.Emit(OpCodes.Call, concatMethod);
            }
            il.Emit(OpCodes.Call, concatMethod);
            il.Emit(OpCodes.Ret);
        }
    }
}
