using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Collections;
using System.Text;

namespace AdventOfCode
{
    public static class Extensions {
        public static char[,] FlipX(this char[,] charArray)
        {
            var tmpArray = new Char[charArray.GetLength(0), charArray.GetLength(1)];
            for (var x = 0; x < charArray.GetLength(0); x++)
            {
                for (var y = 0; y < charArray.GetLength(1); y++)
                {
                    tmpArray[charArray.GetUpperBound(0) - x, y] = charArray[x, y];
                }
            }

            return tmpArray;
        }

        public static char[,] FlipY(this char[,] charArray)
        {
            var tmpArray = new Char[charArray.GetLength(0), charArray.GetLength(1)];
            for (var x = 0; x < charArray.GetLength(0); x++)
            {
                for (var y = 0; y < charArray.GetLength(1); y++)
                {
                    tmpArray[x, charArray.GetUpperBound(1) - y] = charArray[x, y];
                }
            }

            return tmpArray;
        }

        public static char[,] Rotate(this char[,] charArray)
        {
            var tmpArray = new Char[charArray.GetLength(0), charArray.GetLength(1)];
            for (var x = 0; x < charArray.GetLength(0); x++)
            {
                for (var y = 0; y < charArray.GetLength(1); y++)
                {
                    tmpArray[y, x] = charArray[x, y];
                }
            }

            return tmpArray.FlipX();
        }
    }

    public class Day20
    {
        public class Tile
        {
            public int KnownEdges { get; set; }
            public Int64 Id { get; set; }
            public Char[,] Pixels { get; set; } = new Char[10, 10];
            public IEnumerable<char> TopEdge => Enumerable.Range(0, Pixels.GetLength(0)).Select(x => Pixels[x, 0]);
            public IEnumerable<char> RightEdge => Enumerable.Range(0, Pixels.GetLength(1)).Select(x => Pixels[Pixels.GetUpperBound(0), x]);
            public IEnumerable<char> BottomEdge => Enumerable.Range(0, Pixels.GetLength(0)).Select(x => Pixels[x, Pixels.GetUpperBound(1)]);
            public IEnumerable<char> LeftEdge => Enumerable.Range(0, Pixels.GetLength(1)).Select(x => Pixels[0, x]);

            public IEnumerable<IEnumerable<char>> Edges()
            {
                yield return TopEdge;
                yield return RightEdge;
                yield return BottomEdge;
                yield return LeftEdge;
            }

            public Tile Rotate()
            {
                this.Pixels = Pixels.Rotate();
                return this;
            }

            public Tile FlipX() {
                this.Pixels = Pixels.FlipX();
                return this;
            }

            public Tile FlipY()
            {
                this.Pixels = Pixels.FlipY();
                return this;
            }

            public override string ToString()
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append($"Tile {this.Id}{Environment.NewLine}");
                for (var y = 0; y < 10; y++)
                {
                    for (var x = 0; x < 10; x++)
                    {
                        stringBuilder.Append(this.Pixels[x, y]);
                    }
                    stringBuilder.Append(Environment.NewLine);
                }

                return stringBuilder.ToString();
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

                tile.KnownEdges = edgesFound;
                Console.WriteLine($"Tile: {tile.Id}, {edgesFound} Edges hit");
            }

            Console.WriteLine(sum);
        }

        public bool CheckLeftEdge(IEnumerable<char> EdgeToFind, Tile tile) {
            return  EdgeToFind.SequenceEqual(tile.LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().FlipX().LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().LeftEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().FlipX().LeftEdge);
        }

        public bool CheckTopEdge(IEnumerable<char> EdgeToFind, Tile tile) {
            return  EdgeToFind.SequenceEqual(tile.TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().FlipX().TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().TopEdge) ||
                    EdgeToFind.SequenceEqual(tile.Rotate().FlipX().TopEdge);
        }

        public Tile FindRightTile(Tile a) {
            foreach(var tile in this.TileList.Where(x => x != a)) {
                if(CheckLeftEdge(a.RightEdge, tile)) {
                    return tile;
                }
            }
            return null;
        }

        public Tile FindBelowTile(Tile a) {
            foreach(var tile in this.TileList.Where(x => x != a)) {
                if(CheckTopEdge(a.BottomEdge, tile)) {
                    return tile;
                }
            }
            return null;
        }

        public Tile GetTopLeftCornerTile() {
            var initialTile = this.TileList.First(x => x.KnownEdges == 2);
            // Let's rotate and flip it to fit the top left corner
            if (FindBelowTile(initialTile) == null) {
                initialTile.FlipX();
            }
            if(FindBelowTile(initialTile) == null) {
                initialTile.FlipY();
            }
            if(FindRightTile(initialTile) == null) {
                initialTile.FlipX();
            }

            return initialTile;
        }

        public List<List<Tile>> Map = new List<List<Tile>>();

        private void Task2()
        {
            var leftTile = this.GetTopLeftCornerTile();
            do {
                var row = new List<Tile>();
                Tile nextRightTile = leftTile;
                do {
                    row.Add(nextRightTile);
                    nextRightTile = FindRightTile(nextRightTile);
                } while (nextRightTile != null);

                Map.Add(row);
                leftTile = FindBelowTile(leftTile);
            } while (leftTile != null);

            var highWater = 0;
            char[,] charMap = new char[Map[0].Count()*8, Map.Count()*8];
            for(var row = 0; row < Map.Count(); row++) {
                for(var column = 0; column < Map[0].Count(); column++) {
                    for(var y = 1; y < 9; y++) {
                        for(var x = 1; x < 9; x++) {
                            charMap[column*8 + (x-1), row*8 + (y-1)] = Map[row][column].Pixels[x, y]; 
                            if (Map[row][column].Pixels[x, y] == '#') {
                                highWater += 1;
                            }
                        }
                    }
                }
            }

            for(var y = 0; y < charMap.GetLength(1); y++) {
                for (var x = 0; x < charMap.GetLength(0); x++) {
                    Console.Write(charMap[x,y]);
                }

                Console.WriteLine();
            }

            
            for(var i = 0; i < 4; i++) {
                var stringBuild = new StringBuilder();
                for(var y = 0; y < charMap.GetLength(1); y++) {
                    for (var x = 0; x < charMap.GetLength(0); x++) {
                        stringBuild.Append(charMap[x,y]);
                    }

                    stringBuild.Append(Environment.NewLine);
                }
                var textMap = stringBuild.ToString();

                var numMonsters = FindMonsters(textMap);
                charMap = charMap.Rotate();
            }

            var monsters = CheckForMonstersAllRotations(charMap);
            if (monsters == 0) {
                charMap = charMap.FlipX();
                monsters = CheckForMonstersAllRotations(charMap);
                if (monsters == 0) {
                    charMap = charMap.FlipY();
                    monsters = CheckForMonstersAllRotations(charMap);
                    if (monsters == 0) {
                        charMap = charMap.FlipX();
                        monsters = CheckForMonstersAllRotations(charMap);
                    }
                }
            }
            Console.WriteLine($"Monsters: {monsters}");
            Console.WriteLine($"High water: {highWater - (monsters*15)}");
            
        }


        public int CheckForMonstersAllRotations(char[,] charMap) {
            for(var i = 0; i < 3; i++) {
                var stringBuild = new StringBuilder();
                for(var y = 0; y < charMap.GetLength(1); y++) {
                    for (var x = 0; x < charMap.GetLength(0); x++) {
                        stringBuild.Append(charMap[x,y]);
                    }

                    stringBuild.Append(Environment.NewLine);
                }
                var textMap = stringBuild.ToString();

                var numMonsters = FindMonsters(textMap);
                if (numMonsters > 0) {
                    return numMonsters;
                }
                charMap = charMap.Rotate();
            }
            return 0;
        }

        public int FindMonsters(string image) {
            var monsters = 0;
            var lineArray = image.Split(Environment.NewLine);
            for(var i = 1; i < lineArray.Length; i++) {
                var potensialMonsters = Regex.Matches(lineArray[i], @"#.{4}##.{4}##.{4}###");
                foreach(Match potensialMonster in potensialMonsters) {
                    if (lineArray[i-1][potensialMonster.Index + 18] == '#' &&
                        Regex.IsMatch(lineArray[i+1].Substring(potensialMonster.Index, 17), @"#.{2}#.{2}#.{2}#.{2}#.{2}#")) {
                        monsters += 1;
                    }
                }
            }

            return monsters;
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