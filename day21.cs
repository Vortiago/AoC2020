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
    public class Day21
    {
        public class Ingredient {
            public string Name { get; set; }
            public int TimesMentioned { get; set; } = 0;
            public string Allergen {get; set;}

            public bool ContainsAllergen => !String.IsNullOrEmpty(Allergen);
        }

        public class Allergene {
            public string Name { get; set; }
            public int TimesMentioned { get; set; } = 0;
            public string Ingredient {get; set;}
            public Dictionary<string, int> PossibleIngredient { get; } = new Dictionary<string, int>();
        }

        public Dictionary<string, Ingredient> Ingredients { get; } = new Dictionary<string, Ingredient>();
        
        public Dictionary<string, Allergene> Allergenes { get; } = new Dictionary<string, Allergene>();

        private string Init()
        {
            string data;
            using(var sr = new StreamReader("input_day21.txt")) {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private void ParseTodaysInput(string s) {
            foreach(var line in s.Split("\n", StringSplitOptions.RemoveEmptyEntries)) {
                var comp = line.Split("(contains", StringSplitOptions.RemoveEmptyEntries);
                foreach(var ingredient in comp[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)) {
                    Ingredients.TryGetValue(ingredient, out var ingredient1);
                    ingredient1 = ingredient1 ?? new Ingredient(){ Name = ingredient};
                    ingredient1.TimesMentioned += 1;

                    if (!Ingredients.TryAdd(ingredient, ingredient1)) {
                        Ingredients[ingredient] = ingredient1;
                    }
                }


                foreach(var allergene in comp[1].Replace(")", "").Split(",", StringSplitOptions.RemoveEmptyEntries)) {
                    Allergenes.TryGetValue(allergene, out var alle);
                    alle = alle ?? new Allergene{ Name = allergene };
                    alle.TimesMentioned += 1;
                    foreach(var ingredient in comp[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)) {
                        alle.PossibleIngredient.TryGetValue(ingredient, out var ingr);
                        ingr += 1;
                        if (!alle.PossibleIngredient.TryAdd(ingredient, ingr)) {
                            alle.PossibleIngredient[ingredient] = ingr;
                        }
                    }

                    if (!Allergenes.TryAdd(allergene, alle)) {
                        Allergenes[allergene] = alle;
                    }
                }
            }
        }

        private void Task1()
        {
            foreach(var allergen in Allergenes.Values.OrderByDescending(x => x.PossibleIngredient.Max(y => y.Value))) {
                var maxMentions = allergen.PossibleIngredient.Where(x => !Ingredients[x.Key].ContainsAllergen).Max(x => x.Value);
                
                foreach(var ingredient in allergen.PossibleIngredient.Where(x => !Ingredients[x.Key].ContainsAllergen && x.Value == maxMentions).Select(x => x.Key)){
                    if (Ingredients[ingredient].ContainsAllergen) continue;
                    Ingredients[ingredient].Allergen = allergen.Name;
                }
            }
            Console.WriteLine(Ingredients.Where(x => x.Value.ContainsAllergen == false).Sum(x => x.Value.TimesMentioned));
        }

        private void Task2()
        {
            Console.WriteLine(String.Join(",", Ingredients.Where(x => x.Value.ContainsAllergen).OrderBy(x => x.Value.Allergen).Select(x => x.Value.Name)));
        }

        public void Run()
        {
            var s = this.Init();
//             s = @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
// trh fvjkl sbzzf mxmxvkd (contains dairy)
// sqjhc fvjkl (contains soy)
// sqjhc mxmxvkd sbzzf (contains fish)".Replace("\r\n", "\n");
            this.ParseTodaysInput(s);
            Console.WriteLine("\nLooking for answer for Task 1.");
            this.Task1();
            Console.WriteLine("\nLooking for answer for Task 2.");
            this.Task2();
        }
    }
}