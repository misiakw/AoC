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
                .Part1(19114);
                //.Part2(167409079868000);
            builder.New("output", "./Inputs/Day19/output.txt");
                //.Part1(376008);
                //.Part2(0);
        }

        public override long Part1(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var i = 0;
            while (!string.IsNullOrEmpty(lines[i]))
            {
                var parts = lines[i].Split("{");
                new Rule(parts[0], new string(parts[1].SkipLast(1).ToArray()));
                i++;
            }
            i++;

            var paths = Rule.Rules["in"].AcceptPaths(FourRange.Full);
            var ctr = 0;
            for(; i<lines.Count; i++)
            {
                var part = new Part(lines[i]);
                if(paths.Any(p => p.Contains(part)))
                    ctr++;
            }

            return ctr;
        }

        public override long Part2(IComparableInput<long> input)
        {
            return 0;
        }

        private class Rule{
            public string Name;
            public (string, string)[] Conditions;
            public static long Accepted = 0;
            public static IDictionary<string, Rule> Rules = new Dictionary<string, Rule>();

            public Rule(string name, string pattern){
                this.Name = name;
                Conditions = pattern.Split(",")
                    .Select(c => c.Split(":"))
                    .Select(c => c.Length == 1
                        ? (null, c[0])
                        : (c[0], c[1]))
                    .ToArray();
                Rules.Add(this.Name, this);
            }
            public FourRange[] AcceptPaths(FourRange range){
                var result = new List<FourRange>();
                foreach(var cond in Conditions){
                    FourRange current;
                    if(cond.Item1 != null){
                        var split = range.Split(cond.Item1);
                        current = split.Item1;
                        range = split.Item2;
                    }else
                        current = range;

                    if(cond.Item2 == "A")
                        result.Add(current);
                    else if(cond.Item2 == "R")
                        continue;
                    else{
                        foreach(var path in Rules[cond.Item2].AcceptPaths(current))
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
                if(p.X < Edges['x'].Item1 || p.X > Edges['x'].Item2)
                    return false;
                if(p.M < Edges['m'].Item1 || p.X > Edges['m'].Item2)
                    return false;
                if(p.A < Edges['a'].Item1 || p.X > Edges['a'].Item2)
                    return false;
                if(p.S < Edges['s'].Item1 || p.X > Edges['s'].Item2)
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


            public override string ToString()
                => string.Join(",",Edges.Select(kv => $"{kv.Key}({kv.Value.Item1},{kv.Value.Item2})")
                    .ToList());
        }

        /*private abstract class AbstractRule
        {
            public abstract string Pattern { get; }
            public readonly string Name;
            public abstract void Process(Part part);
            public AbstractRule(string name)
            {
                this.Name = name;
            }
            public override string ToString() => string.IsNullOrEmpty(Name) ? Pattern : Name;
        }
        private class RuleIf: AbstractRule
        {
            public override string Pattern => string.IsNullOrEmpty(Name)
                ? conditionPattern + ":" +
                (this.True != null ? this.True.Pattern : "[EMPTY]") + "," +
                (this.False != null ? this.False.Pattern : "[EMPTY]")
                : Name;
            protected string conditionPattern;
            protected AbstractRule True;
            protected AbstractRule False;
            protected Func<Part, long> GetValue;
            protected Func<long, long, bool> Condition;
            protected long Limit;

            public RuleIf(string name) : base(name) { }
            public RuleIf(string name, string pattern, IDictionary<string, AbstractRule> ruleDict): base(name)
            {
                FillRule(pattern, ruleDict);
            }
            public void FillRule(string pattern, IDictionary<string, AbstractRule> ruleDict)
            {
                var parts = pattern.Split(":", 2);
                this.Limit = long.Parse(parts[0].Substring(2));
                conditionPattern = parts[0];
                GetValue = pattern[0] switch
                {
                    'x' => new Func<Part, long>(p => p.X),
                    'm' => new Func<Part, long>(p => p.M),
                    'a' => new Func<Part, long>(p => p.A),
                    's' => new Func<Part, long>(p => p.S)
                };
                Condition = pattern[1] switch
                {
                    '>' => new Func<long, long, bool>((a, b) => a > b),
                    '<' => new Func<long, long, bool>((a, b) => a < b),
                };

                var merged = string.Empty;
                var toProcess = parts[1];
                for(var i =0; i < toProcess.Length; i++)
                {
                    if (toProcess[i] == ',')
                    {//finished true rule
                        if (!ruleDict.ContainsKey(merged))
                            ruleDict.Add(merged, new RuleIf(merged));
                        this.True = ruleDict[merged];
                        toProcess = toProcess.Substring(merged.Length+1);
                        i = -1;
                        merged = string.Empty;
                    }
                    else if (toProcess[i] == ':')
                    {
                        var newIf = new RuleIf("", toProcess, ruleDict);
                        if (this.True == null)
                            this.True = newIf;
                        else
                            this.False = newIf;
                        toProcess = toProcess.Substring(newIf.Pattern.Length);
                        i = -1;
                        merged = string.Empty;
                        //we have ifRule as param
                    }
                    else
                        merged += toProcess[i];
                }
                if (!string.IsNullOrEmpty(merged)) {
                    if (!ruleDict.ContainsKey(merged))
                        ruleDict.Add(merged, new RuleIf(merged));
                    this.False = ruleDict[merged];
                    merged = string.Empty;
                }
            }

            public override void Process(Part part)
            {
                if(this.Condition(this.GetValue(part), Limit))
                    this.True.Process(part);
               else
                    this.False.Process(part);
            }
        }
        private class RuleCount: AbstractRule
        {
            public RuleCount(string name) : base(name) { }
            public long Ctr = 0;
            public IList<Part> Collection = new List<Part>();
            public override string Pattern => Name;

            public override void Process(Part part)
            {
                Ctr++;
                Collection.Add(part);
            }
        }*/

        private struct Part
        {
            public long X, M, A, S;
            public Part(long x, long m, long a, long s)
            {
                X = x;
                M = m;
                A = a;
                S = s;
            }
            public Part(string input)
            {
                var reg = new Regex(@"{x=(\d*),m=(\d*),a=(\d*),s=(\d*)}");
                var matches = reg.Matches(input);
                X = long.Parse(matches[0].Groups[1].Value);
                M = long.Parse(matches[0].Groups[2].Value);
                A = long.Parse(matches[0].Groups[3].Value);
                S = long.Parse(matches[0].Groups[4].Value);
            }
        }
    }
}
