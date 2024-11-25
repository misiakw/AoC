using System.Collections.Generic;
using System.Linq;
using AoC.LegacyBase;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day6 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day6/example1.txt")
                    .Part(1).Correct(288)
                    .Part(2).Correct(71503)
                .Test("output", "./Inputs/Day6/output.txt")
                    .Part(1).Correct(281600)
                    .Part(2).Correct(33875953);
        }

        public override string Part1(TestState input)
        {
            var races = ReadInput(input);

            var result = 1L;
            foreach(var race in races)
                result *= CalculateRace(race.Item1, race.Item2);

            return result.ToString();
        }

        private IEnumerable<(long, long)> ReadInput(TestState input)
        {
            var lines = input.GetLines().ToList();
            var times = lines[0].Split(" ")
                .Where(s => !string.IsNullOrEmpty(s))
                .Skip(1).Select(long.Parse).ToList();
            var distances = lines[1].Split(" ")
                .Where(s => !string.IsNullOrEmpty(s))
                .Skip(1).Select(long.Parse).ToList();

            for (var i = 0; i < times.Count; i++)
                yield return (times[i], distances[i]);
        }

        public override string Part2(TestState input)
        {
            var races = ReadInput(input);

            var time = long.Parse(string.Join("", races.Select(r => r.Item1)));
            var distance = long.Parse(string.Join("", races.Select(r => r.Item2)));

            return CalculateRace(time, distance).ToString();
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
