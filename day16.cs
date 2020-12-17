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
    public class Day16
    {

        public class Rule {
            public string Name { get; set; }

            public List<Tuple<int, int>> Ranges { get; set; } = new List<Tuple<int, int>>();

            public bool CheckValid(int number) {
                foreach(var range in this.Ranges) {
                    if (range.Item1 <= number && number <= range.Item2) {
                        return true;
                    }
                }

                return false;
            }
        }

        public List<Rule> Rules { get; } = new List<Rule>();

        public class Ticket {
            public List<int> FieldValues { get; } = new List<int>();
        }

        public Ticket MyTicket { get; set; }

        public List<Ticket> OtherTickets { get; } = new List<Ticket>();

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day16.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            var bulks = s.Split("\n\n");
            foreach(var rule in bulks[0].Split("\n")) {
                this.Rules.Add(this.ParseRule(rule));
            }

            this.MyTicket = this.ParseTicket(bulks[1].Split("\n")[1]);
            foreach(var ticket in bulks[2].Split("\n").Where(x => x.Length > 0).Skip(1)) {
                this.OtherTickets.Add(this.ParseTicket(ticket));
            }
        }

        private Rule ParseRule(string ruleAsString) {
            // class: 1-3 or 5-7
            var rule = new Rule{
                Name = ruleAsString.Split(":")[0],
            };

            foreach(var range in ruleAsString.Split(":")[1].Split(" or "))
            {
                var components = range.Split("-");
                rule.Ranges.Add(new Tuple<int, int>(Int32.Parse(components[0]), Int32.Parse(components[1])));
            }

            return rule;
        }

        private Ticket ParseTicket(string ticketAsString) {
            //your ticket:
            //7,1,14
            var ticket = new Ticket();
            ticketAsString.Split(",").ToList().ForEach(x => ticket.FieldValues.Add(Int32.Parse(x)));
            return ticket;
        }

        private void Task1()
        {
            var invalid = this.OtherTickets.SelectMany(x => x.FieldValues.Where(y => this.Rules.All(z => !z.CheckValid(y))));
            Console.WriteLine(invalid.ToList().Sum());
        }

        private void Task2()
        {
            var rulePositions = new List<int>();
            while(this.Rules.Count > 0) {
                for(var i = 0; i < this.MyTicket.FieldValues.Count; i++) 
                {
                    var ticketField = this.OtherTickets.Select(x => x.FieldValues[i]).Where(x => this.Rules.Any(rule => rule.CheckValid(x))).ToList();
                    if(this.Rules.Count(x => ticketField.All(y => x.CheckValid(y))) == 1)
                    {
                        var rule = this.Rules.First(x => ticketField.All(y => x.CheckValid(y)));
                        this.Rules.Remove(rule);
                        if (rule.Name.StartsWith("departure")) {
                            rulePositions.Add(i);
                        }
                    }
                }   
            }
            
            Console.WriteLine(MyTicket.FieldValues.Where((field, index) => rulePositions.Contains(index)).Aggregate((long)1, (x, y) => x*y));
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"class: 1-3 or 5-7
// row: 6-11 or 33-44
// seat: 13-40 or 45-50

// your ticket:
// 7,1,14

// nearby tickets:
// 7,3,47
// 40,4,50
// 55,2,20
// 38,6,12
// ".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}