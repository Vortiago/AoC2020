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
    public class Day17
    {
        public enum CubeState {
            Inactive,
            Active
        }

        public class Cube {
            public CubeState State { get; set; } = CubeState.Inactive;
        }

        public class Cubes : IEnumerable<Cube>
        {
            private SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, Cube>>>> cubes = new SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, Cube>>>>();

            public Cube this[int x, int y, int z, int w = 0] {
                get =>  cubes.TryGetValue(x, out var yzDict) &&
                        yzDict.TryGetValue(y, out var zDict) &&
                        zDict.TryGetValue(z, out var wDict) &&
                        wDict.TryGetValue(w, out var value) ? value : new Cube{ State = CubeState.Inactive };

                set {
                    if (!cubes.TryGetValue(x, out var yzDict)) {
                        cubes[x] = yzDict = new SortedDictionary<int, SortedDictionary<int, SortedDictionary<int, Cube>>>();
                    }
                    if (!yzDict.TryGetValue(y, out var zDict)) {
                        yzDict[y] = zDict = new SortedDictionary<int, SortedDictionary<int, Cube>>();
                    }
                    if (!zDict.TryGetValue(z, out var wDict)) {
                        zDict[z] = wDict = new SortedDictionary<int, Cube>();
                    }

                    wDict[w] = value;
                }
            }

            public IEnumerable<int> GetXIndexes() {
                return cubes.Keys;
            }

            public IEnumerable<int> GetYIndexes() {
                return cubes.Values.SelectMany(y => y.Keys).Distinct();
            }

            public IEnumerable<int> GetZIndexes() {
                return cubes.Values.SelectMany(y => y.Values.SelectMany(z => z.Keys).Distinct());
            }

            public IEnumerable<int> GetWIndexes() {
                return cubes.Values.SelectMany(y => y.Values.SelectMany(z => z.Values.SelectMany(w => w.Keys))).Distinct();
            }

            public IEnumerator<Cube> GetEnumerator()
            {
                foreach(var y in cubes.Values) {
                    foreach(var z in y.Values) {
                        foreach(var w in z.Values) {
                            foreach(var cube in w.Values) {
                                yield return cube;
                            }
                        }
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public Cubes InitialSetOfCubes = new Cubes();

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day17.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            var rows = s.Split("\n");
            for(var i = 0; i < rows.Count(); i++) {
                for (var y = 0; y < rows[i].Length; y++) {
                    InitialSetOfCubes[y,i,0,0] = rows[i][y] == '#' ? 
                        new Cube{ State = CubeState.Active} : 
                        new Cube{ State = CubeState.Inactive };
                }
            }
        }


        private void Task1(int cycles)
        {
            var oldCubes = this.InitialSetOfCubes;
            foreach(var cycle in Enumerable.Range(0, cycles)) {
                var newCubes = new Cubes();
                var firstX = oldCubes.GetXIndexes().Min() - 1;
                var lastX = oldCubes.GetXIndexes().Max() + 1;
                var firstY = oldCubes.GetYIndexes().Min() - 1;
                var lastY = oldCubes.GetYIndexes().Max() + 1;
                var firstZ = oldCubes.GetZIndexes().Min() -1;
                var lastZ = oldCubes.GetZIndexes().Max() + 1;
                for(var x = firstX; x <= lastX; x++) {
                    for(var y = firstY; y <= lastY; y++ ) {
                        for(var z = firstZ; z <= lastZ; z++) {
                            var surroundingCubes = new List<Cube>();
                            for(var xx = x - 1; xx <= x + 1; xx++) {
                                for(var yy = y - 1; yy <= y + 1; yy++) {
                                    for(var zz = z - 1; zz <= z + 1; zz++) {
                                        if (xx == x & yy == y && zz == z) {
                                            continue;
                                        }
                                        surroundingCubes.Add(oldCubes[xx, yy, zz]);
                                    }
                                }
                            }

                            if (oldCubes[x, y, z].State == CubeState.Inactive && surroundingCubes.Where(cube => cube.State == CubeState.Active).Count() == 3) {
                                newCubes[x, y, z] = new Cube{ State = CubeState.Active};
                            }
                            else if(oldCubes[x, y, z].State == CubeState.Active) {
                                if (Enumerable.Range(2, 2).Contains(surroundingCubes.Count(cube => cube.State == CubeState.Active))){
                                    newCubes[x, y, z] = new Cube{ State = CubeState.Active };
                                }
                                else {
                                    newCubes[x, y, z] = new Cube{ State = CubeState.Inactive };
                                }
                            }
                        }
                    }
                };

                oldCubes = newCubes;
            }

            Console.WriteLine(oldCubes.Count(x => x.State == CubeState.Active));
        }

        private void Task2(int cycles)
        {
            var oldCubes = this.InitialSetOfCubes;
            foreach(var cycle in Enumerable.Range(0, cycles)) {
                var newCubes = new Cubes();
                var firstX = oldCubes.GetXIndexes().Min() - 1;
                var lastX = oldCubes.GetXIndexes().Max() + 1;
                var firstY = oldCubes.GetYIndexes().Min() - 1;
                var lastY = oldCubes.GetYIndexes().Max() + 1;
                var firstZ = oldCubes.GetZIndexes().Min() -1;
                var lastZ = oldCubes.GetZIndexes().Max() + 1;
                var firstW = oldCubes.GetWIndexes().Min() -1;
                var lastW = oldCubes.GetWIndexes().Max() + 1;
                for(var x = firstX; x <= lastX; x++) {
                    for(var y = firstY; y <= lastY; y++ ) {
                        for(var z = firstZ; z <= lastZ; z++) {
                            for(var w = firstW; w <= lastW; w++) {
                            var surroundingCubes = new List<Cube>();
                                for(var xx = x - 1; xx <= x + 1; xx++) {
                                    for(var yy = y - 1; yy <= y + 1; yy++) {
                                        for(var zz = z - 1; zz <= z + 1; zz++) {
                                            for(var ww = w - 1; ww <= w + 1; ww++) {
                                                if (xx == x & yy == y && zz == z && ww == w) {
                                                    continue;
                                                }
                                                surroundingCubes.Add(oldCubes[xx, yy, zz, ww]);
                                            }
                                        }
                                    }
                                }

                                if (oldCubes[x, y, z, w].State == CubeState.Inactive && surroundingCubes.Where(cube => cube.State == CubeState.Active).Count() == 3) {
                                    newCubes[x, y, z, w] = new Cube{ State = CubeState.Active};
                                }
                                else if(oldCubes[x, y, z, w].State == CubeState.Active) {
                                    if (Enumerable.Range(2, 2).Contains(surroundingCubes.Count(cube => cube.State == CubeState.Active))){
                                        newCubes[x, y, z, w] = new Cube{ State = CubeState.Active };
                                    }
                                    else {
                                        newCubes[x, y, z, w] = new Cube{ State = CubeState.Inactive };
                                    }
                                }
                            }
                        }
                    }
                };

                oldCubes = newCubes;
            }

            Console.WriteLine(oldCubes.Count(x => x.State == CubeState.Active));
        }

        public void Run()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var s = this.Init();
//             s = @".#.
// ..#
// ###".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1(6);
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2(6);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);
        }
    }
}