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
        public class Tiles : IEnumerable<char>
        {
            private SortedDictionary<int, SortedDictionary<int, char>> tiles = new SortedDictionary<int, SortedDictionary<int, char>>();

            public char this[int x, int y] {
                get =>  tiles.TryGetValue(x, out var yzDict) &&
                        yzDict.TryGetValue(y, out var value) ? value : 'w';

                set {
                    if (!tiles.TryGetValue(x, out var yzDict)) {
                        tiles[x] = yzDict = new SortedDictionary<int, char>();
                    }

                    yzDict[y] = value;
                }
            }

            public IEnumerable<int> GetXIndexes() {
                return tiles.Keys;
            }

            public IEnumerable<int> GetYIndexes() {
                return tiles.Values.SelectMany(y => y.Keys).Distinct();
            }

            public IEnumerator<char> GetEnumerator()
            {
                foreach(var y in tiles.Values) {
                    foreach(var tile in y.Values) {
                        yield return tile;

                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public Tiles InitialSetOfTiles = new Tiles();

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

                if (InitialSetOfTiles[x,y] == 'w') {
                    Console.WriteLine($"\n\n{line}\nTile: {x}, {y}: {InitialSetOfTiles[x,y]} -> b");
                    InitialSetOfTiles[x,y] = 'b';
                } else {
                    Console.WriteLine($"\n\n{line}\nTile: {x}, {y}: {InitialSetOfTiles[x,y]} -> w");
                    InitialSetOfTiles[x,y] = 'w';
                }
            }
        }

        private void Task1()
        {
            Console.WriteLine(InitialSetOfTiles.Count(x => x == 'b'));
        }

        private void Task2(int cycles)
        {
             var oldTiles = this.InitialSetOfTiles;
            foreach(var cycle in Enumerable.Range(0, cycles)) {
                var newTiles = new Tiles();
                var firstX = oldTiles.GetXIndexes().Min() - 1;
                var lastX = oldTiles.GetXIndexes().Max() + 1;
                var firstY = oldTiles.GetYIndexes().Min() - 1;
                var lastY = oldTiles.GetYIndexes().Max() + 1;
                for(var x = firstX; x <= lastX; x++) {
                    for(var y = firstY; y <= lastY; y++ ) {
                        var surroundingCubes = new List<char>();
                        surroundingCubes.Add(oldTiles[x-1, y-1]);
                        surroundingCubes.Add(oldTiles[x+1, y+1]);
                        surroundingCubes.Add(oldTiles[x-1, y+1]);
                        surroundingCubes.Add(oldTiles[x+1, y-1]);
                        surroundingCubes.Add(oldTiles[x-2, y]);
                        surroundingCubes.Add(oldTiles[x+2, y]);

                        newTiles[x,y] = oldTiles[x,y];
                        if (oldTiles[x, y] == 'w' && surroundingCubes.Where(tile => tile == 'b').Count() == 2) {
                            newTiles[x, y] = 'b';
                        }
                        else if(oldTiles[x, y] == 'b') {
                            if (surroundingCubes.Count(tile => tile == 'b') == 0 || surroundingCubes.Count(tile => tile == 'b') > 2 ){
                                newTiles[x, y] = 'w';
                            }
                        }
                    }
                };

                oldTiles = newTiles;
            }

            Console.WriteLine(oldTiles.Count(x => x == 'b'));
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
            this.Task2(100);
        }
    }
}