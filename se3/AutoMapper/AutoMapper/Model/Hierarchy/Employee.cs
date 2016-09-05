using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model.Hierarchy
{
    public class Employee : PersonH
    {
        public int Id { get; set; }
        public Employee eng { get; set; }
        public Employee(string name, int id) : base(name) { Id = id; eng = this; }
    }
}
