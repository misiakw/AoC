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
    [TestFile(File = "exampleSmall.txt", Name = "ExampleSmall", TestToProceed = TestCase.Part1)]
    [TestFile(File = "examplePart1.txt", Name = "Example", TestToProceed = TestCase.Part1)]
    [TestFile(File = "examplePart2.txt", Name = "Example", TestToProceed = TestCase.Part2)]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day22 : DayBase
    {
        public Day22(string path) : base(path) { }

        public long[] Parse(string input)
        {
            var result = new long[7];
            var parts = input.Trim().Split(" ");
            result[0] = parts[0].Equals("on") ? 1 : 0;
            var ranges = parts[1].Split(',').Select(l => l.Trim().Substring(2).Split("..").Select(s => long.Parse(s)).ToArray()).ToArray();

            result[1] = ranges[0][0];
            result[2] = ranges[0][1];
            result[3] = ranges[1][0];
            result[4] = ranges[1][1];
            result[5] = ranges[2][0];
            result[6] = ranges[2][1];
            return result;
        }

        public Tuple<bool, Cuboid> ToInput(long[] data) => Tuple.Create(data[0]==1, 
            new Cuboid(new Range(data[1], data[2], Axis.X), new Range(data[3], data[4], Axis.Y), new Range(data[5], data[6], Axis.Z)));

        [ExpectedResult(TestName = "ExampleSmall", Result = "39")]
        [ExpectedResult(TestName = "Example", Result = "590784")]
        [ExpectedResult(TestName = "Input", Result = "601104")]
        public override string Part1(string testName)
        {
            var turnedList = new List<Cuboid>();
            var tmpInput = LineInput.Select(l => Parse(l)).ToList();

            var Input = new List<Tuple<bool, Cuboid>>();
            foreach (var tmp in tmpInput)
            {
                var limited = new long[7];
                limited[0] = tmp[0];
                for (var i = 1; i < 7; i++)
                    limited[i] = tmp[i] < -50 ? -50
                        : tmp[i] > 50 ? 50 : tmp[i];
                Input.Add(ToInput(limited));
            }

            foreach (var inp in Input)
                Console.WriteLine($"{inp.Item1} {inp.Item2}");

            var step = 1;
            foreach (var action in Input)
            {
                var overlapping = turnedList.Where(c => c.Overlap(action.Item2)).ToList();
                if (action.Item1) //turn on
                {
                    if (!overlapping.Any())
                    { //no overlapping
                        turnedList.Add(action.Item2);
                    }
                    else
                    {
                        foreach (var overlap in overlapping)
                        {
                            turnedList.Remove(overlap);
                            turnedList.AddRange(overlap.Add(action.Item2));
                        }
                    }
                }
                else //substract
                {
                    overlapping = turnedList.ToList();
                    foreach (var overlap in overlapping)
                    {
                        turnedList.Remove(overlap);
                        turnedList.AddRange(overlap.Substract(action.Item2));
                    }
                }
                turnedList = turnedList.Distinct().ToList();
                Console.WriteLine($"Step  {step++}, turned on {turnedList.Select(c => c.Volume).Sum()}");
            }


            return turnedList.Select(c => c.Volume).Sum().ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "2758514936282235")]
        [ExpectedResult(TestName = "Input", Result = "", TooHigh = 24720527721166241)]
        public override string Part2(string testName)
        {
            var turnedList = new List<Cuboid>();
            var Input = LineInput.Select(l => ToInput(Parse(l))).ToList();

            foreach (var action in Input)
            {
                var overlapping = turnedList.Where(c => c.Overlap(action.Item2)).ToList();
                if (action.Item1) //turn on
                {
                    if (!overlapping.Any()) { //no overlapping
                        turnedList.Add(action.Item2);
                    }
                    else
                    {
                        foreach(var overlap in overlapping)
                        {
                            turnedList.Remove(overlap);
                            turnedList.AddRange(overlap.Add(action.Item2));
                        }
                    }
                }
                else //substract
                {
                    foreach(var overlap in overlapping)
                    {
                        turnedList.Remove(overlap);
                        turnedList.AddRange(overlap.Substract(action.Item2));
                    }
                }
                turnedList = turnedList.Distinct().ToList();
            }
            

            return turnedList.Select(c => c.Volume).Sum().ToString();
        }
    }
    public class Cuboid: IEquatable<Cuboid>
    {
        public readonly IDictionary<Axis, Range> Ranges;
        public long Volume => Ranges.Values.Select(v => v.Span).Aggregate(1L, (x, y) => x * y);
        public Cuboid(Range X, Range Y, Range Z)
        {
            Ranges = new Dictionary<Axis, Range>() {
                {X.Axis, X}, {Y.Axis, Y}, {Z.Axis, Z}
            };
        }
        public Range X => Ranges[Axis.X];
        public Range Y => Ranges[Axis.Y];
        public Range Z => Ranges[Axis.Z];

        public bool Overlap(Cuboid other) => Ranges.All(kv => kv.Value.Overlap(other.Ranges[kv.Key]));

        public IEnumerable<Cuboid> Add(Cuboid other)
        {
            var result = Substract(other).ToList();
            result.Add(other);
            return result;
        }

        public IEnumerable<Cuboid> Substract(Cuboid other)
        {
            var result = new List<Cuboid>();
            if (!Overlap(other))
                result.Add(this);
            else
            {
                result.AddRange(ChopRange(other.X));
                result.AddRange(ChopRange(other.Y));
                result.AddRange(ChopRange(other.Z));
            }
            return result;
        }

        public IEnumerable<Cuboid> ChopRange(Range range)
        {
            var compare = Ranges[range.Axis];
            if (compare.Min < range.Min)
            {
                var tmp = new Range(compare.Min, range.Min - 1, range.Axis);
                Ranges[range.Axis] = new Range(range.Min, compare.Max, range.Axis);
                yield return new Cuboid(range.Axis == Axis.X ? tmp : X,
                    range.Axis == Axis.Y ? tmp : Y,
                    range.Axis == Axis.Z ? tmp : Z);
            }
            if (compare.Max > range.Max)
            {
                var tmp = new Range(range.Max + 1, compare.Max, range.Axis);
                Ranges[range.Axis] = new Range(range.Min, range.Max, range.Axis);
                yield return new Cuboid(range.Axis == Axis.X ? tmp : X,
                    range.Axis == Axis.Y ? tmp : Y,
                    range.Axis == Axis.Z ? tmp : Z);
            }
        }
        public override bool Equals(object obj) =>
            obj is Cuboid cuboid &&
            X == cuboid.X &&
            Y == cuboid.Y &&
            Z == cuboid.Z;
        public override string ToString() => $"x={X},y={Y},z={Z}";

        public bool Equals(Cuboid other) => Equals((object)other);

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
            public long Span => Max + 1 - Min;
            public Range Clone => new Range(Min, Max, Axis);
            public bool Overlap(Range other) {
                var l = this.Min < other.Min ? this : other;
                var r = l == this ? other : this;

                return l.Max >= r.Min;
            }
            public override bool Equals(object obj) => 
                obj is Range range && 
                range.Min == Min && 
                range.Max == Max && 
                range.Axis  == Axis;
            public override string ToString() => $"{Min}..{Max}";
        }
    }
}
