using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model
{
    public struct PersonStruct
    {
        public string Name { get; set; }
        public int Nr { get; set; }
        [ForeignerAttibute(OriginCountry = "Spain")]
        public bool Origin { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            PersonStruct person = (PersonStruct)obj;
            return this.Name.Equals(person.Name) && this.Nr == person.Nr && this.Origin == person.Origin;
        }
    }
}
