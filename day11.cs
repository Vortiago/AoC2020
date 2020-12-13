using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace AdventOfCode
{
    public class Day11
    {
        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day11.txt")) {
                data = sr.ReadToEnd();
            }

            return data;
        }

        private char[,] ParseTodaysInput(string s) {
            var data = s.Split("\n").Where(x => x.Length > 0).ToList();
            var columns = data[0].Length;
            var rows = data.Count();
            var map = new char[rows,columns];

            for (var y = 0; y < rows; y++) {
                for (var x = 0; x < columns; x++) {
                    map[y, x] = data[y].ElementAt(x);
                }
            }

            return map;
        }

        /*
            If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
            If a seat is occupied (#) and four or more seats adjacent to it are also occupied, the seat becomes empty.
            Otherwise, the seat's state does not change.
        */
        private char[,] GenerateNewMapPart1(char[,] map) {
            var newMap = new char[map.GetLength(0), map.GetLength(1)];
            foreach(var y in Enumerable.Range(0, map.GetLength(0))) {
                foreach(var x in Enumerable.Range(0, map.GetLength(1))) {
                    var list = new List<char>();
                    if(y < map.GetLength(0) - 1) {
                        list.Add(map[y+1, x]);
                        if (x < map.GetLength(1) - 1) {
                            list.Add(map[y+1, x+1]);
                        }

                        if (x > 0) {
                            list.Add(map[y+1, x-1]);
                        }
                    }

                    if(x < map.GetLength(1) - 1) {
                        list.Add(map[y, x+1]);
                    }
                    if(x > 0) {
                        list.Add(map[y, x-1]);
                    }

                    if (y > 0) {
                        list.Add(map[y-1, x]);
                        if (x < map.GetLength(1) - 1) {
                            list.Add(map[y-1, x+1]);
                        }

                        if (x > 0) {
                            list.Add(map[y-1, x-1]);
                        }
                    }

                    if (map[y, x] == 'L' && !list.Contains('#')) {
                        newMap[y, x] = '#';
                    }
                    else if (map[y, x] == '#' && list.Count(x => x == '#') >= 4) {
                        newMap[y, x] = 'L';
                    } else {
                        newMap[y, x] = map[y, x];
                    }
                }
            }
            return newMap;
        }
        private void Task1(char[,] map)
        {
            var newMap = this.GenerateNewMapPart1(map);
            var oldMap = map;
            while(!newMap.Cast<char>().SequenceEqual(oldMap.Cast<char>())) {
                oldMap = newMap;
                newMap = this.GenerateNewMapPart1(oldMap);
            }

            Console.WriteLine(oldMap.Cast<char>().Count(x => x == '#'));
        }
        private char[,] GenerateNewMapPart2(char[,] map) {
            var newMap = new char[map.GetLength(0), map.GetLength(1)];
            foreach(var y in Enumerable.Range(0, map.GetLength(0))) {
                foreach(var x in Enumerable.Range(0, map.GetLength(1))) {
                    var list = new List<char>();
                    var right = Enumerable.Range(x+1, map.GetLength(1) - x - 1);
                    var left = Enumerable.Range(0, x).Reverse();
                    var up = Enumerable.Range(0, y).Reverse();
                    var down = Enumerable.Range(y+1, map.GetLength(0) - y - 1);
                    
                    foreach(var pos in right) {
                        if (map[y, pos] == 'L' || map[y, pos] == '#') {
                            list.Add(map[y, pos]);
                            break;
                        }
                    }

                    foreach(var pos in right.Take(up.Count())) {
                        var element = map[up.ElementAt(right.ToList().IndexOf(pos)), pos];
                        if (element == 'L' || element == '#') {
                            list.Add(element);
                            break;
                        }
                    }

                    foreach(var pos in right.Take(down.Count())) {
                        var element = map[down.ElementAt(right.ToList().IndexOf(pos)), pos];
                        if (element == 'L' || element == '#') {
                            list.Add(element);
                            break;
                        }
                    }

                    foreach(var pos in left) {
                        if (map[y, pos] == 'L' || map[y, pos] == '#') {
                            list.Add(map[y, pos]);
                            break;
                        }
                    }

                    foreach(var pos in left.Take(up.Count())) {
                        var element = map[up.ElementAt(left.ToList().IndexOf(pos)), pos];
                        if (element == 'L' || element == '#') {
                            list.Add(element);
                            break;
                        }
                    }

                    foreach(var pos in left.Take(down.Count())) {
                        var element = map[down.ElementAt(left.ToList().IndexOf(pos)), pos];
                        if (element == 'L' || element == '#') {
                            list.Add(element);
                            break;
                        }
                    }

                    foreach(var pos in up) {
                        if (map[pos, x] == 'L' || map[pos, x] == '#') {
                            list.Add(map[pos, x]);
                            break;
                        }
                    }

                    foreach(var pos in down) {
                        if (map[pos, x] == 'L' || map[pos, x] == '#') {
                            list.Add(map[pos, x]);
                            break;
                        }
                    }

                    if (map[y, x] == 'L' && !list.Contains('#')) {
                        newMap[y, x] = '#';
                    }
                    else if (map[y, x] == '#' && list.Count(x => x == '#') >= 5) {
                        newMap[y, x] = 'L';
                    } else {
                        newMap[y, x] = map[y, x];
                    }
                }
            }
            return newMap;
        }
        private void Task2(char[,] map)
        {
            var newMap = this.GenerateNewMapPart2(map);
            var oldMap = map;
            while(!newMap.Cast<char>().SequenceEqual(oldMap.Cast<char>())) {
                oldMap = newMap;
                newMap = this.GenerateNewMapPart2(oldMap);
            }

            Console.WriteLine(oldMap.Cast<char>().Count(x => x == '#'));
        }

        public void Run()
        {
            var s = this.Init();
            var map = this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1(map);
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(map);
        }
    }
}