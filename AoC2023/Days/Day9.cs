using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day9 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day9/example1.txt")
               .Part1(114)
               .Part2(2);
            builder.New("output", "./Inputs/Day9/output.txt")
                .Part1(1666172641)
                .Part2(933);
        }

        public override long Part1(IComparableInput<long> input)
        {
            var result = 0;
            foreach(var line in ReadLines(input))
            result += Extrapolate(line.Split(" ").Select(int.Parse).ToList());
            return result;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var result = 0;
            foreach (var line in ReadLines(input))
                result += ExtrapolateBackwards(line.Split(" ").Select(int.Parse).ToList());
            return result;
        }

        private int Extrapolate(IList<int> states){
            if(states.All(s => s == 0))
                return 0;
            var newStates = new List<int>();
            var prev = states.First();
            foreach(var curr in states.Skip(1)){
                newStates.Add(curr - prev);
                prev = curr;
            }
            return prev + Extrapolate(newStates);
        }

        private int ExtrapolateBackwards(IList<int> states){
            if(states.All(s => s == 0))
                return 0;
            var newStates = new List<int>();
            var prev = states.First();
            foreach(var curr in states.Skip(1)){
                newStates.Add(curr - prev);
                prev = curr;
            }
            var first = states.First();
            return first - ExtrapolateBackwards(newStates);
        }
    }
}
