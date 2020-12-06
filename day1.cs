using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day1 {
        
        private List<int> taskContent;

        private void Task1() {
            using(var sr = new StreamReader("input_day1.txt")) {
                taskContent = sr.ReadToEnd().Split('\n').Where(text => int.TryParse(text, out _)).Select(x => Int32.Parse(x)).ToList();
            }
            
            var y = taskContent.Where(a => taskContent.Exists(b => a + b == 2020)).Aggregate((a, b) => a * b);
            Console.WriteLine(y);

            var z = taskContent.SelectMany(a => taskContent, (a, b) => new List<int>{a, b}).First(x => x.Sum() == 2020).Aggregate((a, b) => a * b);
            Console.WriteLine(z);

            foreach (var numberA in taskContent) {
                foreach (var numberB in taskContent) {
                    if (numberA + numberB == 2020) {
                        Console.WriteLine("Found the answer!\n" + numberA * numberB);
                        return;
                    }
                }
            }
        }

        private void Task2() {
            
            var z = taskContent.SelectMany(a => taskContent, (a, b) => new List<int>{a, b}).First(x => x.Sum() == 2020).Aggregate((a, b) => a * b);
            Console.WriteLine(z);

            foreach (var numberA in taskContent) {
                foreach (var numberB in taskContent) {
                    foreach (var numberC in taskContent) {
                        if (numberA + numberB + numberC == 2020) {
                            Console.WriteLine("Found the answer!\n" + numberA * numberB * numberC);
                            return;
                        }
                   }
                }
            }
        }

        public void Run() {
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }   
}