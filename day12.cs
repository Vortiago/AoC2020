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
    public class Day12
    {
        public enum Direction {
            North,
            South,
            West,
            East
        }

        public enum Orientation {
            Left,
            Right
        }

        public class Waypoint {
            public int X { get; private set; } = 10;
            public int Y { get; private set; } = 1;
            public void Move(Direction direction, int distance) {
                switch (direction) {
                    case Direction.East:
                        X += distance;
                        break;
                    case Direction.West:
                        X -= distance;
                        break;
                    case Direction.North:
                        Y += distance;
                        break;
                    case Direction.South:
                        Y -= distance;
                        break;
                }
            }

            public void Rotate(Orientation orientation, int degrees) {
                switch (orientation) {
                    case Orientation.Left:
                        foreach(var x in Enumerable.Range(0, degrees/90)) {
                            if (this.X > 0 && this.Y > 0) {
                                var tmp = Y;
                                Y = X;
                                X = -tmp;
                            }
                            else if (this.X < 0 && this.Y > 0) {
                                var tmp = Y;
                                Y = X;
                                X = -tmp;
                            }
                            else if (this.X < 0 && this.Y < 0) {
                                var tmp = Y;
                                Y = X;
                                X = Math.Abs(tmp);
                            }
                            else if (this.X > 0 && this.Y < 0) {
                                var tmp = Y;
                                Y = X;
                                X = Math.Abs(tmp);
                            }
                        }
                        break;
                    case Orientation.Right:
                        foreach(var x in Enumerable.Range(0, degrees / 90)) {
                            if (this.X > 0 && this.Y > 0) {
                                var tmp = Y;
                                Y = -X;
                                X = tmp;
                            }
                            else if (this.X > 0 && this.Y < 0) {
                                var tmp = Y;
                                Y = -X;
                                X = tmp;
                            }
                            else if (this.X < 0 && this.Y < 0) {
                                var tmp = Y;
                                Y = Math.Abs(X);
                                X = tmp;
                            }
                            else if (this.X < 0 && this.Y > 0) {
                                var tmp = Y;
                                Y = Math.Abs(X);
                                X = tmp;
                            }                            
                        }
                        break;
                }
            }
        }
        public class Boat {
            public int X { get; private set; } = 0;
            public int Y { get; private set; } = 0;
            public int Orientation { get; private set; } = 0;
            public void Move(string cmd) {
                var num = Int32.Parse(cmd.Substring(1));
                switch (cmd.Substring(0, 1)) {
                    case "N":
                        Y += num;
                        break;
                    case "S":
                        Y -= num;
                        break;
                    case "E":
                        X += num;
                        break;
                    case "W":
                        X -= num;
                        break;
                    case "L":
                        Orientation += num;
                        if (Orientation >= 360) Orientation -= 360;
                        break;
                    case "R":
                        Orientation -= num;
                        if (Orientation < 0) Orientation += 360;
                        break;
                    case "F":
                        if(Orientation == 0) {
                            X += num;
                        }
                        else if(Orientation % 180 == 0) {
                            X -= num;
                        }
                        else if(Orientation % 270 == 0) {
                            Y -= num;
                        }
                        else if(Orientation % 90 == 0) {
                            Y += num;
                        }
                        break;
                }
            }
            public void Move(int x, int y) {
                X += x;
                Y += y;
            }
        }

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day12.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private List<string> ParseTodaysInput(string s) {
            return s.Replace("\r", String.Empty).Split("\n").Where(x => x.Length > 0).ToList();
        }

        private void Task1(List<string> data)
        {
            var boat = new Boat();
            foreach(var cmd in data) {
                boat.Move(cmd);
            }

            Console.WriteLine(Math.Abs(boat.X) + Math.Abs(boat.Y));
        }

        private void Task2(List<string> data)
        {
            var boat = new Boat();
            var waypoint = new Waypoint();
            foreach (var cmd in data) {
                var num = Int32.Parse(cmd.Substring(1));
                switch (cmd.Substring(0, 1)) {
                    case "N":
                        waypoint.Move(Direction.North, num);
                        break;
                    case "S":
                        waypoint.Move(Direction.South, num);
                        break;
                    case "E":
                        waypoint.Move(Direction.East, num);
                        break;
                    case "W":
                        waypoint.Move(Direction.West, num);
                        break;
                    case "L":
                        waypoint.Rotate(Orientation.Left, num);
                        break;
                    case "R":
                        waypoint.Rotate(Orientation.Right, num);
                        break;
                    case "F":
                        foreach(var it in Enumerable.Range(0, num)) {
                            boat.Move(waypoint.X, waypoint.Y);
                        }
                        break;
                }
            }
            
            Console.WriteLine(Math.Abs(boat.X) + Math.Abs(boat.Y));
        }

        public void Run()
        {
            var s = this.Init();
            /*s = @"F10
N3
F7
R90
F11";*/
            var data = this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1(data);
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(data);
        }
    }
}