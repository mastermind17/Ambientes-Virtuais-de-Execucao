using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMapper
{
	public class PropertyMapper<TSrc, TDest> : Mapper<TSrc, TDest>
	{
		private readonly List<PropertyAssociation<TSrc>> _validProps;

		public PropertyMapper(List<PropertyAssociation<TSrc>> associations)
		{
			_validProps = new List<PropertyAssociation<TSrc>>();
			_validProps.AddRange(associations);
		}

		public TDest Map(TSrc src)
		{
			object destHandle = ObjectInstance(typeof(TDest));
			foreach (PropertyAssociation<TSrc> propAssociation in _validProps)
			{
				object propRet = propAssociation.function(src);
				if (propAssociation.PropDest.PropertyType.IsAssignableFrom(propRet.GetType()))
					propAssociation.PropDest.SetValue(destHandle, propRet);
			}

			return (TDest)destHandle;
		}

		public TColDest Map<TColDest>(IEnumerable<TSrc> src) where TColDest : ICollection<TDest>
		{
			ICollection<TDest> col = (ICollection<TDest>)Activator.CreateInstance(typeof(TColDest));
			foreach (TSrc item in src)
				col.Add(Map(item));
			return (TColDest)col;
		}

		public TDest[] MapToArray(IEnumerable<TSrc> src)
		{
			TDest[] propDest = new TDest[src.ToArray().Length];
			for (int i = 0; i < src.Count(); i++)
				propDest[i] = Map(src.ElementAt(i));
			return propDest;
		}

		public IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src)
		{
			foreach (TSrc item in src)
				yield return Map(item);
		}

		public object ObjectInstance(Type dest)
		{
			if (dest.IsValueType)
				return Activator.CreateInstance(dest);
			ConstructorInfo[] c = dest.GetConstructors();
			ParameterInfo[] paramInfo = c[0].GetParameters();
			object[] defaultParam = new object[paramInfo.Length];
			for (int i = 0; i < paramInfo.Length; i++)
			{
				Type paramType = paramInfo[i].ParameterType;
				if (paramType == typeof(string))
					defaultParam[i] = default(string);
				else
					defaultParam[i] = ObjectInstance(paramType);
			}
			return c[0].Invoke(defaultParam);
		}
	}
}