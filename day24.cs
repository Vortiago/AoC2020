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
    public class Day24
    {
        public SortedList<int, SortedList<int, char>> TileGrid = new SortedList<int, SortedList<int, char>>();
        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day24.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            foreach(var line in s.Split("\n", StringSplitOptions.RemoveEmptyEntries)) {
                var x = 0;
                var y = 0;
                for(var i = 0; i < line.Length; i++) {
                    if(line[i] == 'n') {
                        y += 1;
                        i += 1;
                        if (line[i] == 'e') {
                            x += 1;
                        }
                        else if (line[i] == 'w') {
                            x -= 1;
                        }
                    }
                    else if (line[i] == 's') {
                        y -= 1;
                        i += 1;
                        if (line[i] == 'e') {
                            x += 1;
                        }
                        else if (line[i] == 'w') {
                            x -= 1;
                        }
                    }
                    else if (line[i] == 'e') {
                        x += 2;
                    }
                    else if (line[i] == 'w') {
                        x -= 2;
                    }
                }

                if (!TileGrid.TryGetValue(x, out var yList)) {
                    TileGrid[x] = yList = new SortedList<int, char>();
                }
                if (!yList.TryGetValue(y, out var tileFace)) {
                    yList[y] = tileFace = 'w';
                }

                if (tileFace == 'w') {
                    Console.WriteLine($"\n\n{line}\nTile: {x}, {y}: {tileFace} -> b");
                    TileGrid[x][y] = 'b';
                } else {
                    Console.WriteLine($"\n\n{line}\nTile: {x}, {y}: {tileFace} -> w");
                    TileGrid[x][y] = 'w';
                }
            }
        }

        private void Task1()
        {
            Console.WriteLine(TileGrid.SelectMany(x => x.Value).Count(x => x.Value == 'b'));
        }

        private void Task2()
        {
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"sesenwnenenewseeswwswswwnenewsewsw
// neeenesenwnwwswnenewnwwsewnenwseswesw
// seswneswswsenwwnwse
// nwnwneseeswswnenewneswwnewseswneseene
// swweswneswnenwsewnwneneseenw
// eesenwseswswnenwswnwnwsewwnwsene
// sewnenenenesenwsewnenwwwse
// wenwwweseeeweswwwnwwe
// wsweesenenewnwwnwsenewsenwwsesesenwne
// neeswseenwwswnwswswnw
// nenwswwsewswnenenewsenwsenwnesesenew
// enewnwewneswsewnwswenweswnenwsenwsw
// sweneswneswneneenwnewenewwneswswnese
// swwesenesewenwneswnwwneseswwne
// enesenwswwswneneswsenwnewswseenwsese
// wnwnesenesenenwwnenwsewesewsesesew
// nenewswnwewswnenesenwnesewesw
// eneswnwswnwsenenwnwnwwseeswneewsenese
// neswnwewnwnwseenwseesewsenwsweewe
// wseweeenwnesenwwwswnew".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}