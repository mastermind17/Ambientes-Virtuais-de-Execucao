using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jsonzai.Reflect;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Collections;
namespace Jsonzai.Instr
{

    public class Jsoninstr
    {

        /*String to build the name of the Type + Criteria + Serializer*/
        public static Dictionary<string, ICriteria> serializedObject =
            new Dictionary<string, ICriteria>();

        private static readonly string suffix = "Serializer";

        public static string SerializeType(object obj, ICriteria filterCriteria)
        {
            if (obj.GetType().IsArray)
                return GetElements(obj, filterCriteria);
            else
            {
                string objName = obj.GetType().Name;

                string critName = filterCriteria.GetType().Name;
                string searchDictionary = objName + critName + suffix;
                ICriteria contained;
                if (serializedObject.TryGetValue(searchDictionary, out contained))
                    return contained.Execute(obj);
                Jsonfier.criteria = filterCriteria;
                ICriteria newObjSerialized = EmmiterUtils.CodeEmmiter(obj, objName, critName, filterCriteria);//We pass the strings to avoid using reflection again

                serializedObject.Add(searchDictionary, newObjSerialized);
                                
                return newObjSerialized.Execute(obj);
            }
        }

        public static string GetElements(object obj, ICriteria filterCriteria)
        {
            IEnumerable iEnum = obj as IEnumerable;
            IEnumerator rator = iEnum.GetEnumerator();
            rator.MoveNext();
            string toRet = "[";
            while (true)
            {
                if (rator.Current != null)
                    toRet += SerializeType(rator.Current, filterCriteria);
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
