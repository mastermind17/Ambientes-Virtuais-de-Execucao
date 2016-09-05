using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper.Model.Hierarchy
{
    public class Teacher : PersonH
    {
        public int Id { get; set; }
        public Teacher eng { get; set; }
        public Teacher(string name, int id) : base(name) { Id = id; eng = this; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Teacher teacher = obj as Teacher;
            if (teacher == null)
                return false;
            return this.Id == teacher.Id && this.eng == teacher.eng;
        }
    }
}
