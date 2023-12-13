using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day12 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day12/example1.txt")
               .Part1(21);
            //.Part2(82000210);
            builder.New("output", "./Inputs/Day12/output.txt")
                .Part1(6935);
                //.Part2(790194712336);
        }
        public override long Part1(IComparableInput<long> input)
        {
            var result = 0;
            foreach(var line in ReadLines(input))
            {
                var parts = line.Split(" ");
                foreach (var perm in PermuteOptions(parts[0]))
                {
                    var describe = Describe(perm);
                    if ($"{parts[1]},".Equals(describe))
                        result++;
                }
            }
            return result;
        }

        public override long Part2(IComparableInput<long> input)
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
