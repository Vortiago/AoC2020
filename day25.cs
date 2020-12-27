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
    public class Day25
    {
        public class Key {
            private Int64 pub;
            public Int64 Pub { 
                get => this.pub;
                set {
                    this.pub = value;
                    var tmpValue = 1;
                    while (tmpValue != value) {
                        this.LoopSize += 1;
                        tmpValue *= 7;
                        tmpValue = tmpValue % 20201227;
                    }
                }
            }
            public Int64 LoopSize { get; set; } = 0;
            public Int64 Enc { get; set; }

            public override string ToString()
            {
                return $"\nPub: {this.Pub}\nLoop size: {this.LoopSize}\nEnc: {this.Enc}";
            }
        }

        public Key A;

        public Key B;

        private void ParseTodaysInput(string s) {
            this.A = new Key{
                Pub = Int64.Parse(s.Split("\n", StringSplitOptions.RemoveEmptyEntries)[0]),
            };

            this.B = new Key{
                Pub = Int64.Parse(s.Split("\n", StringSplitOptions.RemoveEmptyEntries)[1]),
            };
        }

        private void Task1()
        {
            var loopSize = this.A.LoopSize;
            var pubKey = this.B.Pub;
            Int64 encKey = 1;
            for(var i = 0; i < loopSize; i++) {
                encKey *= pubKey;
                encKey = encKey % 20201227;
            }

            this.A.Enc = encKey;
            this.B.Enc = encKey;

            Console.WriteLine(this.A);
            Console.WriteLine(this.B);
        }

        private void Task2(int cycles)
        {
        }

        public void Run()
        {
            // var s = @"5764801
            // 17807724".Replace("\r\n", "\n");
            var s = @"8335663
8614349".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(100);
        }
    }
}