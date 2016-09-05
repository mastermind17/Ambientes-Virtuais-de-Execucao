using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
	public class PropertyBuilder<TSrc, TDest>
	{
		private readonly List<Func<PropertyInfo, bool>> _validators;
		private readonly List<PropertyAssociation<TSrc>> _associations;

		public PropertyBuilder()
		{
			_associations = new List<PropertyAssociation<TSrc>>();
			_validators = new List<Func<PropertyInfo, bool>>();
		}

		public Mapper<TSrc, TDest> CreateMapper()
		{
			return new PropertyMapper<TSrc, TDest>(ValidateProperties());
		}

		public PropertyBuilder<TSrc, TDest> IgnoreMember(string name)
		{
			_validators.Add(pi => !pi.Name.Equals(name));
			CheckForMemberInsertions(_validators.Last());
			return this;
		}

		public PropertyBuilder<TSrc, TDest> IgnoreMember<MyAttribute>()
		{
			_validators.Add(pi => pi.GetCustomAttribute(typeof(MyAttribute)) == null);
			CheckForMemberInsertions(_validators.Last());
			return this;
		}

		public PropertyBuilder<TSrc, TDest> ForMember(string namePropDest, Func<TSrc, object> func)
		{
			//Para não adicionar às "cegas", pode não existir a prop destino pretendido e pode gerar excepção no Mapper
			IEnumerable<PropertyInfo> prop = typeof(TDest).GetProperties().Where(propInfo => propInfo.Name.Equals(namePropDest));
			if (prop.Count() > 0)
				_associations.Add(new PropertyAssociation<TSrc>(prop.First(), func));
			return this;
		}

		/// <summary>
		/// //Obter as propriedades com nome e tipo compativel e depois ver se foi colocada alguma restrição para aquela propriedade
		/// </summary>
		/// <returns></returns>
		public List<PropertyAssociation<TSrc>> ValidateProperties()
		{
			PropertyInfo[] propsSrc = typeof(TSrc).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			PropertyInfo[] propsDest = typeof(TDest).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			foreach (PropertyInfo propIn in propsSrc)
			{
				IEnumerable<PropertyInfo> prop = propsDest.Where(propertyDest => propIn.Name.Equals(propertyDest.Name) &&
																 propertyDest.PropertyType.IsAssignableFrom(propIn.PropertyType));
				if (prop.Count() > 0)
				{
					PropertyInfo curr = prop.First();
					if (ValidateProperty(curr))
						_associations.Add(new PropertyAssociation<TSrc>(curr, src => propIn.GetGetMethod().Invoke(src, new object[0])));
				}
			}
			return _associations;
		}

		private void CheckForMemberInsertions(Func<PropertyInfo, bool> check)
		{
			if (_associations.Count > 0)
			{//ForMember adicionou alguma coisa, verificar se este IgnoreMember é referente à prop introduzida pelo ForMember
				IEnumerable<PropertyAssociation<TSrc>> forMemberAdded = _associations.Where(propAssociation => !check(propAssociation.PropDest));
				_associations.Remove(forMemberAdded.First());
			}
		}

		private bool ValidateProperty(PropertyInfo pi)
		{
			return pi != null && _validators.All(validator => validator(pi));
		}

	}
}
