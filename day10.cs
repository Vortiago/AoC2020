using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day10
    {
        LinkedList<int> Numbers = new LinkedList<int>();
        private void Init()
        {
            this.Numbers.AddFirst(new LinkedListNode<int>(0));
            using(var sr = new StreamReader("input_day10.txt")) {
                var data = sr.ReadToEnd()
                    .Split("\n")
                    .Where(text => text.Length > 0)
                    .Select(x => Int32.Parse(x)).ToList().OrderBy(x => x);

                this.Numbers.AddFirst(0);

                foreach(var number in data) {
                    this.Numbers.AddAfter(this.Numbers.Last, number);
                }

                this.Numbers.AddAfter(this.Numbers.Last, this.Numbers.Last.Value + 3);
            }
        }

        private Tuple<int, int, int> Task1()
        {
            var one = 0;
            var two = 0;
            var three = 0;
            var number = this.Numbers.First;
            while(number.Next is not null) {
                switch(number.Next.Value - number.Value) {
                    case 1:
                        one += 1;
                        break;
                    case 2:
                        two += 1;
                        break;
                    case 3:
                        three += 1;
                        break;
                }
                number = number.Next;
            }

            return new Tuple<int, int, int>(one, two, three);
        }

        private void Task2(int one, int two, int three)
        {
            
        }

        private void Rec(LinkedList<int> numbers) 
        {
            
        }

        public void Run()
        {
            this.Init();
            Console.WriteLine("\nLooking for answer for Task 1.");
            var x = this.Task1();
            Console.WriteLine(x.Item1 * x.Item3);
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(x.Item1, x.Item2, x.Item3);
        }
    }
}