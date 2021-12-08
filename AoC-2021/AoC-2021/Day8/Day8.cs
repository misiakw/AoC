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
        public override string Part1()
        {
            var easy = 0L;
            foreach (var entry in this.Input)
            {
                easy += entry.Output.Where(o => o.Length == DIGITS[1].Length).Count();
                easy += entry.Output.Where(o => o.Length == DIGITS[7].Length).Count();
                easy += entry.Output.Where(o => o.Length == DIGITS[4].Length).Count();
                easy += entry.Output.Where(o => o.Length == DIGITS[8].Length).Count();
            }

            return easy.ToString();
        }

        [ExpectedResult(TestName = "Easy", Result = "5353")]
        [ExpectedResult(TestName = "Example", Result = "61229")]
        [ExpectedResult(TestName = "Input", Result = "1031553")]
        public override string Part2()
        {
            var permutates = Permutate("abcdefg");
            var sum = 0;

            foreach(var entry in this.Input)
            {
                var pattern  = entry.Match(permutates);
                var entryOutput = 0;
                foreach(var output in entry.Output)
                {
                    entryOutput = entryOutput * 10 + output.GetMatch(pattern).Value;
                }
                sum += entryOutput;
            }

            return sum.ToString();
        }


        public IList<string> Permutate(string input)
        {
            var result = new List<string>();

            if(input.Length == 1)
            {
                result.Add(input);
                return result;
            }
            
            foreach(var ch in input)
            {
                var tails = Permutate(input.Replace($"{ch}", ""));
                foreach(var tail in tails)
                {
                    result.Add($"{ch}{tail}");
                }
            }

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

            public string Match(IList<string> permutations)
            {
                foreach(var permutation in permutations)
                {
                    if (MatchAllPattern(permutation))
                        return permutation;
                    else
                        continue;
                }
                return "";
            }

            private bool MatchAllPattern(string permutation)
            {
                foreach (var pattern in Patterns)
                    if (!pattern.GetMatch(permutation).HasValue)
                        return false;
                return true;
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
