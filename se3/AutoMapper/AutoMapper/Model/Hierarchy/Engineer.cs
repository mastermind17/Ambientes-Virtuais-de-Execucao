using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model.Hierarchy
{
    public class Engineer : Teacher
    {
        public string title { get; set; }
        public Engineer(string name, int id) : base(name, id) { title = "Eng." + name; eng = this; }
    }
}
