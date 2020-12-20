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
                if (curIndex >= data.Length) {
                    return -1;
                }
                if (data[curIndex] == Char)
                {
                    return curIndex + 1;
                }
                return -1;
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
                    if (nextIndex == -1) {
                        return -1;
                    }
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
                Console.WriteLine($"{ input } - {RuleSet[0].Validate(0, input)} - {RuleSet[0].Validate(0, input) == input.Length}");
            }

            Console.WriteLine(Inputs.Count(x => RuleSet[0].Validate(0, x) == x.Length));
        }

        private void Task2()
        {
        }

        public void Run()
        {
            var s = this.Init();
            s = @"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: a
11: 42 31 | 42 100 31
100: 42 31 | 42 101 31
101: 42 31 | 42 102 31
102: 42 31 | 42 103 31
103: 42 31 | 42 104 31
104: 42 31 | 42 105 31
105: 42 31 | 42 106 31
106: 42 31 | 42 107 31
107: 42 31 | 42 108 31
108: 42 31 | 42 109 31
109: 42 31 | 42 110 31
110: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: b
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42 | 42 200
200: 42 | 42 201
201: 42 | 42 202
202: 42 | 42 203
203: 42 | 42 204
204: 42 | 42 205
205: 42 | 42 206
206: 42 | 42 207
207: 42 
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}