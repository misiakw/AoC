using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day6 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day6/example1.txt")
                .Part1(288)
                .Part2(71503);
            builder.New("output", "./Inputs/Day6/output.txt")
                .Part1(281600)
                .Part2(33875953);
        }

        public override long Part1(IComparableInput<long> input)
        {
            var races = ReadInput(input);

            var result = 1L;
            foreach(var race in races)
                result *= CalculateRace(race.Item1, race.Item2);

            return result;
        }

        private IEnumerable<(long, long)> ReadInput(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var times = lines[0].Split(" ")
                .Where(s => !string.IsNullOrEmpty(s))
                .Skip(1).Select(long.Parse).ToList();
            var distances = lines[1].Split(" ")
                .Where(s => !string.IsNullOrEmpty(s))
                .Skip(1).Select(long.Parse).ToList();

            for (var i = 0; i < times.Count; i++)
                yield return (times[i], distances[i]);
        }

        public override long Part2(IComparableInput<long> input)
        {
            var races = ReadInput(input);

            var time = long.Parse(string.Join("", races.Select(r => r.Item1)));
            var distance = long.Parse(string.Join("", races.Select(r => r.Item2)));

            return CalculateRace(time, distance);
        }

        private static long CalculateRace(long time, long distance)
        {
            for (var chargeTime = 0; chargeTime < time; chargeTime++)
                if ((time - chargeTime) * chargeTime > distance)
                    return time - 2 * chargeTime + 1;
            return 0;
        }
    }
}
