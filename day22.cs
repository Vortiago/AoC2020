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
    public class Day22
    {
        public class Player {
            public List<int> Hand { get; set; } = new List<int>();

            public IEnumerable<int> Draw() {
                var card = Hand.FirstOrDefault();
                if (card != 0) Hand.Remove(card);
                yield return card;
            }

            public int Score() {
                Hand.Reverse();
                return Hand.Select((x,i) => x * (i + 1)).Sum();
            }

            public override string ToString()
            {
                return String.Join(",", Hand);
            }
        }

        public class Game {
            public Player POne { get; set; }
            public Player PTwo { get; set; }
            public List<List<int>> PreviousPOneHands { get; } = new List<List<int>>();
            public List<List<int>> PreviousPTwoHands { get; } = new List<List<int>>();

            private int round = 0;

            public int Play(out int score) {
                score = 0;
                while(true) {
                    round += 1;
                    if(CheckSequenceEqual()) {
                        Console.WriteLine($"Player 1 won by Rule 1 with score {POne.Score()}");
                        score = POne.Score();
                        return 1;
                    }

                    PreviousPOneHands.Add(POne.Hand.Select(x => x).ToList());
                    PreviousPTwoHands.Add(PTwo.Hand.Select(x => x).ToList());

                    var cardA = POne.Draw().FirstOrDefault();
                    var cardB = PTwo.Draw().FirstOrDefault();

                    if (POne.Hand.Count >= cardA && PTwo.Hand.Count >= cardB) {
                        Console.WriteLine($"New recursive game");
                        var game = new Game{
                            POne = new Player {
                                Hand = POne.Hand.Take(cardA).Select(x => x).ToList(),
                            },
                            PTwo = new Player {
                                Hand = PTwo.Hand.Take(cardB).Select(x => x).ToList(),
                            },
                        };

                        var result = game.Play(out var subScore);
                        if (result == 1) {
                            POne.Hand.Add(cardA);
                            POne.Hand.Add(cardB);
                        } else if (result == 2) {
                            PTwo.Hand.Add(cardB);
                            PTwo.Hand.Add(cardA);
                        }
                    } else {
                        if (cardA > cardB) {
                            POne.Hand.Add(cardA);
                            POne.Hand.Add(cardB);
                        } else {
                            PTwo.Hand.Add(cardB);
                            PTwo.Hand.Add(cardA);
                        }
                    }

                    if (POne.Hand.Count == 0) {
                        Console.WriteLine($"Game over, POne hand empty!");
                        score = PTwo.Score();
                        return 2;
                    } else if (PTwo.Hand.Count == 0) {
                        Console.WriteLine($"Game over, PTwo hand empty!");
                        score = POne.Score();
                        return 1;
                    }
                }
            }

            private bool CheckSequenceEqual() {
                if(PreviousPOneHands.All(x => !x.SequenceEqual(POne.Hand)) && PreviousPTwoHands.All(x => !x.SequenceEqual(PTwo.Hand))) {
                    return false;
                }
                return true;
            }
        }

        public Player PlayerOne {get;set;}

        public Player PlayerTwo{get;set;}

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day22.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            var players = s.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            PlayerOne = new Player();
            foreach(var card in players[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)) {
                PlayerOne.Hand.Add(Int32.Parse(card));
            }

            PlayerTwo = new Player();
            foreach(var card in players[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)) {
                PlayerTwo.Hand.Add(Int32.Parse(card));
            }
        }

        private void Task1()
        {
            var cardA = PlayerOne.Draw().First();
            var cardB = PlayerTwo.Draw().First();
            while (true) {
                if (cardA > cardB) {
                    PlayerOne.Hand.Add(cardA);
                    PlayerOne.Hand.Add(cardB);
                } else {
                    PlayerTwo.Hand.Add(cardB);
                    PlayerTwo.Hand.Add(cardA);
                }

                if (PlayerOne.Hand.Count() == 0 || PlayerTwo.Hand.Count() == 0) {
                    break;
                }
                
                cardA = PlayerOne.Draw().FirstOrDefault();
                cardB = PlayerTwo.Draw().FirstOrDefault();
            }

            if (PlayerOne.Hand.Count() == 0) {
                Console.WriteLine($"Player Two Won!: {PlayerTwo.Score()}");
            } else {
                Console.WriteLine($"Player One Won!: {PlayerOne.Score()}");
            }
        }

        private void Task2()
        {
            var game = new Game {
                POne = PlayerOne,
                PTwo = PlayerTwo,
            };

            var result = game.Play(out var score);
            Console.WriteLine($"Game over! Player {result} won with score {score}");
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"Player 1:
// 9
// 2
// 6
// 3
// 1

// Player 2:
// 5
// 8
// 4
// 7
// 10".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.ParseTodaysInput(s);
            this.Task2();
        }
    }
}