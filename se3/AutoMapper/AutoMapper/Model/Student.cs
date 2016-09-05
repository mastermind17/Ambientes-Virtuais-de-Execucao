using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model
{
    public class Student
    {
        public string Name { get; set; }
        public int Nr { get; set; }

        [ForeignerAttibute(OriginCountry = "Spain")]
        public bool Origin { get; set; }
    }
    public sealed class ForeignerAttibute : System.Attribute
    {
        public string OriginCountry { get; set; }
    }
}
