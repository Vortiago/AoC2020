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
    public class Day13
    {
        public class Bus {
            public int Id { get; set; }
            public int TimeSincePrevious { get; set; }
            public double GetDepartureAfter(double time) 
            {
                return (Math.Floor(time / Id) * Id) + Id;
            }
        }

        public class Timetable {
            public List<Bus> Buses = new List<Bus>();

            public Bus GetNextBus(int time) {
                return this.Buses.OrderBy(x => x.GetDepartureAfter(time)).First();
            }
            public double GetTimestamp() {
                var maxIncreaseBus = this.Buses.OrderByDescending(x => x.Id).First();
                double timestamp = maxIncreaseBus.Id - maxIncreaseBus.TimeSincePrevious;
                while(true) {
                    if (this.Buses.All(x => (timestamp + x.TimeSincePrevious) % x.Id == 0)) {
                        return timestamp;
                    }
                    timestamp += maxIncreaseBus.Id;
                }
            }
        }

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day13.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private List<string> ParseTodaysInput(string s) {
            return s.Replace("\r", String.Empty).Split("\n").Where(x => x.Length > 0).ToList();
        }

        private void Task1(List<string> data)
        {
            var timetable = new Timetable();
            var timeToLeave = Int32.Parse(data[0]);
            timetable.Buses.AddRange(data[1].Split(',').Where(x => x != "x").Select(x => new Bus{Id = Int32.Parse(x)}));
            var nextBus = timetable.GetNextBus(timeToLeave);
            Console.WriteLine((nextBus.GetDepartureAfter(timeToLeave) - timeToLeave) * nextBus.Id);
        }

        private void Task2(List<string> data)
        {
            var timetable = new Timetable();
            var xRange = 0;
            var uBuses = data[1].Split(',').ToList();
            foreach(var num in Enumerable.Range(0, uBuses.Count())) {
                if (uBuses[num].Equals("x")) xRange += 1;
                else {
                    timetable.Buses.Add(new Bus {
                        Id = Int32.Parse(uBuses[num]),
                        TimeSincePrevious = xRange,
                    });
                    xRange += 1;
                }
            }

            Console.WriteLine(timetable.GetTimestamp());
        }

        public void Run()
        {
            var s = this.Init();
            var data = this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1(data);
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(data);
        }
    }
}