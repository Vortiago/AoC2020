using System.Net.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day4 {

        /* Fields
            byr (Birth Year) - four digits; at least 1920 and at most 2002.
            iyr (Issue Year) - four digits; at least 2010 and at most 2020.
            eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
            hgt (Height) - a number followed by either cm or in:
            If cm, the number must be at least 150 and at most 193.
            If in, the number must be at least 59 and at most 76.
            hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            pid (Passport ID) - a nine-digit number, including leading zeroes.
            cid (Country ID) - ignored, missing or not.
        */

        public class IDCard {
            public string byr { get; set; }
            public string iyr { get; set; }
            public string eyr { get; set; }
            public string hgt { get; set; }
            public string hcl { get; set; }
            public string ecl { get; set; }
            public string pid { get; set; }
            public string cid { get; set; }

            public bool IsValid() {
                return !String.IsNullOrEmpty(this.byr) &&
                        !String.IsNullOrEmpty(this.iyr) &&
                        !String.IsNullOrEmpty(this.eyr) &&
                        !String.IsNullOrEmpty(this.hgt) &&
                        !String.IsNullOrEmpty(this.hcl) &&
                        !String.IsNullOrEmpty(this.ecl) &&
                        !String.IsNullOrEmpty(this.pid);
            }

            public bool IsStrictValid() {
                return (!String.IsNullOrEmpty(this.byr) && Int32.Parse(this.byr) >= 1920 && Int32.Parse(this.byr) <= 2002) &&
                        (!String.IsNullOrEmpty(this.iyr) && Int32.Parse(this.iyr) >= 2010 && Int32.Parse(this.iyr) <= 2020) &&
                        (!String.IsNullOrEmpty(this.eyr) && Int32.Parse(this.eyr) >= 2020 && Int32.Parse(this.eyr) <= 2030) &&
                        (!String.IsNullOrEmpty(this.hgt) && Regex.IsMatch(this.hgt, @"^(1[5-8][0-9]cm|19[0-3]cm|59in|6[0-9]in|7[0-9]in)$")) &&
                        (!String.IsNullOrEmpty(this.hcl) && Regex.IsMatch(this.hcl, @"^#([a-f]|[0-9]){6}$")) &&
                        (!String.IsNullOrEmpty(this.ecl) && new List<string>{"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(this.ecl)) &&
                        (!String.IsNullOrEmpty(this.pid) && Regex.IsMatch(this.pid, @"^\d{9}$"));
            }
        }

        public List<IDCard> IDCards { get; } = new List<IDCard>();

        private void Init(string filename) {
            using(var sr = new StreamReader(filename)) {
                
                var data = sr.ReadToEnd()
                    .Split("\n\n")
                    .Where(text => text.Length > 0).ToList();
                
                this.IDCards.AddRange(data.Select(bulk => 
                        new IDCard{
                            byr = GetElement(bulk, "byr"),
                            iyr = GetElement(bulk, "iyr"),
                            eyr = GetElement(bulk, "eyr"),
                            hgt = GetElement(bulk, "hgt"),
                            hcl = GetElement(bulk, "hcl"),
                            ecl = GetElement(bulk, "ecl"),
                            pid = GetElement(bulk, "pid"),
                            cid = GetElement(bulk, "cid")
                        }));
            }
        }

        private static string GetElement(string bulk, string element) {
            return bulk.Split(' ', '\n').FirstOrDefault(x => x.StartsWith(element))?.Split(':')[1];
        }

        private void Task1() {
            Console.WriteLine(IDCards.Where(x => x.IsValid()).Count());
        }

        private void Task2() {
            Console.WriteLine(IDCards.Where(x => x.IsStrictValid()).Count());
        }

        public void Run(string filename) {
            this.Init(filename);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }   
}