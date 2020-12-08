using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day8
    {
        /*
        acc 
        increases or decreases a single global value called the accumulator by the value given in the argument. 
        For example, acc +7 would increase the accumulator by 7. 
        The accumulator starts at 0. 
        After an acc instruction, the instruction immediately below it is executed next.
        
        jmp 
        jumps to a new instruction relative to itself. 
        The next instruction to execute is found using the argument as an offset from the jmp instruction; 
        for example, jmp +2 would skip the next instruction, 
        jmp +1 would continue to the instruction immediately below it, 
        and jmp -20 would cause the instruction 20 lines above to be executed next.
        
        nop 
        stands for No OPeration - it does nothing. The instruction immediately below it is executed next.
*/  

        public interface iInstruction {
            int Addr { get; set; }
            bool Spent { get; set; }
            int NextAddr(ref int accumulator);
        }

        public class Acc : iInstruction {
            public int Addr { get; set; }

            public int AccumulatorIncrease { get; set; }

            public bool Spent { get; set; } = false;

            public int NextAddr(ref int accumulator) {
                this.Spent = true;
                accumulator += this.AccumulatorIncrease;
                return this.Addr + 1;
            }

        }

        public class Jmp : iInstruction {
            public int Addr { get; set; }

            public int JumpOffset { get; set; }

            public bool Spent { get; set; } = false;

            public int NextAddr(ref int accumulator) {
                this.Spent = true;
                return this.Addr + this.JumpOffset;
            }
        }

        public class NoOp : iInstruction {
            public int Addr { get; set; }

            public int ExtraValue { get; set; }
            
            public bool Spent { get; set; } = false;

            public int NextAddr(ref int accumulator) {
                this.Spent = true;
                return this.Addr + 1;
            }
        }

        List<iInstruction> Instructions { get; } = new List<iInstruction>();

        private void Init()
        {
            this.Instructions.Clear();
            using(var sr = new StreamReader("input_day8.txt")) {
                var data = sr.ReadToEnd()
                    .Split("\n")
                    .Where(text => text.Length > 0)
                    .ToList();

                data.ForEach(x => {
                    var instruction = x.Split(" ");
                    switch (instruction[0]) {
                        case "acc": 
                            this.Instructions.Add(
                                new Acc {
                                    Addr = this.Instructions.Count,
                                    AccumulatorIncrease = Int32.Parse(instruction[1])
                                }
                            );
                            break;
                        case "jmp":
                            this.Instructions.Add(
                                new Jmp{
                                    Addr = this.Instructions.Count,
                                    JumpOffset = Int32.Parse(instruction[1])
                                }
                            );
                            break;
                        case "nop":
                            this.Instructions.Add(
                                new NoOp{
                                    Addr = this.Instructions.Count,
                                    ExtraValue = Int32.Parse(instruction[1])
                                }
                            );
                            break;
                    };
                });
            }
        }

        private void Task1()
        {
            var accumulator = 0;
            var nextInstruction = 0;
            do {
                nextInstruction = this.Instructions.ElementAt(nextInstruction).NextAddr(ref accumulator);
            } while (!this.Instructions.ElementAt(nextInstruction).Spent);

            Console.WriteLine(accumulator);
        }

        private void Task2()
        {
            var iterList = this.Instructions.Where(x => x is NoOp || x is Jmp).ToList();
            foreach(var instruction in iterList) {
                this.Init();
                switch(instruction) {
                    case NoOp x:
                        this.Instructions[x.Addr] = new Jmp {
                            Addr = x.Addr,
                            JumpOffset = x.ExtraValue
                        };
                        break;

                    case Jmp x:
                        this.Instructions[x.Addr] = new NoOp {
                            Addr = x.Addr,
                            ExtraValue = x.JumpOffset
                        };
                        break;
                }

                var accumulator = 0;
                var nextInstruction = 0;
                do {
                    nextInstruction = this.Instructions.ElementAt(nextInstruction).NextAddr(ref accumulator);
                } while (nextInstruction < this.Instructions.Count && !this.Instructions.ElementAt(nextInstruction).Spent);

                if (nextInstruction >= this.Instructions.Count) Console.WriteLine(accumulator);
            }
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