using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day9
    {
        public class XMAS {
            public List<long> Preamble { get; set; } = new List<long>();

            public long Value { get; set; }

            public bool IsValid() {
                foreach(var (x, y) in this.Preamble.SelectMany(x => this.Preamble, (x, y) => new Tuple<long, long>(x, y))) {
                    if (x + y == this.Value) return true;
                };

                return false;
            }
        }

        List<XMAS> Xmas = new List<XMAS>();

        List<long> Numbers = new List<long>();
        private void Init()
        {
            using(var sr = new StreamReader("input_day9.txt")) {
                this.Numbers.AddRange(sr.ReadToEnd()
                    .Split("\n")
                    .Where(text => text.Length > 0)
                    .Select(x => Int64.Parse(x)));

                this.Numbers.Skip(25).ToList().ForEach(x => {
                    this.Xmas.Add(new XMAS{
                        Preamble = this.Numbers.Skip(Xmas.Count).Take(25).Select(x => x).ToList(),
                        Value = x
                    });
                });
            }
        }

        private long Task1()
        {
            foreach(var xmas in this.Xmas) {
                if (!xmas.IsValid()) {
                    return xmas.Value;
                }
            }

            return 0;
        }

        private void Task2(long input)
        {
            // Initial solution
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for(var i = 0; i < this.Numbers.Count(); i++) {
                long sum = 0;
                var range = 2;
                while (sum < input) {
                    sum = this.Numbers.Skip(i).Take(range).Sum();
                    range += 1;
                }

                if (sum == input) {
                    var congRange = this.Numbers.GetRange(i, range);
                    var min = congRange.Min();
                    var max = congRange.Max();
                    Console.WriteLine($"{min + max}");
                }
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            // Solution based on feedback
            watch.Restart();
            long total = 0;
            var skip = 0;
            var take = 2;
            while(total != input) {
                if (total < input) {
                    take += 1;
                } else if (total > input) {
                    skip += 1;
                    take -= 1;
                }

                total = this.Numbers.Skip(skip).Take(take).Sum();
            }

            var numberRange = this.Numbers.GetRange(skip, take);
            var mi = numberRange.Min();
            var ma = numberRange.Max();
            Console.WriteLine($"{mi + ma}");
            
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        public void Run()
        {
            this.Init();
            Console.WriteLine("\nLooking for answer for Task 1.");
            Console.WriteLine(this.Task1());
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(this.Task1());
        }
    }
}