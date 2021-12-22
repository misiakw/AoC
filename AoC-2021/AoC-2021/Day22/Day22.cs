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
            var minLimit = -50;
            var maxLimit = 50;
            var steps = Input.Where(i =>
            {
                if (i.MaxX < minLimit || i.MinX > maxLimit) return false;
                if (i.MaxY < minLimit || i.MinY > maxLimit) return false;
                if (i.MaxZ < minLimit || i.MinZ > maxLimit) return false;
                return true;
            }).ToList();

            var reactor = new Reactor();

            foreach(var step in steps)
            {
                var minX = step.MinX <= minLimit ? minLimit : step.MinX;
                var maxX = step.MaxX >= maxLimit ? maxLimit : step.MaxX;
                var minY = step.MinY <= minLimit ? minLimit : step.MinY;
                var maxY = step.MaxY >= maxLimit ? maxLimit : step.MaxY;
                var minZ = step.MinZ <= minLimit ? minLimit : step.MinZ;
                var maxZ = step.MaxZ >= maxLimit ? maxLimit : step.MaxZ;

                for (var x = minX; x <= maxX; x++)
                    for (var y = minY; y <= maxY; y++)
                        for (var z = minZ; z <= maxZ; z++)
                            reactor[x, y, z] = step.State;
            }

            return reactor.TurnedOn.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "2758514936282235")]
        //[ExpectedResult(TestName = "Input", Result = "571032")]
        public override string Part2(string testName)
        {
            //need optimization. sort of "work in region"
            var reactor = new Reactor();

            foreach (var step in Input)
            {
                var minX = step.MinX;
                var maxX = step.MaxX;
                var minY = step.MinY;
                var maxY = step.MaxY;
                var minZ = step.MinZ;
                var maxZ = step.MaxZ;

                for (var x = minX; x <= maxX; x++)
                    for (var y = minY; y <= maxY; y++)
                        for (var z = minZ; z <= maxZ; z++)
                            reactor[x, y, z] = step.State;
            }

            return reactor.TurnedOn.ToString();
        }
    }
    public class Cuboid
    {
        public readonly bool State;
        public readonly long MinX, MaxX, MinY, MaxY, MinZ, MaxZ;
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
    }

    public  class Reactor: Array3D<bool>
    {
        public Reactor() : base(false) { }

        public long TurnedOn => _data.Values.Where(v => v == true).Count();
    }
}
