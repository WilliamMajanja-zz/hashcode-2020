using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hashcode_2020
{
    public static class Algorithms
    {
        public static List<KeyValuePair<int, int>> OptimisticSolution(this int[] pizzas, int slicesLimit)
        {
            var result = new List<KeyValuePair<int, int>>();
            for (var i = pizzas.Length - 1; i >= 0; i--)
            {
                var totalSlices = 0;
                var tempResult = new List<KeyValuePair<int, int>>();
                for (var j = i; j >= 0 && totalSlices < slicesLimit; j--)
                {
                    var pizza = pizzas[j];
                    if (pizza + totalSlices <= slicesLimit)
                    {
                        tempResult.Add(new KeyValuePair<int, int>(j, pizza));
                        totalSlices += pizza;
                    }
                }
                if (tempResult.Score() > result.Score())
                {
                    result = tempResult.ToList();
                }
                if (totalSlices == slicesLimit)
                {
                    break;
                }

            }
            return result;
        }
        public static ulong Score(this List<KeyValuePair<int, int>> result)
        {
            ulong total = 0;
            result.ForEach(i =>
            {
                total += (ulong)i.Value;
            });
            return total;
        }
        /// <summary>
        /// Returns a random long from min (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="min">The inclusive minimum bound</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        /// <summary>
        /// Returns a random long from 0 (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
        public static long NextLong(this Random random, long max)
        {
            return random.NextLong(0, max);
        }

        /// <summary>
        /// Returns a random long over all possible values of long (except long.MaxValue, similar to
        /// random.Next())
        /// </summary>
        /// <param name="random">The given random instance</param>
        public static long NextLong(this Random random)
        {
            return random.NextLong(long.MinValue, long.MaxValue);
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            long M = new Random(DateTime.Now.Millisecond).Next((int)Math.Pow(10, 8), (int)Math.Pow(10, 9));
            int N = new Random(DateTime.Now.Millisecond).Next((int)Math.Pow(10, 4), (int)Math.Pow(10, 5));
            var S = new long[N];
            var filename = $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.in";
            S[0] =new Random(DateTime.Now.Millisecond).NextLong(0, (int)M%N);
            var minValue = S[0];
            for (int i = 1; i < N; i++) {
                minValue = S[i-1];
                long maxValue = new Random(DateTime.Now.Millisecond).NextLong(Math.Min(S[i-1]+50, M-6), Math.Min(S[i-1]+100, M-5));
                var value = new Random(DateTime.Now.Millisecond).NextLong(minValue, maxValue);
                S[i]=new Random(DateTime.Now.Millisecond).NextLong(minValue, maxValue);
            }
            String[] output2 = {
                $"{M} {N}",
                string.Join(" ", S)
            };
            File.WriteAllLines($"./input/{filename}", output2);

            var path = "./input/";
            DirectoryInfo d = new DirectoryInfo(path);
            ulong totalPoints = 0;
            foreach (var file in d.GetFiles("*.in"))
            {
                Console.WriteLine(file.Name);
                var input = System.IO.File.ReadAllLines(path + file.Name);
                var slicesLimit = int.Parse(input.First().Split(" ").First());
                var pizzas = input.Skip(1).Take(1).First().Split(" ").Select(i => int.Parse(i)).ToArray();

                var results = pizzas.OptimisticSolution(slicesLimit);
                totalPoints += results.Score();
                Console.WriteLine($"Límite: {slicesLimit}, Puntuación: {results.Score()}");
                String[] output = {
                    $"{results.Count}",
                    string.Join(" ", results.Select(i => i.Key).Reverse())
                };
                File.WriteAllLines("./output/" + file.Name, output);
            }
            Console.WriteLine($"Total Puntos: {totalPoints}");
        }
    }
}
