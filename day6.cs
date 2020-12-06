using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day6 {

        public class Person {
            public List<char> Answers { get; set; }
        }

        public class GroupOfPersons {
            public List<Person> Persons { get; set; } 
            public int NumberOfAnswers => this.Persons.SelectMany(person => person.Answers).Distinct().Count();
            public int GetAnsweredByAll() {
                var charArray = Enumerable.Range('a', 26).Select(x => (char) x);
                this.Persons.ForEach(x => charArray = charArray.Intersect(x.Answers));
                return charArray.Count();
            }
        }

        public List<GroupOfPersons> Group  { get; set; }

        private void Init(string filename) {
            using(var sr = new StreamReader(filename)) {
                this.Group = sr.ReadToEnd()
                    .Split("\n\n")
                    .Where(text => text.Length > 0)
                    .Select(group => new GroupOfPersons{
                        Persons = group.Split('\n').Where(x => x.Length > 0).Select(person => new Person {Answers = person.ToCharArray().ToList()}).ToList()
                    }).ToList();
            }
        }

        private void Task1() {
            Console.WriteLine(this.Group.Sum(x => x.NumberOfAnswers));
        }

        private void Task2() {
            Console.WriteLine(this.Group.Sum(x => x.GetAnsweredByAll()));
        }

        public void Run(string filename) {
            this.Init(filename);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }   
}