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
            public List<int> Hand {get;} = new List<int>();

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
                return String.Join("\n", Hand);
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
            this.Task2();
        }
    }
}