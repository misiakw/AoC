using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day8 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day8/example1.txt")
                .Part1(2);
            builder.New("example2", "./Inputs/Day8/example2.txt")
                .Part1(6);
            builder.New("example part 2", "./Inputs/Day8/example1part2.txt")
                .Part2(6);
            builder.New("output", "./Inputs/Day8/output.txt")
                .Part1(22199)
                .Part2(13334102464297);
        }

        public override long Part1(IComparableInput<long> input)
        {
            var lines = ReadLines(input).ToArray();
            var turns = new RollArray<char>(lines[0].ToArray());

            var steps = new Dictionary<string, (string, string)>();
            var regex = new Regex(@"([A-Z]+) = \(([A-Z]+), ([A-Z]+)\)");
            foreach(var line in lines.Skip(2))
            {
                var matches = regex.Match(line).Groups;
                steps.Add(matches[1].Value, (matches[2].Value, matches[3].Value));
            }

            var now = "AAA";
            var ctr = 0;
            while (now != "ZZZ")
            {
                now = turns.Pick() == 'L'
                    ? steps[now].Item1
                    : steps[now].Item2;
                ctr++;
            }

            return ctr;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var lines = ReadLines(input).ToArray();
            var turns = new RollArray<char>(lines[0].ToArray());

            var steps = new Dictionary<string, (string, string)>();
            var regex = new Regex(@"([A-Z0-9]+) = \(([A-Z0-9]+), ([A-Z0-9]+)\)");
            foreach (var line in lines.Skip(2))
            {
                var matches = regex.Match(line).Groups;
                steps.Add(matches[1].Value, (matches[2].Value, matches[3].Value));
            }

            var nows = steps.Keys.Where(k => k.EndsWith('A')).ToArray();
            var ctr = 0L;
                                        //start, end, length
            var states = nows.Select(n => (-1L, -1L, 0L)).ToArray();
            //get loops
            while (states.Any(s => s.Item1 < 0 || s.Item2 < 0))
            {
                var turn = turns.Pick();
                for (var i = 0; i < nows.Count(); i++)
                {
                    nows[i] = turn == 'L'
                        ? steps[nows[i]].Item1
                        : steps[nows[i]].Item2;
                    if (nows[i].EndsWith('Z'))
                        if (states[i].Item1 < 0)
                            states[i] = (ctr + 1, -1, 0);
                        else if (states[i].Item2 < 0)
                            states[i] = (states[i].Item1, ctr + 1, ctr+1 - states[i].Item1);
                }

                ctr++;
            }

            var max = states.OrderByDescending(s => s.Item2).First();
            var rest = states.Where(s => s != max).ToList();
            while (rest.Any(r => (max.Item2 - r.Item1) % r.Item3 != 0))
                max.Item2 += max.Item3;

            return max.Item2;
        }   
    }
}
