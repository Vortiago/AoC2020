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
    public class Day18
    {
        public class Equation {
            private List<string> operators = new List<string>{"+", "*"};
            public Int64 Answer() {
                var processed = TextRepresentation;
                while(Regex.IsMatch(processed, @"\((\d+ [+\-*\\] )+\d+\)")) {
                    var equation = Regex.Match(processed, @"\((\d+ [+\-*\\] )+\d+\)")
                        .Captures
                        .First()
                        .Value;

                    var newValue = this.ProcessEquation(
                        equation
                            .Replace("(", "")
                            .Replace(")", "")
                            .Split(" ", StringSplitOptions.RemoveEmptyEntries));

                    processed = processed.Replace(equation, $"{ newValue }");
                }

                return this.ProcessEquation(processed.Split(" ", StringSplitOptions.RemoveEmptyEntries));
            }

            public Int64 AdvancedMathAnswer() {
                var processed = TextRepresentation;
                while(Regex.IsMatch(processed, @"\((\d+ [+\-*\\] )+\d+\)")) {
                    var equation = Regex.Match(processed, @"\((\d+ [+\-*\\] )+\d+\)")
                        .Captures
                        .First()
                        .Value;;

                    var newValue = this.ProcessAdvancedMath(equation.Replace("(", "").Replace(")", ""));

                    processed = processed.Replace(equation, $"{ newValue }");
                }

                return this.ProcessAdvancedMath(processed);;
            }

            private Int64 TwoNumbers(string numbers) {
                var tmp = numbers.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if(tmp.Contains("+")) {
                    return Int64.Parse(tmp[0]) + Int64.Parse(tmp[2]);
                }

                return Int64.Parse(tmp[0]) * Int64.Parse(tmp[2]);
            }

            private Int64 ProcessAdvancedMath(string equation) {
                var processed = equation;
                while(Regex.IsMatch(processed, @"\d+ \+ \d+")) {
                    var match = Regex.Match(processed, @"\d+ \+ \d+").Captures.First().Value;
                    var newValue = TwoNumbers(match);
                    processed = processed.Replace(match, $"{ newValue }");
                }

                while(Regex.IsMatch(processed, @"\d+ \* \d+")) {
                    var match = Regex.Match(processed, @"\d+ \* \d+").Captures.First().Value;
                    var newValue = TwoNumbers(match);
                    processed = processed.Replace(match, $"{ newValue }");
                }

                return Int64.Parse(processed);
            }

            private Int64 ProcessEquation(IEnumerable<string> equation) {
                Int64 sum = 0;
                var previousOperator = "+";
                foreach(var item in equation) {
                    if (operators.Contains(item)) {
                        previousOperator = item;
                    }
                    else {
                        switch(previousOperator) {
                            case "+":
                                sum += Int64.Parse(item);
                                break;
                            case "*":
                                sum *= Int64.Parse(item);
                                break;
                        }
                    }
                }

                return sum;
            }

            public string TextRepresentation { get; set; }
        }

        public List<Equation> Equations { get; } = new List<Equation>();
        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day18.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            foreach(var equation in s.Split("\n", StringSplitOptions.RemoveEmptyEntries)) {
                this.Equations.Add(new Equation{ TextRepresentation = equation.Trim() });
            }
        }

        private void Task1()
        {
            Console.WriteLine(this.Equations.Sum(equation => equation.Answer()));
        }

        private void Task2()
        {
            Console.WriteLine(this.Equations.Sum(equation => equation.AdvancedMathAnswer()));
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"2 * 3 + (4 * 5)
// 5 + (8 * 3 + 9 + 3 * 4 * 3)
// 5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))
// ((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}