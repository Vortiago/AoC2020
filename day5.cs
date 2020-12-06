using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day5 {

        public class Seat {
            public int Row { get; set; }
            public int Column { get; set; }
            public int ID => this.Row * 8 + this.Column;
        }

        public List<Seat> Rows { get; } = new List<Seat>();

        private void Init(string filename) {
            using(var sr = new StreamReader(filename)) {
                var data = sr.ReadToEnd()
                    .Split("\n")
                    .Where(text => text.Length > 0);

                foreach(var positionRow in data) {
                    var row = Enumerable.Range(0, 128);
                    var column = Enumerable.Range(0, 8);

                    foreach(var letter in positionRow) {
                        switch (letter) {
                            case 'B':
                                row = row.Skip((int)(row.Count() / 2.0));
                                break;
                            case 'F':
                                row = row.Take((int)(row.Count() / 2.0));
                                break;
                            case 'R':
                                column = column.Skip((int)(column.Count() / 2.0));
                                break;
                            case 'L':
                                column = column.Take((int)(column.Count() / 2.0));
                                break;
                        }
                    }

                    this.Rows.Add(new Seat{
                        Row = row.First(),
                        Column = column.First()
                    });
                }
                
            }
        }

        private void Task1() {
            Console.WriteLine(this.Rows.Max(x => x.ID));
        }

        private void Task2() {
            Enumerable.Range(this.Rows.Min(x => x.ID), this.Rows.Max(x => x.ID) - this.Rows.Min(x => x.ID)).Except(this.Rows.Select(x => x.ID)).ToList().ForEach(x => Console.Write($"{x} "));
        }

        public void Run(string filename) {
            this.Init(filename);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }   
}