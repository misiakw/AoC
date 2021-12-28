using AoC_2021.Attributes;
using AoC_2021.Common;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day22
{
    [BasePath("Day22")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day22 : ParseableDay<Cuboid>
    {
        public Day22(string path) : base(path){ }

        public override Cuboid Parse(string input)
        {
            return new Cuboid(input);
        }

        [ExpectedResult(TestName = "Example", Result = "474140")]
        [ExpectedResult(TestName = "Input", Result = "601104")]
        public override string Part1(string testName)
        {
            var turnedOn = new List<Cuboid>();
            var minLimit = -50;
            var maxLimit = 50;
            var steps = Input.Where(i =>
            {
                if (i.MaxX < minLimit || i.MinX > maxLimit) return false;
                if (i.MaxY < minLimit || i.MinY > maxLimit) return false;
                if (i.MaxZ < minLimit || i.MinZ > maxLimit) return false;
                return true;
            }).ToList();

            foreach(var cuboid in steps)
            {
                var toIntersect = turnedOn.Where(c => c.Intersect(cuboid)).ToList();
                foreach (var intersect in toIntersect)
                {
                    turnedOn.Remove(intersect);
                    turnedOn.AddRange(intersect.Slice(cuboid));
                }
                if (!toIntersect.Any() && cuboid.State)
                    turnedOn.Add(cuboid);
            }

            return turnedOn.Select(c => c.Volume).Sum().ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "2758514936282235")]
        //[ExpectedResult(TestName = "Input", Result = 571032)]
        public override string Part2(string testName)
        {
            var turnedOn = new List<Cuboid>();

            foreach (var cuboid in Input)
            {
                var toIntersect = turnedOn.Where(c => c.Intersect(cuboid)).ToList();
                foreach (var intersect in toIntersect)
                {
                    turnedOn.Remove(intersect);
                    turnedOn.AddRange(intersect.Slice(cuboid));
                }
                if (!toIntersect.Any() && cuboid.State)
                    turnedOn.Add(cuboid);
            }

            return turnedOn.Select(c => c.Volume).Sum().ToString();
        }
    }
    public class Cuboid
    {
        public readonly bool State;
        public readonly long MinX, MaxX, MinY, MaxY, MinZ, MaxZ;
        public long Volume => (MaxX - MinX) * (MaxY - MinY) * (MaxZ - MinZ);
        public Cuboid(long minX, long maxX, long minY, long maxY, long minZ, long maxZ)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;
        }
        public Cuboid(string input)
        {
            var parts = input.Trim().Split(" ");
            State = parts[0].Equals("on");
            var ranges = parts[1].Split(',').Select(l => l.Trim().Substring(2).Split("..").Select(s => long.Parse(s)).ToArray()).ToArray();

            MinX = ranges[0][0];
            MaxX = ranges[0][1];
            MinY = ranges[1][0];
            MaxY = ranges[1][1];
            MinZ = ranges[2][0];
            MaxZ = ranges[2][1];
        }

        public bool Intersect(Cuboid other)
        {
            //ToDo: implement logic to verify if cuboids ocupy the same space
            return false;
        }

        public IEnumerable<Cuboid> Slice(Cuboid other)
        {
            //ToDo: Implement slicing cuboids. return all shigning parts;
            yield return this;
            if (other.State)
                yield return other;
        }
    }
}
