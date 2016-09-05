using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jsonzai.Reflect
{
    public class FilterParametlessNonVoidMethod : ICriteria
    {
        public string Execute(object src)
        {
            string processObj = "{";
            MethodInfo[] methodInfo = src.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (MethodInfo t in methodInfo)
            {
                if (t.ReturnType == typeof(void) || t.GetParameters().Length != 0) continue;
                processObj += "\"" + t.Name + "\":";
                object methodReturn = t.Invoke(src, new object[0]);
                processObj += Jsonfier.ToJson(methodReturn, this);
                processObj += ",";
            }
            processObj = processObj.Substring(0, processObj.Length -1);
            processObj += "}";
            return processObj;
        }
    }
}
