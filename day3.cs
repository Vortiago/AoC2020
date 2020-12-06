using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day3 {
        public class Cell {
            public bool Tree { get; set; }
        }

        public class Row {
            public List<Cell> Cells { get; set; }
            public Cell GetCell(int index) {
                var fixedIndex = index;
                while(fixedIndex > this.Cells.Count() - 1) {
                    fixedIndex -= this.Cells.Count();
                }

                return this.Cells.ElementAt(fixedIndex);
            }
        }

        public class Map {
            public List<Row> Rows { get; set; } = new List<Row>();
        }

        private Map map = new Map();

        private int mapLength => this.map.Rows.Count();

        private double GetTreesHit(int x, int y) {
            var currentX = 0;
            var currentY = 0;
            var treesHit = 0;

            while (currentY < this.mapLength) {
                treesHit += this.map.Rows[currentY].GetCell(currentX).Tree ? 1 : 0;
                currentY += y;
                currentX += x;
            }

            return treesHit;
        }

        private void Task1() {
            using(var sr = new StreamReader("input_day3.txt")) {
                map.Rows.AddRange(sr.ReadToEnd()
                    .Split('\n')
                    .Where(text => text.Length > 0)
                    .Select(x => 
                        new Row{Cells = x.Select(y => y == '#' ? new Cell{Tree = true} : new Cell()).ToList()})
                );
            }

            Console.WriteLine(this.GetTreesHit(3, 1));
        }

        private void Task2() {
            var sum = 
                this.GetTreesHit(1, 1) * 
                this.GetTreesHit(3, 1) * 
                this.GetTreesHit(5, 1) * 
                this.GetTreesHit(7, 1) * 
                this.GetTreesHit(1, 2);
            
            Console.WriteLine(sum);
        }

        public void Run() {
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }   
}