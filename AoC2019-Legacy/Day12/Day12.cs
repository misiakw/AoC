using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Day12
{
    [Day("Day12")]
    [Input("Short Input", typeof(Day12ShortTestInput))]
    [Input("Long Input", typeof(Day12LongTestInput))]
    [Input("AoC", typeof(Day12AocInput))]
    public class Day12: IDay
    {
        public string Task1(IInput input)
        {
            var moons = new List<Moon>();
            foreach (var line in input.Input.Split("\n"))
            {
                moons.Add(new Moon(line.Trim()));
            }
            foreach (var moon in moons)
                moon.OtherMoons = moons.Where(m => moon != m).ToList();

            var maxSteps = 1000;

            var start = DateTime.Now;

            for (var i = 0; i < maxSteps; i++)
            {
                Symulate(moons);
            }

            var stop = DateTime.Now;

            Console.WriteLine($"symulation took {new TimeSpan(stop.Ticks - start.Ticks).Milliseconds}ms");

            return moons.Select(m =>  m.KineticEnergy*m.PotentialEnergy).Sum().ToString();
        }

        public string Task2(IInput input)
        {
            var moons = new List<Moon>();

            var states = new List<string>();

            foreach (var line in input.Input.Split("\n"))
            {
                var moon = new Moon(line.Trim());
                moons.Add(moon);
            }
            
            var loopX = GetAxisLoop(moons.Select(m => m.FlatX).ToList());
            var loopY = GetAxisLoop(moons.Select(m => m.FlatY).ToList());
            var loopZ = GetAxisLoop(moons.Select(m => m.FlatZ).ToList());


            var result = nww(loopX, loopY);
            result = nww(result, loopZ);

            return result.ToString();
        }

        private long GetAxisLoop(IList<FlattenMoon> moons)
        {
            var state = string.Join(";",moons.Select(m => $"<{m.pos};{m.vel}>"));

            var ctr = 0l;
            do
            {
                SymulateAxis(moons);
                ctr++;
            } while (!state.Equals(string.Join(";", moons.Select(m => $"<{m.pos};{m.vel}>"))));

            return ctr;
        }

        private void SymulateAxis(IList<FlattenMoon> moons)
        {
            foreach (var moon in moons)
            {
                var otherMoons = moons.Where(m => m != moon).ToList();
                moon.vel += otherMoons.Sum(m => m.pos.CompareTo(moon.pos));
            }

            foreach (var moon in moons)
                moon.pos += moon.vel;
        }

        private void Symulate(IList<Moon> moons)
        {
            foreach(var moon in moons)
            {
                moon.Velocity.X += moon.OtherMoons.Select(m => m.Position.X.CompareTo(moon.Position.X)).Sum();
                moon.Velocity.Y += moon.OtherMoons.Select(m => m.Position.Y.CompareTo(moon.Position.Y)).Sum();
                moon.Velocity.Z += moon.OtherMoons.Select(m => m.Position.Z.CompareTo(moon.Position.Z)).Sum();
            }
            
            foreach (var moon in moons)
            {
                moon.Move();
            }
        }

        private class Position
        {
            public long X;
            public long Y;
            public long Z;

            public override string ToString() => $"<{X},{Y},{Z}>";
        }

        private class Moon
        {
            public readonly Position Position;
            public readonly Position Velocity;
            public IList<Moon> OtherMoons;

            public Moon(string input)
            {
                Velocity = new Position {X = 0, Y = 0, Z = 0};

                input = input.Substring(1, input.Length - 2);
                var tiles = input.Split(", ");
                Position = new Position
                {
                    X = long.Parse(tiles[0].Substring(2).Trim()),
                    Y = long.Parse(tiles[1].Substring(2).Trim()),
                    Z = long.Parse(tiles[2].Substring(2).Trim()),
                };
            }

            public override string ToString() => $"p{Position}v{Velocity}";

            public void Move()
            {
                Position.X += Velocity.X;
                Position.Y += Velocity.Y;
                Position.Z += Velocity.Z;
            }

            public long StateHash => Velocity.X + Velocity.Y + Velocity.Z + Position.X + Position.Y + Position.Z;
            public long KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);
            public long PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);
            
            public FlattenMoon FlatX => new FlattenMoon {pos = Position.X, vel = Velocity.X};
            public FlattenMoon FlatY => new FlattenMoon {pos = Position.Y, vel = Velocity.Y};
            public FlattenMoon FlatZ => new FlattenMoon {pos = Position.Z, vel = Velocity.Z};
        }

        public class FlattenMoon
        {
            public long pos;
            public long vel;
        }

        private long nwd(long a, long b)
        {
            while (a != b)
            {
                if (a > b)
                    a -= b;
                else if (b > a)
                    b -= a;
            }

            return a;
        }

        private long nww(long a, long b)
        {
            return (a * b) / nwd(a, b);
        }
    }
}
