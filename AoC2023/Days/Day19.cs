using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.LegacyBase;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day19 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day19/example1.txt")
                    .Part(1).Correct(19114)
                    .Part(2).Correct(167409079868000)
                .Test("output", "./Inputs/Day19/output.txt")
                    .Part(1).Correct(376008)
                    .Part(2).Correct(124078207789312);
        }

        public override string Part1(TestState input)
        {
            Rule.Rules = new Dictionary<string, Rule>();
            var lines = input.GetLines().ToArray();
            var i = 0;
            while (!string.IsNullOrEmpty(lines[i]))
            {
                var parts = lines[i].Split("{");
                new Rule(parts[0], new string(parts[1].SkipLast(1).ToArray()));
                i++;
            }
            i++;

            var ctr = 0l;
            for(; i<lines.Count(); i++)
            {
                var part = new Part(lines[i]);
                if (Rule.Rules["in"].Process(part))
                    ctr += part.Score;
            }

            return ctr.ToString();
        }

        public override string Part2(TestState input)
        {
            var ranges = Rule.Rules["in"].AcceptRanges(FourRange.Part2Full);

            var span = 0l;
            foreach(var range in ranges)
            {
                span += range.Size;
            }
            return span.ToString();
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

                var onTrue = new FourRange();
                var onFalse = new FourRange();

                foreach (var kv in Edges)
                {
                    if(kv.Key != ch)
                    {
                        onTrue.Edges.Add(kv.Key, Edges[kv.Key]);
                        onFalse.Edges.Add(kv.Key, Edges[kv.Key]);
                    }
                    else
                    {
                        if (isLower)
                        {
                            onTrue.Edges.Add(ch, (Edges[ch].Item1, num - 1));
                            onFalse.Edges.Add(ch, (num, Edges[ch].Item2));
                        }
                        else
                        {
                            onTrue.Edges.Add(ch, (num + 1, Edges[ch].Item2));
                            onFalse.Edges.Add(ch, (Edges[ch].Item1, num));
                        }
                    }
                }

                return (onTrue, onFalse);
            }

            public bool Overlap(FourRange other)
            {
                foreach (var k in Edges.Keys)
                {
                    var left = Edges[k].Item1 < other.Edges[k].Item1
                        ? Edges[k] : other.Edges[k];
                    var right = left == Edges[k] ? other.Edges[k] : Edges[k];
                    if (left.Item2 >= right.Item1)
                        return false;
                }
                return true;
            }

            public bool Contains(Part p){
                foreach(var k in p.Xmas.Keys)
                    if (p.Xmas[k] < Edges[k].Item1 || p.Xmas[k] > Edges[k].Item2)
                        return false;
                return true;
            }

            public static FourRange Part2Full = new FourRange(){
                Edges = new Dictionary<char, (long, long)>(){
                    {'x', (1, 4000)},
                    {'m', (1, 4000)},
                    {'a', (1, 4000)},
                    {'s', (1, 4000)}
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
                        result *= (scope.Item2 - scope.Item1)+1;
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
