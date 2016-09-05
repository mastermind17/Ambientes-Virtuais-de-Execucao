using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model
{
    public class PersonForMember
    {
        public string Name {get;set;}
        public string Id {get;set;}

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            PersonForMember person = obj as PersonForMember;
            if (person == null)
                return false;
            return this.Name.Equals(person.Name) && this.Id == person.Id;
        }
    }
}
