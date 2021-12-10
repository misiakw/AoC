using AoC_2021.Attributes;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day8
{

    [BasePath("Day8")]
    [TestFile(File = "Easy.txt", Name = "Easy")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day8 : ParseableDay<Day8.Entry>
    {
        public static IDictionary<int, string> DIGITS = new Dictionary<int, string>
        {
            {0, "ABCEFG"}, {1, "CF"}, {2, "ACDEG"}, {3, "ACDFG"}, {4, "BCDF"},
            {5, "ABDFG"}, {6, "ABDEFG"}, {7, "ACF"}, {8, "ABCDEFG"}, {9, "ABCDFG"}
        };

        public static IDictionary<int, int[]> BINARIZED = new Dictionary<int, int[]>();

        public Day8(string path) : base(path) {
            foreach (var kv in DIGITS)
                if (!BINARIZED.ContainsKey(kv.Key))
                    BINARIZED.Add(kv.Key, kv.Value.Select(c => c - 'A').ToArray());
        }

        public override Entry Parse(string input)
        {
            var parts = input.Trim().Split(" | ");
            return new Entry(parts[0].Split(" "), parts[1].Split(" "));
        }

        [ExpectedResult(TestName = "Example", Result = "26")]
        [ExpectedResult(TestName = "Input", Result = "369")]
        public override string Part1(string testName)
        {
            var easy = 0L;
            foreach (var entry in this.Input)
            {
                easy += entry.Output.Where(o => o.Length == 2).Count(); // 1
                easy += entry.Output.Where(o => o.Length == 3).Count(); // 7
                easy += entry.Output.Where(o => o.Length == 4).Count(); // 4
                easy += entry.Output.Where(o => o.Length == 7).Count(); // 7
            }

            return easy.ToString();
        }

        [ExpectedResult(TestName = "Easy", Result = "5353")]
        [ExpectedResult(TestName = "Example", Result = "61229")]
        [ExpectedResult(TestName = "Input", Result = "1031553")]
        public override string Part2(string testName)
        {
            var sum = 0;
            foreach (var entry in Input)
            {
                var codes = entry.Patterns.Select(p => p.Contents).Distinct().ToList();
                var cf = codes.First(c => c.Length == 2);
                var acf = codes.First(c => c.Length == 3);
                var bcdf = codes.First(c => c.Length == 4);
                var bc = Remove(bcdf, cf);
                var adg = Common(codes.Where(c => c.Length == 5).ToArray());
                var bf = Remove(Common(codes.Where(c => c.Length == 6).ToArray()), adg);

                var A = Remove(acf, cf);
                var G = Remove(adg, A + bcdf);
                var D = Remove(adg, A + G);
                var B = Remove(bcdf, cf + D);
                var F = Remove(bf, B);
                var C = Remove(cf, F);
                var E = Remove(codes.First(c => c.Length == 7), A + B + C + D + F + G);

                var key = $"{A}{B}{C}{D}{E}{F}{G}";

                var entryOutput = 0;
                foreach (var output in entry.Output)
                {
                    entryOutput = entryOutput * 10 + output.GetMatch(key).Value;
                }
                sum += entryOutput;
            }

            return sum.ToString();
        }

        private string Remove(string input, string toRemove)
        {
            foreach (var ch in toRemove)
                input = input.Replace($"{ch}", "");
            return input;
        }

        private string Common(string[] group)
        {
            var result = group[0];
            foreach (var set in group)
                foreach (var ch in result)
                    if (!set.Contains(ch))
                        result = result.Replace($"{ch}", "");
            return result;
        }

        public class Entry
        {
            public readonly IList<Digit> Patterns;
            public readonly IList<Digit> Output;
            public Entry(string[] patterns, string[] output)
            {
                Patterns = patterns.Select(s => new Digit(s)).OrderBy(d => d.Potential.Count).ToList();
                Output = output.Select(s => new Digit(s)).ToList();
            }
        }

        public class Digit
        {
            private char[] contents;
            public readonly IList<int> Potential;
            public Digit(string signal)
            {
                contents = signal.Trim().OrderBy(c => c).ToArray();
                Potential = DIGITS.Where(kv => kv.Value.Length == contents.Length).Select(kv => kv.Key).ToList();
            }
            public long Length
            {
                get { return contents.Length; }
            }
            public string Contents
            {
                get { return string.Join("", contents); }
            }

            public int? GetMatch(string permutation)
            {
                foreach(var potential in Potential)
                {
                    var key = "";
                    foreach (var i in BINARIZED[potential])
                        key = $"{key}{permutation[i]}";
                    key = string.Join("", key.OrderBy(c => c).ToArray());
                    if (key.Equals(Contents))
                        return potential;
                }
                return null;
            }
        }
    }
}
