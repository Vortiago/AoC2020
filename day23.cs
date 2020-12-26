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
    public class Day23
    {
        public class CupsGame {
            private int currentCup = 0;
            public int CurrentCup { 
                get {
                    return Cups[currentCup];
                }
                set {
                    currentCup = Cups.IndexOf(value);
                }
            }
            public List<int> Cups { get; set; }= new List<int>();

            public void MoveClockwise() {
                var cupValue = CurrentCup;
                var threeCups = Cups.Skip(currentCup + 1).Take(3).ToList();
                if (threeCups.Count < 3) {
                    threeCups.AddRange(Cups.Take(3 - threeCups.Count));
                }
                threeCups.ForEach(x => Cups.Remove(x));
                var destinationCupValue = cupValue;
                var destinationCupIndex = -1;
                while (destinationCupIndex == -1) {
                    destinationCupValue -= 1;
                    if (destinationCupValue < Cups.Min()) destinationCupValue = Cups.Max();
                    destinationCupIndex = Cups.IndexOf(destinationCupValue);
                    
                }
                Cups.InsertRange(destinationCupIndex + 1, threeCups);
                currentCup = Cups.IndexOf(cupValue);
                currentCup = currentCup == Cups.Count() - 1 ? currentCup = 0 : currentCup + 1;
            }

            public override string ToString()
            {   
                var s = $"CurrentCup {CurrentCup}\n" +
                        $"Cups: {String.Join(" ", Cups)}";
                return s;
            }
        }

        public List<int> Cups = new List<int>();

        private void ParseTodaysInput(string s) {
            Cups = s.Select(x => (int)Char.GetNumericValue(x)).ToList();
        }

        private void Task1()
        {
            var game = new CupsGame() {
                Cups = Cups.Select(x => x).ToList(),
            };

            for(var i = 0; i < 100; i++) {
                game.MoveClockwise();
            }
            Console.WriteLine(game);
        }

        private void Task2()
        {
            var game = new CupsGame() {
                Cups = Cups.Select(x => x).ToList(),
            };
            game.Cups.AddRange(Enumerable.Range(Cups.Max() + 1, (1000000 - Cups.Count())).ToList());
            for(var i = 0; i < 10000000; i++) {
                game.MoveClockwise();
            }
            var indexOne = game.Cups.IndexOf(1);
            Console.WriteLine($"{game.Cups[indexOne + 1]} * {game.Cups[indexOne + 2]} = {Convert.ToInt64(game.Cups[indexOne+1]) * Convert.ToInt64(game.Cups[indexOne+2])}");
        }

        public void Run()
        {
            this.ParseTodaysInput("916438275");
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}