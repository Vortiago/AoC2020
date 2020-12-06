using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day2 {
        
        public class RowContent {
            public int Min { get; set; }
            public int Max { get; set; }
            public string Char { get; set; }
            public string Password { get; set; }
        }

        private List<RowContent> taskContent;

        private void Task1() {
            using(var sr = new StreamReader("input_day2.txt")) {
                taskContent = sr.ReadToEnd()
                    .Split('\n')
                    .Where(text => text.Length > 0)
                    .Select(x => new RowContent {
                        Min = Int32.Parse(x.Split('-')[0]),
                        Max = Int32.Parse(x.Split('-', ' ')[1]),
                        Char = x.Split(' ', ':')[1],
                        Password = x.Split(':')[1].Trim()

                }).ToList();
            }
            
            var result = taskContent
                .Where(x => x.Password
                    .Where(chr => Convert.ToString(chr) == x.Char)
                    .Count() <= x.Max && x.Password.Where(chr => Convert.ToString(chr) == x.Char)
                    .Count() >= x.Min);

            Console.WriteLine(result.Count());
        }

        private void Task2() {
            var result = taskContent
                .Select(x => new List<char>{x.Password.ElementAtOrDefault(x.Min - 1), x.Password.ElementAtOrDefault(x.Max - 1)}
                    .Count(y => y == Convert.ToChar(x.Char)))
                .Where(x => x == 1);

            Console.WriteLine(result.Count());
        }

        public void Run() {
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }   
}