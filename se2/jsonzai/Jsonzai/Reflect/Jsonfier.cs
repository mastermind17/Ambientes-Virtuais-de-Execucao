using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using System.Collections;
using Jsonzai.Instr;

namespace Jsonzai.Reflect
{
    public class Jsonfier
    {

        public static string jsonObj = null;

        public static ICriteria criteria;

        static LinkedList<int> list = new LinkedList<int>();

        public static string ToJson(object src) {
            return ToJson(src, criteria);
        }

        public static string ToJson(object src, ICriteria crit) 
        {
            System.Diagnostics.Debug.WriteLine("Criteria is: " + crit);
            if (src == null)
                return "\"null\"";//return null;//Console.WriteLine("Object passed is null.");
            
            Type t = src.GetType();
            
            if (t.IsPrimitive || t == typeof(string)) {
                return ProcessPrimitiveOrString(src);
            }

            if (t.IsArray)
                return ProcessArray(src, crit);

            return ProcessObject(src, crit);
        }

        public static string ToJsonDictonary(object src)
        {
            if (src == null)
                return "\"null\"";//return null;//Console.WriteLine("Object passed is null.");

            Type t = src.GetType();

            if (t.IsPrimitive || t == typeof(string))
            {
                return ProcessPrimitiveOrString(src);
            }

            return t.IsArray ? Jsoninstr.GetElements(src, criteria) : Jsoninstr.SerializeType(src, criteria);
        }

        private static string ProcessObject(object src, ICriteria crit)
        {
            int objHash = src.GetHashCode();
            
            if (!list.Contains(objHash))
                list.AddFirst(objHash);
            else
                return "\"null\"";
            return crit.Execute(src);
        }

        private static string ProcessPrimitiveOrString(object src)
        {
            string addValue="";
            if (src is string)
                addValue += "\"" + src + "\"";
            else
                addValue += src;

            return addValue;
        }

        private static string ProcessArray(object type, ICriteria crit)
        {
            IEnumerable iEnum = type as IEnumerable;
            IEnumerator rator = iEnum.GetEnumerator();
            rator.MoveNext();
            string toRet = "[";
            while (true)
            {
                if (rator.Current == null)
                    toRet += "null";
                else
                {
                    toRet += ToJson(rator.Current, crit);
                }
                if (rator.MoveNext())
                    toRet += ",";
                else
                    break;
            }
            toRet += "]";
            return toRet;
        }
    }

}
