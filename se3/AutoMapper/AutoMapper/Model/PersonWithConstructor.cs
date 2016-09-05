using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model
{
    public class PersonWithConstructor
    {
        public string Name { get; set; }
        public int Nr { get; set; }
        [ForeignerAttibute(OriginCountry = "Spain")]
        public bool Origin { get; set; }
        string dateOfBirth;
        public PersonWithConstructor(string dateOfBirth) { this.dateOfBirth = dateOfBirth;}
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            PersonWithConstructor person = obj as PersonWithConstructor;
            if (person == null)
                return false;
            return this.Name.Equals(person.Name) && this.Nr == person.Nr && this.Origin == person.Origin;
        }
    }
}
