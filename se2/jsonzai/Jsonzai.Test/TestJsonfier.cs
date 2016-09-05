using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Jsonzai.Test.Model;
using System.Globalization;
using Jsonzai.Reflect;
using Jsonzai.Instr;
using Newtonsoft.Json.Linq;
using System.Diagnostics;


namespace Jsonzai.Test
{
    public class LinkedList<T>
    {
        private readonly Node<T> head = new Node<T>(default(T), null);

        public LinkedList()
        {
            head.next = head;
        }
        public void Add(T val)
        {
            Node<T> n = new Node<T>(val, head.next);
            this.head.next = n;
        }
        public bool Contains(T elem)
        {
            for (Node<T> h = head.next; h != head; h = h.next)
            {
                if (h.val.Equals(elem))
                    return true;
            }
            return false;
        }
        class Node<R>
        {
            public R val;
            public Node<R> next;
            public Node(R val, Node<R> next)
            {
                this.val = val;
                this.next = next;
            }
        }
    }

    [TestClass]
    public class TestJsonfier
    {
        [TestMethod]
        public void TestEmmitVsReflectionWithParametlessNonVoidReturnForStudent()
        {
            double durationEmmit = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsoninstr.SerializeType(new Student(3, "Ramiro Jorge"), new FilterParametlessNonVoidMethod());
                Jsoninstr.SerializeType(new Student(5, "Ramiro Jorge test 2"), new FilterParametlessNonVoidMethod());
                double test = time.Elapsed.TotalMilliseconds;
                durationEmmit = test < durationEmmit ? test : durationEmmit;
            }

            double durationReflection = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsonfier.ToJson(new Student(27721, "Ze Manel"), new FilterParametlessNonVoidMethod());
                Jsonfier.ToJson(new Student(15642, "Maria Papoila"), new FilterParametlessNonVoidMethod());
                double test = time.Elapsed.TotalMilliseconds;
                durationReflection = test < durationReflection ? test : durationReflection;
            }
            if (durationEmmit < durationReflection)
                Assert.AreNotEqual(durationEmmit, durationReflection);
            else
                Assert.AreEqual(durationEmmit, durationReflection);
        }


        [TestMethod]
        public void TestEmmitVsReflectionWithFieldsForStudent()
        {
            double durationEmmit = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsoninstr.SerializeType(new Student(3, "Ramiro Jorge"), new FilterByField());
                Jsoninstr.SerializeType(new Student(5, "Ramiro Jorge test 2"), new FilterByField());
                double test = time.Elapsed.TotalMilliseconds;
                durationEmmit = test < durationEmmit ? test : durationEmmit;
            }

            double durationReflection = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsonfier.ToJson(new Student(27721, "Ze Manel"), new FilterByField());
                Jsonfier.ToJson(new Student(15642, "Maria Papoila"), new FilterByField());
                double test = time.Elapsed.TotalMilliseconds;
                durationReflection = test < durationReflection ? test : durationReflection;
            }
            if (durationEmmit < durationReflection)
                Assert.AreNotEqual(durationEmmit, durationReflection);
            else
                Assert.AreEqual(durationEmmit, durationReflection);
        }

        [TestMethod]
        public void TestEmmitVsReflectionWithPropertyForStudent()
        {
            double durationEmmit = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsoninstr.SerializeType(new Student(3, "Ramiro Jorge"), new FilterByProperty());
                Jsoninstr.SerializeType(new Student(5, "Ramiro Jorge test 2"), new FilterByProperty());
                double test = time.Elapsed.TotalMilliseconds;
                durationEmmit = test < durationEmmit ? test : durationEmmit;
            }

            double durationReflection = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsonfier.ToJson(new Student(27721, "Ze Manel"), new FilterByProperty());
                Jsonfier.ToJson(new Student(15642, "Maria Papoila"), new FilterByProperty());
                double test = time.Elapsed.TotalMilliseconds;
                durationReflection = test < durationReflection ? test : durationReflection;
            }
            if (durationEmmit < durationReflection)
                Assert.AreNotEqual(durationEmmit, durationReflection);
            else
                Assert.AreEqual(durationEmmit, durationReflection);
        }

        [TestMethod]
        public void TestEmmitVsReflectionWithPropertyForCourse()
        {
            Course expected = new Course
                    (
                        "AVE",
                        new Student[4]{
                            new Student(27721, "Ze Manel"),
                            new Student(15642, "Maria Papoila"),
                            null,
                            null
                        }
                    );
            double durationEmmit = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsoninstr.SerializeType(expected, new FilterByProperty());
                double test = time.Elapsed.TotalMilliseconds;
                durationEmmit = test < durationEmmit ? test : durationEmmit;
            }

            double durationReflection = double.MaxValue;
            for (int i = 0; i < 15; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                Jsonfier.ToJson(expected, new FilterByProperty());
                double test = time.Elapsed.TotalMilliseconds;
                durationReflection = test < durationReflection ? test : durationReflection;
            }
            if (durationEmmit < durationReflection)
                Assert.AreNotEqual(durationEmmit, durationReflection);
            else
                Assert.AreEqual(durationEmmit, durationReflection);

        }

        [TestMethod]
        public void TestJsonfierStudent()
        {
            //Jsonfier.setCriteria(new FilterByProperty());
            Student expected = new Student(27721, "Ze Manel");
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, new FilterByProperty());
            //string json1 = "{\"nr\":27721,\"name\":\"Ze Manel\"}";
            Student actual = JsonConvert.DeserializeObject<Student>(json);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJsonfierStudentForMethods()
        {
            //Jsonfier.setCriteria(new FilterParametlessNonVoidMethod());
            Student expected = new Student(27721, "Ze Manel");
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, new FilterParametlessNonVoidMethod());
            Assert.AreEqual(json, "{\"get_Nr\":27721,\"get_Name\":\"Ze Manel\"}");
        }

        [TestMethod]
        public void TestJsonfierCourseForMethods()
        {
            //Jsonfier.setCriteria(new FilterParametlessNonVoidMethod());
            Course expected = new Course
                                (
                                    "AVE",
                                    new Student[4]{
                                        new Student(27721, "Ze Manel"),
                                        new Student(15642, "Maria Papoila"),
                                        null,
                                        null
                                    }
                                );
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, new FilterParametlessNonVoidMethod());
            Assert.AreEqual(json, "{\"get_Students\":[{\"get_Nr\":27721,\"get_Name\":\"Ze Manel\"},{\"get_Nr\":15642,\"get_Name\":\"Maria Papoila\"},null,null],\"get_Name\":\"AVE\"}");
        }

        [TestMethod]
        public void TestJsonfierArrayPrimitives()
        {
            //Jsonfier.setCriteria(new FilterByField());
            int[] expected = { 4, 5, 6, 7 };
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json = Jsonfier.ToJson(expected, new FilterByField());
            //string json1 = "[4,5,6,7]";
            int[] actual = JsonConvert.DeserializeObject<int[]>(json);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJsonfierCourse()
        {
            //Jsonfier.setCriteria(new FilterByField());
            Course expected = new Course
            (
                "AVE",
                new Student[4]{
                    new Student(27721, "Ze Manel"),
                    new Student(15642, "Maria Papoila"),
                    null,
                    null
                }
            );
            /*
             * O resultado de ToJson(expected) deve ser igual à string json abaixo
             */
            string json1 = Jsonfier.ToJson(expected, new FilterByField());
            /*string json = "{" +
                "\"name\":\"AVE\"," +
                "\"stds\":" +
                    "[" +
                        "{\"nr\":27721,\"name\":\"Ze Manel\"}," +
                        "{\"nr\":15642,\"name\":\"Maria Papoila\"}," +
                        "null," +
                        "null" +
                    "]" +
                "}";*/
            Course actual = JsonConvert.DeserializeObject<Course>(json1);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestLinkedList() //Rever este teste
        {
            //Jsonfier.setCriteria(new FilterByField());
            LinkedList<String> l = new LinkedList<String>();
            l.Add("Test");
            l.Add("Para");
            l.Add("Ver");
            string json = Jsonfier.ToJson(l, new FilterByField());
            //LinkedList<String> returned = JsonConvert.DeserializeObject<LinkedList<String>>("{\"head\":{\"val\":,\"next\":{\"val\":\"Ver\",\"next\":{\"val\":\"Para\",\"next\":{\"val\":\"Test\",\"next\":\"null\"}}}}}");
            Assert.AreEqual("{\"head\":{\"val\":\"null\",\"next\":{\"val\":\"Ver\",\"next\":{\"val\":\"Para\",\"next\":{\"val\":\"Test\",\"next\":\"null\"}}}}}", json);

        }
    }
}
