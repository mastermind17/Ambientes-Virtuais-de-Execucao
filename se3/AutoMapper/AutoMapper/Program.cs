using AutoMapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
	public class Program
	{
		public static void Main(string[] args)
		{
            Mapper<Student, Person>m = AutoMapper.Build<Student, Person>().CreateMapper();
            Student s = new Student { Nr = 39393, Name = "Edgar Demétrio" };
            Person p = m.Map(s);
		}
	}
}
