using AoC_2021.Attributes;
using AoC_2021.Common;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AoC_2021.Day22.Cuboid;
using Range = AoC_2021.Day22.Cuboid.Range;

namespace AoC_2021.Day22
{
    [BasePath("Day22")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day22 : DayBase
    {
        public Day22(string path) : base(path) { }

        public Tuple<bool, Cuboid> Parse(string input) => Parse(input, long.MinValue, long.MaxValue);

        public Tuple<bool, Cuboid> Parse(string input, long minLimit, long maxLimit)
        {
            var parts = input.Trim().Split(" ");
            var state = parts[0].Equals("on");
            var ranges = parts[1].Split(',').Select(l => l.Trim().Substring(2).Split("..").Select(s => long.Parse(s)).ToArray()).ToArray();

            var x = new Range(ranges[0][0], ranges[0][1], Axis.X);
            var y = new Range(ranges[1][0], ranges[1][1], Axis.Y);
            var z = new Range(ranges[2][0], ranges[2][1], Axis.Z);
            return Tuple.Create(state, new Cuboid(x, y, z));
        }

        [ExpectedResult(TestName = "Example", Result = "474140")]
        [ExpectedResult(TestName = "Input", Result = "601104")]
        public override string Part1(string testName)
        {
            var turnedList = new List<Cuboid>();
            var Input = LineInput.Select(l => Parse(l, -50, 50)).ToList();

            foreach (var opeation in Input)
            {
                var newSet = new List<Cuboid>();
                foreach (var turnedOn in turnedList)
                {
                    if (opeation.Item1)
                        newSet.AddRange(turnedOn.Add(opeation.Item2));
                    else
                        newSet.AddRange(turnedOn.Substract(opeation.Item2));
                }
                turnedList = newSet;
            }

            return turnedList.Select(c => c.Volume).Sum().ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "2758514936282235")]
        //[ExpectedResult(TestName = "Input", Result = 571032)]
        public override string Part2(string testName)
        {
            var turnedList = new List<Cuboid>();
            var Input = LineInput.Select(l => Parse(l)).ToList();

            foreach (var opeation in Input)
            {
                var newSet = new List<Cuboid>();
                foreach (var turnedOn in turnedList)
                {
                    if (opeation.Item1)
                        newSet.AddRange(turnedOn.Add(opeation.Item2));
                    else
                        newSet.AddRange(turnedOn.Substract(opeation.Item2));
                }
                turnedList = newSet;
            }

            return turnedList.Select(c => c.Volume).Sum().ToString();
        }
    }
    public class Cuboid
    {
        public readonly IReadOnlyDictionary<Axis, Range> Ranges;
        public long Volume => Ranges.Values.Select(v => v.Span).Aggregate(1L, (x, y) => x * y);
        public Cuboid(Range X, Range Y, Range Z)
        {
            Ranges = new Dictionary<Axis, Range>() {
                {X.Axis, X}, {Y.Axis, Y}, {Z.Axis, Z}
            };
        }

        public IEnumerable<Cuboid> Add(Cuboid other)
        {
            yield return this;
            yield return other;
        }

        public IEnumerable<Cuboid> Substract(Cuboid other)
        {
            yield return this;
        }

        public Range GetRange(Axis axis) => Ranges[axis];

        /*public Cuboid Slice(Range range)
        {
            var rangeX = range.Axis == Axis.X ? range : GetRange(Axis.X);
            var rangeY = range.Axis == Axis.Y ? range : GetRange(Axis.Y);
            var rangeZ = range.Axis == Axis.Y ? range : GetRange(Axis.Z);
            return new Cuboid(rangeX, rangeY, rangeZ);
        }*/

        public enum Axis { X, Y, Z }
        public class Range
        {
            public readonly long Min, Max;
            public readonly Axis Axis;
            public Range(long min, long max, Axis axis)
            {
                Min = min;
                Max = max;
                Axis = axis;
            }
            public long Span => Max - Min;
        }
    }
}
