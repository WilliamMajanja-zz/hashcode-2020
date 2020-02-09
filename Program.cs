using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace hashcode_2020
{
    public static class Algorithms {
        public static List<KeyValuePair<int, int>> EndToStartWithMultiplesProbes(this int[] pizzas, int slicesLimit) {
            var result = new List<KeyValuePair<int, int>>();
            var watch = Stopwatch.StartNew();
            for (var i = pizzas.Length -1; i >= 0 && watch.ElapsedMilliseconds < 10000; i--) {
                var tempResult = new List<KeyValuePair<int, int>>();
                var totalSlices = 0;
                for(var j = i; pizzas.Length - j <= pizzas.Length && totalSlices < slicesLimit; j--) {
                    var pos = j < 0 ? pizzas.Length + j : j;
                    var pizza = pizzas[pos];
                    if (pizza + totalSlices <= slicesLimit) {
                        tempResult.Add(new KeyValuePair<int, int>(pos, pizza));
                        totalSlices += pizza;
                    }
                }
                if (tempResult.Score() > result.Score()) {
                    result = tempResult.ToList();
                }
            }
            watch.Stop();
            return result;
        }
        public static List<KeyValuePair<int, int>> MiddleToStart(this int[] pizzas, int slicesLimit) {
            var newPizzas = pizzas.Take((int)pizzas.Length/2).Reverse().Concat(pizzas.Skip((int)pizzas.Length/2).Reverse()).ToArray();
            var totalSlices = 0;
            var result = new List<KeyValuePair<int, int>>();
            for (var i = 0; i < pizzas.Length && totalSlices < slicesLimit; i++) {
                var pizza = newPizzas[i];
                if (pizza <= slicesLimit-totalSlices) {
                    if (i-(int)pizzas.Length/2 < 0) {
                        result.Add(new KeyValuePair<int, int>((int)pizzas.Length/2 + i, pizza));
                    } else {
                        result.Add(new KeyValuePair<int, int>(i, pizza));
                    }
                    totalSlices += pizza;
                }
            }
            return result;
        }
        public static List<KeyValuePair<int, int>> MiddleToEnd(this int[] pizzas, int slicesLimit) {
            var newPizzas = pizzas.Skip((int)pizzas.Length/2).Concat(pizzas.Take((int)pizzas.Length/2)).ToArray();
            var totalSlices = 0;
            var result = new List<KeyValuePair<int, int>>();
            for (var i = 0; i < pizzas.Length && totalSlices < slicesLimit; i++) {
                var pizza = newPizzas[i];
                if (pizza <= slicesLimit-totalSlices) {
                    if (i-(int)pizzas.Length/2 < 0) {
                        result.Add(new KeyValuePair<int, int>((int)pizzas.Length/2 + i, pizza));
                    } else {
                        result.Add(new KeyValuePair<int, int>(i, pizza));
                    }
                    totalSlices += pizza;
                }
            }
            return result;
        }
        public static List<KeyValuePair<int, int>> AlterningSidesStartFirst(this int[] pizzas, int slicesLimit) {
            var i = 0;
            var j = pizzas.Length -1;
            var result = new List<KeyValuePair<int, int>>();
            var totalSlices = 0;
            do {
                if (pizzas[i] + totalSlices <= slicesLimit) {
                    result.Add(new KeyValuePair<int, int>(i, pizzas[i]));
                    totalSlices += pizzas[i];
                }
                if (pizzas[j] + totalSlices <= slicesLimit) {
                    result.Add(new KeyValuePair<int, int>(j, pizzas[j]));
                    totalSlices += pizzas[j];
                }
                i++;
                j--;
            } while (i < j && totalSlices < slicesLimit);
            return result;
        }
        public static List<KeyValuePair<int, int>> AlterningSidesEndFirst(this int[] pizzas, int slicesLimit) {
            var i = 0;
            var j = pizzas.Length -1;
            var result = new List<KeyValuePair<int, int>>();
            var totalSlices = 0;
            do {
                if (pizzas[j] + totalSlices <= slicesLimit) {
                    result.Add(new KeyValuePair<int, int>(j, pizzas[j]));
                    totalSlices += pizzas[j];
                }
                if (pizzas[i] + totalSlices <= slicesLimit) {
                    result.Add(new KeyValuePair<int, int>(i, pizzas[i]));
                    totalSlices += pizzas[i];
                }
                i++;
                j--;
            } while (i < j && totalSlices < slicesLimit);
            return result;
        }
        public static List<KeyValuePair<int, int>> AlterningSidesSameTime(this int[] pizzas, int slicesLimit) {
            var i = 0;
            var j = pizzas.Length -1;
            var result = new List<KeyValuePair<int, int>>();
            var totalSlices = 0;
            do {
                if (pizzas[i] + pizzas[j] + totalSlices <= slicesLimit) {
                    result.Add(new KeyValuePair<int, int>(i, pizzas[i]));
                    result.Add(new KeyValuePair<int, int>(j, pizzas[j]));
                    totalSlices += pizzas[i] + pizzas[j];
                }
                i++;
                j--;
            } while (i < j && totalSlices < slicesLimit);
            return result;
        }
        public static List<KeyValuePair<int, int>> StartToEnd(this int[] pizzas, int slicesLimit) {
            var totalSlices = 0;
            var result = new List<KeyValuePair<int, int>>();
            for (var i = 0; i < pizzas.Length && totalSlices < slicesLimit; i++) {
                var pizza = pizzas[i];
                if (pizza <= slicesLimit-totalSlices) {
                    result.Add(new KeyValuePair<int, int>(i, pizza));
                    totalSlices += pizza;
                }
            }
            return result;
        }
        public static List<KeyValuePair<int, int>> EndToStart(this int[] pizzas, int slicesLimit) {
            var totalSlices = 0;
            var result = new List<KeyValuePair<int, int>>();
            for (var i = pizzas.Length -1; i >= 0 && totalSlices < slicesLimit; i--) {
                var pizza = pizzas[i];
                if (pizza <= slicesLimit-totalSlices) {
                    result.Add(new KeyValuePair<int, int>(i, pizza));
                    totalSlices += pizza;
                }
            }
            return result;
        }
        public static int Score(this List<KeyValuePair<int, int>> result) {
            var total = 0;
            result.ForEach(i => {
                total += i.Value;
            });
            return total;
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            var path = "./input/";
            DirectoryInfo d = new DirectoryInfo(path);
            foreach (var file in d.GetFiles("*.in"))
            {
                Console.WriteLine(file.Name);
                var input = System.IO.File.ReadAllLines(path + file.Name);
                var slicesLimit = int.Parse(input.First().Split(" ").First());
                var pizzas = input.Skip(1).Take(1).First().Split(" ").Select(i => int.Parse(i)).ToArray();

                var results = new Dictionary<string, List<KeyValuePair<int, int>>>();
                results.Add("Método 1", pizzas.EndToStart(slicesLimit));
                results.Add("Método 2", pizzas.StartToEnd(slicesLimit));
                results.Add("Método 3", pizzas.AlterningSidesSameTime(slicesLimit));
                results.Add("Método 4", pizzas.AlterningSidesEndFirst(slicesLimit));
                results.Add("Método 5", pizzas.AlterningSidesStartFirst(slicesLimit));
                results.Add("Método 6", pizzas.MiddleToEnd(slicesLimit));
                results.Add("Método 7", pizzas.MiddleToStart(slicesLimit));
                results.Add("Método 8", pizzas.EndToStartWithMultiplesProbes(slicesLimit));

                var ganador = results.OrderByDescending(i => i.Value.Score()).First();
                Console.WriteLine($"Límite: {slicesLimit}");
                Console.WriteLine($"Ganador {ganador.Key}, Puntuación: {ganador.Value.Score()}");
                String[] output = {
                    $"{ganador.Value.Count}",
                    string.Join(" ", ganador.Value.Select(i => i.Key).Reverse())
                };
                File.WriteAllLines("./output/" + file.Name, output);
            }
            
            // Console.WriteLine($"Pizzas:  {string.Join(" ", ganador.Value.Select(i => i.Key).Reverse())}");
            // Console.WriteLine(result.Count);
            // Console.WriteLine(string.Join(" ", result.Select(i => i.Key).Reverse()));
        }
    }
}
