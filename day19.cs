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
    public class Day19
    {
        public interface IRule {
            int Validate(int curIndex, string data);
        }

        public class CharRule : IRule
        {
            public char Char { get; set; }
            public int Validate(int curIndex, string data)
            {
                if (data[curIndex] == Char)
                {
                    return curIndex + 1;
                }
                return curIndex;
            }
        }

        public class OrRule : IRule {
            public RecursiveRule LeftRule { get; set; } = new RecursiveRule();
            public RecursiveRule RightRule { get; set; } = new RecursiveRule();
            public int Validate(int curIndex, string data) {
                var a = LeftRule.Validate(curIndex, data);
                var b = RightRule.Validate(curIndex, data);
                return new List<int>{a, b}.Max();
            }
        }

        public class RecursiveRule : IRule {
            public List<int> RecursiveRules { get; set; } = new List<int>();

            public int Validate(int curIndex, string data) {
                var nextIndex = curIndex;
                foreach(var rule in RecursiveRules) {
                    nextIndex = RuleSet[rule].Validate(nextIndex, data);
                }

                return nextIndex;
            }
        }

        public static Dictionary<int, IRule> RuleSet = new Dictionary<int, IRule>();

        public List<string> Inputs = new List<string>();

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day19.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            var areas = s.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            foreach(var line in areas[0].Split("\n", StringSplitOptions.RemoveEmptyEntries)) {
                var components = line.Split(":");
                if (components[1].Contains("|")) {
                    // Or rule
                    var orRule = new OrRule();
                    foreach(var nextRule in components[1].Split("|", StringSplitOptions.RemoveEmptyEntries)[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)) {
                        orRule.LeftRule.RecursiveRules.Add(Int32.Parse(nextRule));
                    }

                    foreach(var nextRule in components[1].Split("|", StringSplitOptions.RemoveEmptyEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)) {
                        orRule.RightRule.RecursiveRules.Add(Int32.Parse(nextRule));
                    }

                    RuleSet.Add(Int32.Parse(components[0]), orRule);
                }
                else if (components[1].Contains("a") || components[1].Contains("b")) {
                    // Char rule
                    var charRule = new CharRule{
                        Char = Convert.ToChar(components[1].Trim().Replace("\"", ""))
                    };
                    RuleSet.Add(Int32.Parse(components[0]), charRule);
                }
                else {
                    // Recursive rule
                    var next = components[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var recursiveRule = new RecursiveRule();
                    foreach(var nextRule in next) {
                        recursiveRule.RecursiveRules.Add(Int32.Parse(nextRule));
                    }
                    RuleSet.Add(Int32.Parse(components[0]), recursiveRule);
                }
            }

            foreach(var line in areas[1].Split("\n", StringSplitOptions.RemoveEmptyEntries)) {
                Inputs.Add(line);
            }
        }

        private void Task1()
        {
            foreach(var input in Inputs) {
                Console.WriteLine($"{ input } - {RuleSet[0].Validate(0, input) == input.Length}");
            }

            Console.WriteLine(Inputs.Count(x => RuleSet[0].Validate(0, x) == x.Length));
        }

        private void Task2()
        {
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"0: 4 1 5
// 1: 2 3 | 3 2
// 2: 4 4 | 5 5
// 3: 4 5 | 5 4
// 4: a
// 5: b

// ababbb
// bababa
// abbbab
// aaabbb
// aaaabbb".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}