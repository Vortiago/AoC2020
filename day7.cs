using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day7
    {
        public class Bag {
            public string Name { get; set; }
            public List<Bag> Content { get; set; } = new List<Bag>();

            public bool CanContainBag(string name) {
                if (this.Content.Count == 0) {
                    return false;
                }
                if (this.Content.Any(x => x.Name == name) || this.Content.Distinct().Any(x => x.CanContainBag(name))) {
                    return true;
                }
                return false;
            }

            public int CountOfChildBags() {
                return this.Content.Count + this.Content.Sum(x => x.CountOfChildBags());
            }

            public override string ToString() {
                return $"{this.Name} - {Content.Count}";
            }
        }

        public List<Bag> Bags = new List<Bag>();
        private void Init()
        {
            using(var sr = new StreamReader("input_day7.txt")) {
                var data = sr.ReadToEnd()
                    .Split("\n")
                    .Where(text => text.Length > 0)
                    .ToList();

                data.ForEach(x => {
                    var name = x.Split("bags contain")[0].Trim();
                    var bag = this.Bags.FirstOrDefault(bag => bag.Name.Equals(name));
                    if (bag is null) {
                        bag = new Bag{Name = name};
                        this.Bags.Add(bag);
                    }
                    var content = x.Split("bags contain")[1].Split(",");
                    foreach (var item in content.Where(contentString => !contentString.Contains("no other bags"))) {
                        var contentName = string.Join(" ", item.Trim().Split(" ").Skip(1).SkipLast(1));
                        var contentNumber = Convert.ToInt32(item.Trim().Split(" ")[0]);
                        var childBag = this.Bags.FirstOrDefault(existingBag => existingBag.Name.Equals(contentName));
                        if (childBag is null) {
                            childBag = new Bag{Name = contentName};
                            this.Bags.Add(childBag);
                        }
                        Enumerable.Range(0, contentNumber).ToList().ForEach(itr => bag.Content.Add(childBag));
                    }
                });
            }
        }

        private void Task1()
        {
            var count = this.Bags.Sum(x => Convert.ToInt32(x.CanContainBag("shiny gold")));
            Console.WriteLine($"{count}");
        }

        private void Task2()
        {
            var sum = this.Bags.First(x => x.Name == "shiny gold").CountOfChildBags();
            Console.WriteLine($"{sum}");
        }

        public void Run()
        {
            this.Init();
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}