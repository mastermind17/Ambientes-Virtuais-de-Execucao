using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jsonzai.Reflect
{
    public class FilterByProperty: ICriteria
    {
        public string Execute(object src) {
            string processObj = "{";
            PropertyInfo[] propInfo = src.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            for (int i = 0; i < propInfo.Length; i++)
            {
                MethodInfo m = propInfo[i].GetGetMethod();
                object val = propInfo[i].GetGetMethod().Invoke(src, new object[0]);
                processObj += "\"" + propInfo[i].Name + "\":";
                processObj += Jsonfier.ToJson(val, this);
                if (i + 1 != propInfo.Length)
                    processObj += ",";

            }
            processObj += "}";
            return processObj;
        }
    }
}
