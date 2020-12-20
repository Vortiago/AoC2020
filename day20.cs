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
    public class Day20
    {
        public class Tile {
            public Int64 Id { get; set; }

            public Char[,] Pixels { get; set; } = new Char[10,10];

            public IEnumerable<IEnumerable<char>> Edges() {
                yield return Enumerable.Range(0, Pixels.GetLength(0)).Select(x => Pixels[x, 0]);
                yield return Enumerable.Range(0, Pixels.GetLength(1)).Select(x => Pixels[Pixels.GetUpperBound(0), x]);
                yield return Enumerable.Range(0, Pixels.GetLength(0)).Select(x => Pixels[x, Pixels.GetUpperBound(1)]);
                yield return Enumerable.Range(0, Pixels.GetLength(1)).Select(x => Pixels[0, x]);
            }
        }

        public List<Tile> TileList = new List<Tile>();

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day20.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            var tiles = s.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            foreach(var sTile in tiles) {
                var rows = sTile.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                var tile = new Tile();
                tile.Id = Int64.Parse(Regex.Match(rows[0], @"\d+").Value);
                for(var x = 0; x < rows[1].Length; x++) {
                    for (var y = 1; y < rows.Length; y++) {
                        tile.Pixels[x,y - 1] = rows[y][x];
                    }
                }

                this.TileList.Add(tile);
            }
        }

        private void Task1()
        {
            foreach(var edge in this.TileList[0].Edges()) {
                Console.WriteLine(String.Join("", edge));
            }

            Int64 sum = 1;

            foreach(var tile in this.TileList) {
                var edgesFound = 0;
                foreach(var otherTile in this.TileList) {
                    if (otherTile == tile) continue;
                    foreach(var edge in tile.Edges()) {
                        foreach(var otherTileEdge in otherTile.Edges()) {
                            if (otherTileEdge.SequenceEqual(edge) || 
                                otherTileEdge.SequenceEqual(edge.Reverse())) {
                                    edgesFound += 1;
                                }
                        }
                    }
                }
                if (edgesFound == 2) {
                    sum *= tile.Id;
                }
                Console.WriteLine($"Tile: {tile.Id}, {edgesFound} Edges hit");
            }

            Console.WriteLine(sum);
        }

        private void Task2()
        {
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"Tile 2311:
// ..##.#..#.
// ##..#.....
// #...##..#.
// ####.#...#
// ##.##.###.
// ##...#.###
// .#.#.#..##
// ..#....#..
// ###...#.#.
// ..###..###

// Tile 1951:
// #.##...##.
// #.####...#
// .....#..##
// #...######
// .##.#....#
// .###.#####
// ###.##.##.
// .###....#.
// ..#.#..#.#
// #...##.#..

// Tile 1171:
// ####...##.
// #..##.#..#
// ##.#..#.#.
// .###.####.
// ..###.####
// .##....##.
// .#...####.
// #.##.####.
// ####..#...
// .....##...

// Tile 1427:
// ###.##.#..
// .#..#.##..
// .#.##.#..#
// #.#.#.##.#
// ....#...##
// ...##..##.
// ...#.#####
// .#.####.#.
// ..#..###.#
// ..##.#..#.

// Tile 1489:
// ##.#.#....
// ..##...#..
// .##..##...
// ..#...#...
// #####...#.
// #..#.#.#.#
// ...#.#.#..
// ##.#...##.
// ..##.##.##
// ###.##.#..

// Tile 2473:
// #....####.
// #..#.##...
// #.##..#...
// ######.#.#
// .#...#.#.#
// .#########
// .###.#..#.
// ########.#
// ##...##.#.
// ..###.#.#.

// Tile 2971:
// ..#.#....#
// #...###...
// #.#.###...
// ##.##..#..
// .#####..##
// .#..####.#
// #..#.#..#.
// ..####.###
// ..#.#.###.
// ...#.#.#.#

// Tile 2729:
// ...#.#.#.#
// ####.#....
// ..#.#.....
// ....#..#.#
// .##..##.#.
// .#.####...
// ####.#.#..
// ##.####...
// ##..#.##..
// #.##...##.

// Tile 3079:
// #.#.#####.
// .#..######
// ..#.......
// ######....
// ####.#..#.
// .#...#.##.
// #.#####.##
// ..#.###...
// ..#.......
// ..#.###...".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}