using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using AutoMapper.Model;
using System.Collections.Generic;
using AutoMapper.Model.Hierarchy;

namespace MapperTests
{
    [TestClass]
    public class TestFunctions
    {
        [TestMethod]
        public void TestMapOperation()
        {
            Mapper<Student, Person> m = AutoMapper.AutoMapper.Build<Student, Person>().CreateMapper();
            Student s = new Student { Nr = 39393, Name = "Edgar Demétrio" , Origin = true};
            Person expected = new Person { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            Person actual = m.Map(s);
            Assert.AreEqual(expected, actual);
            
            Student[] stds = { new Student { Nr = 27721, Name = "Ze Manel" }, new Student { Nr = 15642, Name = "Maria Papoila" } };
            
            Person[] expectedList = { new Person { Nr = 27721, Name = "Ze Manel" }, new Person { Nr = 15642, Name = "Maria Papoila" } };
            List<Person> actualList = m.Map<List<Person>>(stds);
            CollectionAssert.AreEqual(expectedList, actualList);

            Person[] expectedArray = { new Person { Nr = 27721, Name = "Ze Manel" }, new Person { Nr = 15642, Name = "Maria Papoila" } };
            Person[] actualArray = m.MapToArray(stds);
            CollectionAssert.AreEqual(expectedArray, actualArray);

            IEnumerable<Person> actualLazy = m.MapLazy(stds);
            int index = 0;
            foreach (Person actualPersonLazy in actualLazy)
                Assert.AreEqual(expectedArray[index++], actualPersonLazy);
        }
        
        [TestMethod]
        public void TestIgnoreNameOperation()
        {
            Mapper<Student, Person> m = AutoMapper.AutoMapper.Build<Student, Person>().IgnoreMember("Name").CreateMapper();
            Student s = new Student { Nr = 39393, Name = "Edgar Demétrio", Origin = true };

            Person actual = m.Map(s);
            Assert.IsNull(actual.Name);

            Mapper<Student, Person> mNr = AutoMapper.AutoMapper.Build<Student, Person>().IgnoreMember("Nr").CreateMapper();
            Person actualNr = mNr.Map(s);
            Assert.AreEqual(actualNr.Nr, 0);

            /*Não foi testado o resto dos mapeamentos uma vez que Map<TColDest>, MapToArray e MapLazy chamam todos Map
             *então se o map para o caso especifico funciona e foi testado o resto dos mapeamentos com as definições por omissão
             *estas adições iram funcionar também
             */
        }
        
        [TestMethod]
        public void TestIgnoreCusttomAttributeOperation()
        {
            Mapper<Student, Person> m = AutoMapper.AutoMapper.Build<Student, Person>().IgnoreMember<ForeignerAttibute>().CreateMapper();
            Student s = new Student { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            Person actual = m.Map(s);

            Assert.IsFalse(actual.Origin);
        }

        [TestMethod]
        public void TestMapObjWithConstructor()
        {
            Mapper<Student, PersonWithConstructor> m = AutoMapper.AutoMapper.Build<Student, PersonWithConstructor>().CreateMapper();
            Student s = new Student { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            PersonWithConstructor expected = new PersonWithConstructor(null) { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            PersonWithConstructor actual = m.Map(s);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMapObjToStruct()
        {
            Mapper<Student, PersonStruct> m = AutoMapper.AutoMapper.Build<Student, PersonStruct>().CreateMapper();
            Student s = new Student { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            PersonStruct expected = new PersonStruct { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            PersonStruct actual = m.Map(s);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMapWithObjWithParametelessConstructor() { 
            Mapper<Student, PersonWithParametelessConstructor> m = AutoMapper.AutoMapper.Build<Student, PersonWithParametelessConstructor>().CreateMapper();
            Student s = new Student { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            PersonWithParametelessConstructor expected = new PersonWithParametelessConstructor() { Nr = 39393, Name = "Edgar Demétrio", Origin = true };
            PersonWithParametelessConstructor actual = m.Map(s);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMapForMember() {
            Mapper<StudentForMember, PersonForMember> m = AutoMapper.AutoMapper.Build<StudentForMember, PersonForMember>().ForMember("Id", src=>src.Nr.ToString()).CreateMapper();
            StudentForMember s = new StudentForMember { Nr = 39393, Name = "Edgar Demétrio"};
            PersonForMember expected = new PersonForMember { Name = "Edgar Demétrio", Id = "39393"};
            PersonForMember actual = m.Map(s);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMapIgnoreAfterForMember()
        {
            Mapper<StudentForMember, PersonForMember> m = AutoMapper.AutoMapper.Build<StudentForMember, PersonForMember>().ForMember("Id", src => src.Nr.ToString()).IgnoreMember("Id").CreateMapper();
            StudentForMember s = new StudentForMember { Nr = 39393, Name = "Edgar Demétrio" };
            PersonForMember actual = m.Map(s);
            Assert.IsNull(actual.Id);
        }

        [TestMethod]
        public void TestMapIgnoreBeforeForMember()
        {
            Mapper<StudentForMember, PersonForMember> m = AutoMapper.AutoMapper.Build<StudentForMember, PersonForMember>().IgnoreMember("Id").ForMember("Id", src => src.Nr.ToString()).CreateMapper();
            StudentForMember s = new StudentForMember { Nr = 39393, Name = "Edgar Demétrio" };
            PersonForMember expected = new PersonForMember { Name = "Edgar Demétrio", Id = "39393" };
            PersonForMember actual = m.Map(s);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMapCompatibleType()
        {
	        Holder holder = new Holder { person = new Engineer("Edgar", 39393) };
	        Mapper<Holder, Teacher> mapper = AutoMapper.AutoMapper.Build<Holder, Teacher>().ForMember("eng", hol => hol.person).CreateMapper();
            Teacher actual = mapper.Map(holder);
            Teacher expected = new Teacher("Edgar", 39393);
            Assert.AreEqual(expected.Name, actual.eng.Name);
            Assert.AreEqual(expected.Id, actual.eng.Id);

        }

        [TestMethod]
        public void TestMapCompatibleTypeNotCompatible()
        {
            Mapper<Holder, Teacher> mapper = AutoMapper.AutoMapper.Build<Holder, Teacher>().ForMember("eng", hol => hol.person).CreateMapper();
	        Holder holder = new Holder { person = new Employee("Test", 11111) };
	        Teacher actual = mapper.Map(holder);
            Assert.IsNull(actual.eng.Name);
        }
    }
}
