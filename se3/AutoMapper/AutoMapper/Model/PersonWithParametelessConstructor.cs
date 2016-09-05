using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model
{
    public class PersonWithParametelessConstructor
    {
        public string Name { get; set; }
        public int Nr { get; set; }
        [ForeignerAttibute(OriginCountry = "Spain")]
        public bool Origin { get; set; }

        public PersonWithParametelessConstructor() { }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            PersonWithParametelessConstructor person = obj as PersonWithParametelessConstructor;
            if (person == null)
                return false;
            return this.Name.Equals(person.Name) && this.Nr == person.Nr && this.Origin == person.Origin;
        }
    }
}
