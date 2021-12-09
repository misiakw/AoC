using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC_2021;
using AoC_2021.Attributes;

namespace AoC_2021.Day7
{
    [BasePath("Day7")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day7 : DayBase
    {
        private IReadOnlyDictionary<long, int> Crabs;
        private long Min, Max;
        public Day7(string filePath) : base(filePath)
        {
            this.Crabs = this.RawInput.Trim().Split(",")
                .Select(s => long.Parse(s)).GroupBy(l => l)
                .ToDictionary(k => k.Key, v => v.Count());
            this.Min = Crabs.Keys.Min();
            this.Max = Crabs.Keys.Max();
        }

        [ExpectedResult(TestName = "Example", Result = "37")]
        [ExpectedResult(TestName = "Input", Result = "343605")]
        public override string Part1(string testName)
        {
            var minFuel = long.MaxValue;
            for(var x=Min; x<=Max; x++)
            {
                var fuel = Crabs.Select(kv => GetFuelToPoint(kv, x)).Sum();
                if (fuel < minFuel) minFuel = fuel;
            }

            return minFuel.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "168")]
        [ExpectedResult(TestName = "Input", Result = "96744904")]
        public override string Part2(string testName)
        {
            var minFuel = long.MaxValue;
            for (var x = Min; x <= Max; x++)
            {
                var fuel = Crabs.Select(kv => GetExtendedFuelToPoint(kv, x)).Sum();
                if (fuel < minFuel) minFuel = fuel;
            }

            return minFuel.ToString();
        }

        private long GetFuelToPoint(KeyValuePair<long, int> crabSet, long dest)
        {
            return Math.Abs(dest - crabSet.Key) * crabSet.Value;
        }

        private long GetExtendedFuelToPoint(KeyValuePair<long, int> crabSet, long dest)
        {
            var n = Math.Abs(dest - crabSet.Key);
            return ((n * (n + 1)) / 2) * crabSet.Value;
        }
    }
}
