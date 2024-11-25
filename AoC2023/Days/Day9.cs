using System.Collections.Generic;
using System.Linq;
using AoC.LegacyBase;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day9 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day9/example1.txt")
                    .Part(1).Correct(114)
                    .Part(2).Correct(2)
                .Test("output", "./Inputs/Day9/output.txt")
                    .Part(1).Correct(1666172641)
                    .Part(2).Correct(933);
        }

        public override string Part1(TestState input)
        {
            var result = 0;
            foreach(var line in input.GetLines())
            result += Extrapolate(line.Split(" ").Select(int.Parse).ToList());
            return result.ToString();
        }

        public override string Part2(TestState input)
        {
            var result = 0;
            foreach (var line in input.GetLines())
                result += ExtrapolateBackwards(line.Split(" ").Select(int.Parse).ToList());
            return result.ToString();
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
