using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;

namespace AoC2023.Days
{
    public class Day19 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day19/example1.txt")
                .Part1(19114)
                .Part2(167409079868000);
            builder.New("output", "./Inputs/Day19/output.txt")
                .Part1(376008);
                //.Part2(0);
        }

        public override long Part1(IComparableInput<long> input)
        {
            Rule.Rules = new Dictionary<string, Rule>();
            var lines = ReadLines(input);
            var i = 0;
            while (!string.IsNullOrEmpty(lines[i]))
            {
                var parts = lines[i].Split("{");
                new Rule(parts[0], new string(parts[1].SkipLast(1).ToArray()));
                i++;
            }
            i++;

            var ctr = 0l;
            for(; i<lines.Count; i++)
            {
                var part = new Part(lines[i]);
                if (Rule.Rules["in"].Process(part))
                    ctr += part.Score;
            }

            return ctr;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var ranges = Rule.Rules["in"].AcceptRanges(FourRange.Part2Full);
            var span = 0l;
            foreach(var range in ranges)
            {
                span += range.Size;
                Console.WriteLine($"size: {range.Size} ||| {range}");
            }
            return span;
        }

        private class Rule
        {
            public string Name;
            public (FourRange, string, string)[] Conditions;
            public static IDictionary<string, Rule> Rules = new Dictionary<string, Rule>();

            public Rule(string name, string pattern){
                this.Name = name;
                var newCond = new List<(FourRange, string, string)>();
                var currRange = FourRange.Full;
                var OldConditions = pattern.Split(",")
                    .Select(c => c.Split(":"))
                    .Select(c => c.Length == 1
                        ? (null, c[0])
                        : (c[0], c[1]))
                    .ToArray();

                foreach (var tup in OldConditions)
                    if(tup.Item1 != null)
                    {
                        var split = currRange.Split(tup.Item1);
                        newCond.Add((split.Item1, tup.Item2, tup.Item1));
                        currRange = split.Item2;
                    }
                    else
                        newCond.Add((currRange, tup.Item2, tup.Item1));

                Conditions = newCond.ToArray();
                Rules.Add(this.Name, this);
            }

            public bool Process(Part part)
            {
               foreach(var cond in Conditions)
                {
                    if (cond.Item1.Contains(part))
                        if (cond.Item2 == "A") return true;
                        else if (cond.Item2 == "R") return false;
                        else return Rules[cond.Item2].Process(part);
                }
                throw new Exception("should not get here...");
                return false;
            }


            public FourRange[] AcceptRanges(FourRange range){
                var result = new List<FourRange>();
                foreach(var cond in Conditions){
                    FourRange current;
                    if(cond.Item3 != null){
                        var split = range.Split(cond.Item3);
                        current = split.Item1;
                        range = split.Item2;
                    }else
                        current = range;

                    if(cond.Item2 == "A")
                        result.Add(current);
                    else if(cond.Item2 == "R")
                        continue;
                    else{
                        foreach(var path in Rules[cond.Item2].AcceptRanges(current))
                            result.Add(path);
                    }
                }
                return result.ToArray();
            }
        }

        private class FourRange{
            public IDictionary<char, (long, long)> Edges = new Dictionary<char, (long, long)>();
            public (FourRange, FourRange) Split(string pattern){
                var ch = pattern[0];
                var isLower = pattern[1] == '<';
                var num = long.Parse(pattern.Substring(2));

                var onTrue = new FourRange(){
                    Edges = Edges.Where(kv => kv.Key != ch).ToDictionary(kv => kv.Key, kv => kv.Value)
                };
                var onFalse = new FourRange(){
                    Edges = Edges.Where(kv => kv.Key != ch).ToDictionary(kv => kv.Key, kv => kv.Value)
                };

                if(isLower){
                    onTrue.Edges.Add(ch, (Edges[ch].Item1, num-1));
                    onFalse.Edges.Add(ch, (num, Edges[ch].Item2));
                }else{
                    onTrue.Edges.Add(ch, (num+1, Edges[ch].Item2));
                    onFalse.Edges.Add(ch, (Edges[ch].Item1, num));
                }

                return (onTrue, onFalse);
            }

            public bool Contains(Part p){
                foreach(var k in p.Xmas.Keys)
                    if (p.Xmas[k] < Edges[k].Item1 || p.Xmas[k] > Edges[k].Item2)
                        return false;
                return true;
            }

            public static FourRange Part2Full = new FourRange(){
                Edges = new Dictionary<char, (long, long)>(){
                    {'x', (-4000, 4000)},
                    {'m', (-4000, 4000)},
                    {'a', (-4000, 4000)},
                    {'s', (-4000, 4000)}
                }
            }; 
            public static FourRange Full = new FourRange(){
                Edges = new Dictionary<char, (long, long)>(){
                    {'x', (long.MinValue, long.MaxValue)},
                    {'m', (long.MinValue, long.MaxValue)},
                    {'a', (long.MinValue, long.MaxValue)},
                    {'s', (long.MinValue, long.MaxValue)}
                }
            };

            public long Size
            {
                get
                {
                    var result = 1l;
                    foreach(var scope in Edges.Values)
                        result *= (scope.Item2 - scope.Item1);
                    return result;
                }
            }

            public override string ToString()
                => string.Join(",",Edges.Select(kv => $"{kv.Key}({kv.Value.Item1},{kv.Value.Item2})")
                    .ToList());
        }

        private struct Part
        {
            public IDictionary<char, long> Xmas;
            public long Score => Xmas.Values.Sum();
            public Part(long x, long m, long a, long s)
            {
                Xmas = new Dictionary<char, long>()
                {
                    {'x', x },
                    {'m', m },
                    {'a', a },
                    {'s', s },
                };
            }
            public Part(string input)
            {
                var reg = new Regex(@"{x=(\d*),m=(\d*),a=(\d*),s=(\d*)}");
                var matches = reg.Matches(input);
                Xmas = new Dictionary<char, long>()
                {
                    {'x', long.Parse(matches[0].Groups[1].Value) },
                    {'m', long.Parse(matches[0].Groups[2].Value) },
                    {'a', long.Parse(matches[0].Groups[3].Value) },
                    {'s', long.Parse(matches[0].Groups[4].Value) },
                };
            }
        }
    }
}
