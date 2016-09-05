    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jsonzai.Reflect;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Jsonzai.Instr
{
    public class EmmiterUtils
    {
        private static Dictionary<Type, IEmmiterCriteria> emmiterCriteria =
            new Dictionary<Type, IEmmiterCriteria>();
        
        private static string suffix = "Serializer";

        public static ICriteria CodeEmmiter(object obj, string objName, string critName, ICriteria criteria)
        {
            /*TESTE*/
            if (emmiterCriteria.Count == 0)
            {
                emmiterCriteria.Add(typeof(FilterByProperty), new EmmitProcessProperty());
                emmiterCriteria.Add(typeof(FilterByField), new EmitterProcessFields());
                emmiterCriteria.Add(typeof(FilterParametlessNonVoidMethod), new EmitterProcessParametlessNonVoidMethod());
            }
            /*END TESTE*/
            
            string asmName = objName + critName;
            string className = objName + critName + suffix;
            AssemblyBuilder asmBuilder;

            TypeBuilder typeBuilder = CreateType(asmName, className, out asmBuilder);
            typeBuilder.AddInterfaceImplementation(typeof(ICriteria));
            MethodBuilder mb = typeBuilder.DefineMethod(
                                                        "Execute",
                                                         MethodAttributes.Virtual | MethodAttributes.Public | MethodAttributes.ReuseSlot,
                                                         typeof(string),
                                                         new Type[1] { typeof(object) }
                                                        );
            ConstructorBuilder ctorBuilder = typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            //FieldBuilder fieldBuilder = typeBuilder.DefineField("CriteriaTarget", typeof(Criteria), FieldAttributes.Public);
            //AddCtor(typeBuilder, fieldBuilder);
            
            IEmmiterCriteria ilEmmiter;
            if (!emmiterCriteria.TryGetValue(criteria.GetType(), out ilEmmiter))
            {
                Console.WriteLine("Criteria not accepted");
                return null;
            }
            else
                ilEmmiter.EmmiterCode(typeBuilder, mb, obj);

            Type myClass = typeBuilder.CreateType();

            asmBuilder.Save(asmName + ".dll");

            return (ICriteria)Activator.CreateInstance(myClass);
        }

        private static TypeBuilder CreateType(string asmName, string className,out AssemblyBuilder asmBuilder)
        {
            AssemblyName name = new AssemblyName(asmName);
            asmBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule(asmName, asmName + ".dll");

            TypeBuilder klassBuilder = moduleBuilder.DefineType(
                className,
                TypeAttributes.Public);

            return klassBuilder;
        }

        private static void AddCtor(TypeBuilder typeBuilder, FieldBuilder fieldBuilder) 
        {
            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[1] { typeof(ICriteria) });
            ILGenerator ctorIl = ctorBuilder.GetILGenerator();
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Stfld, fieldBuilder);
            ctorIl.Emit(OpCodes.Ret);
        }
    }
}
