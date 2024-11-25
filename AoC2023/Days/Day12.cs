using System;
using System.Collections.Generic;
using System.Linq;
using AoC.LegacyBase;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day12 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day12/example1.txt")
                    .Part(1).Correct(21)
            //.Part(2).Correct(82000210);
                .Test("output", "./Inputs/Day12/output.txt")
                    .Part(1).Correct(6935);
            //.Part(2).Correct(790194712336);
        }
        public override string Part1(TestState input)
        {
            var result = 0;
            foreach(var line in input.GetLines())
            {
                var parts = line.Split(" ");
                foreach (var perm in PermuteOptions(parts[0]))
                {
                    var describe = Describe(perm);
                    if ($"{parts[1]},".Equals(describe))
                        result++;
                }
            }
            return result.ToString();
        }

        public override string Part2(TestState test)
        {
            throw new NotImplementedException();
        }

        private string Describe(string input)
        {
            var result = string.Empty;
            var ctr = 0;
            foreach(var c in input)
            {
                if (c == '#')
                    ctr++;
                if(c == '.' && ctr != 0)
                {
                    result += $"{ctr},";
                    ctr = 0;
                }
            }
            if(ctr > 0)
                result += $"{ctr},";
            return result;
        }

        private IList<string> PermuteOptions(string input)
        {
            var result = new List<string>();
            if (!input.Contains('?'))
            {
                result.Add(input);
                return result;
            }

            var index = input.IndexOf('?');
            var firstSet = input.Substring(0, index);
            var rest = input.Substring(index+1);
            foreach (var tail in PermuteOptions(rest))
            {
                result.Add(firstSet + '.' + tail);
                result.Add(firstSet + '#' + tail);
            }
            return result;
        }


        public struct SpringPos
        {
            public string Text;
            public IList<int> Numbers;
            
            public SpringPos(string line)
            {
                var parts = line.Split(' ');
                Text = parts[0];
                Numbers = parts[1].Split(',').Select(int.Parse).ToList();
            }
        }
    }
}
