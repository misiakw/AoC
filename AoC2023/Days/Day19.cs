using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using AoC.Common.Maps;

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
            var dict = new Dictionary<string, AbstractRule>
            {
                {"A", new RuleCount("A") }, {"R", new RuleCount("R")}
            };

            var lines = ReadLines(input);
            var i = 0;
            while (!string.IsNullOrEmpty(lines[i]))
            {
                var parts = lines[i].Split("{");
                if(!dict.ContainsKey(parts[0]))
                    dict.Add(parts[0], new RuleIf(parts[0], parts[1].Substring(0, parts[1].Length-1), dict));
                else if (dict[parts[0]] is RuleIf){
                    var ruleif = dict[parts[0]] as RuleIf;
                    ruleif.FillRule(parts[1].Substring(0, parts[1].Length - 1), dict);
                }
                i++;
            }
            i++;
            var start = dict["in"];
            for(; i<lines.Count; i++)
            {
                var part = new Part(lines[i]);
                start.Process(part);
            }

            return ((RuleCount)dict["A"]).Collection.Sum(p => p.X + p.M + p.A + p.S);
        }

        public override long Part2(IComparableInput<long> input)
        {
            return 0;
        }

        private abstract class AbstractRule
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
        }

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
