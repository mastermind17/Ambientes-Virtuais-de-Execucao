using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jsonzai.Reflect
{
    public class FilterByField : ICriteria
    {
        public string Execute(object src) {
            string processObj = "{";
            FieldInfo[] fieldInfo = src.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                processObj += "\"" + fieldInfo[i].Name + "\":";
                processObj += Jsonfier.ToJson(fieldInfo[i].GetValue(src), this);
                if (i + 1 != fieldInfo.Length)
                    processObj += ",";
            }
            processObj += "}";
            return processObj;
        }
    }
}
