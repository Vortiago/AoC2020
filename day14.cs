using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Collections;

namespace AdventOfCode
{
    public class Day14
    {
        public class Mask {
            public long Oner { get; set; }

            public long Nuller { get; set; }  
        }

        public Dictionary<long, long> Memory { get; set; } = new Dictionary<long, long>();

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day14.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private List<string> ParseTodaysInput(string s) {
            return s.Replace("\r", String.Empty).Split("\n").Where(x => x.Length > 0).ToList();
        }

        private Mask GenerateMask(string input) {
            var mask = new Mask();
            foreach (var bit in input) {
                mask.Oner <<= 1;
                mask.Nuller <<= 1;
                if (bit == 'X') {
                    mask.Nuller |= 1;
                }
                else if (bit == '1') {
                    mask.Oner |= 1;
                    mask.Nuller |= 1;
                }
            }
            return mask;
        }

        private void Task1(List<string> lines)
        {
            var mask = new Mask();
            foreach (var line in lines) {
                var parts = line.Split('=');
                if (parts[0].Trim() == "mask") {
                    mask = this.GenerateMask(parts[1].Trim());   
                }
                else {
                    var address = Int64.Parse(Regex.Match(parts[0], @"[0-9]+").Captures.First().Value);
                    var value = Int64.Parse(parts[1]);
                    value |= mask.Oner;
                    value &= mask.Nuller;
                    this.Memory[address] = value;
                }
            }
            Console.WriteLine(this.Memory.Values.Sum());
        }

        private void Task2(List<string> lines)
        {
            
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
// mem[8] = 11
// mem[7] = 101
// mem[8] = 0";
            var input = this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1(input);
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(input);
        }
    }
}