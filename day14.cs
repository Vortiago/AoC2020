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

            public long Xer { get; set; }
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
                mask.Xer <<= 1;
                if (bit == 'X') {
                    mask.Nuller |= 1;
                    mask.Xer |= 1;
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

        private IEnumerable<long> YieldToMe(long address, Mask mask) {
            var addressLength = Convert.ToString(address, 2).Length;
            var reducedOner = mask.Oner & ~(~((long)0) << addressLength);
            var reducedXer = mask.Xer & ~(~((long)0) << addressLength);
            address |= reducedOner;
            for(long round = 0; round <= (address | reducedXer); round++) {
                yield return (address & ~reducedXer) | (reducedXer & round) ;
            }
        }

        private void Task2(List<string> lines)
        {
            Memory.Clear();
            var mask = new Mask();
            foreach (var line in lines) {
                var parts = line.Split('=');
                if (parts[0].Trim() == "mask") {
                    mask = this.GenerateMask(parts[1].Trim());   
                }
                else {
                    var address = Int64.Parse(Regex.Match(parts[0], @"[0-9]+").Captures.First().Value);
                    var value = Int64.Parse(parts[1]);
                    foreach(var maskedAddress in YieldToMe(address, mask)) {
                        this.Memory[maskedAddress] = value;
                    }
                }
            }
            ulong sum = 0;
            foreach(var value in this.Memory.Values) {
                sum += (ulong)value;
            }
            Console.WriteLine(this.Memory.Values.Aggregate((x, y) => x + y));
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
// mem[8] = 11
// mem[7] = 101
// mem[8] = 0";
//             s = @"mask = 000000000000000000000000000000X1001X
// mem[42] = 100
// mask = 00000000000000000000000000000000X0XX
// mem[26] = 1";
            var input = this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1(input);
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(input);
        }
    }
}