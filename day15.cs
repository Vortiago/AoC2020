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
    public class Day15
    {
        public IEnumerable<int> YeetMeNext(int initial, int count) {
            var last = initial;
            for(var i = this.NumberHistory.Keys.Count; i < count; i++) {
                if (this.NumberHistory.ContainsKey(last) && this.NumberHistory[last].Count > 1){
                    last = this.NumberHistory[last].First() - this.NumberHistory[last].Skip(1).First();
                } else {
                    last = 0;
                }   
                this.AddOrUpdate(last, i);
                yield return last;
            }
        }

        private void AddOrUpdate(int number, int counter){
            if (!this.NumberHistory.ContainsKey(number)) {
                this.NumberHistory.Add(number, new List<int>() {counter});
            }
            else {
                this.NumberHistory[number].Insert(0, counter);
            }
        } 

        public Dictionary<int, List<int>> NumberHistory = new Dictionary<int, List<int>>();

        private void Init()
        {
        }

        private void ParseTodaysInput() {
        }

        private void Task1()
        {
            this.AddOrUpdate(0, 0);
            this.AddOrUpdate(3, 1);
            this.AddOrUpdate(6, 2);
            var numberCalled = 0;
            foreach(var value in YeetMeNext(6, 2020)) {
                numberCalled = value;
            }
            Console.WriteLine(numberCalled);
        }

        private void Task2()
        {
            this.NumberHistory.Clear();
            this.AddOrUpdate(1, 0);
            this.AddOrUpdate(0, 1);
            this.AddOrUpdate(15, 2);
            this.AddOrUpdate(2, 3);
            this.AddOrUpdate(10, 4);
            this.AddOrUpdate(13, 5);
            var numberCalled = 0;
            foreach(var value in YeetMeNext(13, 30000000)) {
                numberCalled = value;
            }
            Console.WriteLine(numberCalled);
        }

        public void Run()
        {
            this.ParseTodaysInput();
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}