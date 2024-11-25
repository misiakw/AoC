using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.LegacyBase;
using AoC.Common;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day8 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day8/example1.txt")
                    .Part(1).Correct(2)
                .Test("example2", "./Inputs/Day8/example2.txt")
                    .Part(1).Correct(6)
                .Test("example part 2", "./Inputs/Day8/example1part2.txt")
                    .Part(2).Correct(6)
                .Test("output", "./Inputs/Day8/output.txt")
                    .Part(1).Correct(22199)
                    .Part(2).Correct(13334102464297);
        }

        public override string Part1(TestState input)
        {
            var lines = input.GetLines().ToArray();
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

            return ctr.ToString();
        }

        public override string Part2(TestState input)
        {
            var lines = input.GetLines().ToArray();
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
                for (var i = 0; i < nows.Length; i++)
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

            return max.Item2.ToString();
        }   
    }
}
