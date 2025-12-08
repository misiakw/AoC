using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;
using AoC.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Days
{
    [DayNum(7)]
    file class Day7: IDay
    {
        public void RunAoC()
        {
            AocRuntime.Day<Day7>(7)
                .Callback(1, (d, t) => d.Part1(t.GetMap()))
                .Callback(2, (d, t) => d.Part2(t.GetMap()))
                .Test("example", "Inputs/Day7/example.txt") //.Part(1)//.Part(2)
                .Test("input", "Inputs/Day7/input.txt") //.Part(1)//.Part(2)
                .Run();
        }

        private string Part1(IMap<char> map)
        {
            return new LaserTree(map).GetSplitters().Count().ToString();
        }
        private string Part2(IMap<char> map)
        {
            return new LaserTree(map).GetPotential().ToString();
        }
    }

    file class LaserTree
    {
        private readonly IDictionary<(long, long), Splitter> _splitters = new Dictionary<(long, long), Splitter>();
        private readonly Splitter _floor = new Splitter(-1, -1);
        private Splitter _head;
        public LaserTree(IMap<char> map)
        {
            var sx = 0;
            var sy = 1;
            while (map[sx, 0] != 'S') sx++;

            _head = DropBeam(map, sx, 1);
        }
        
        private Splitter DropBeam(IMap<char> map, long x, long y)
        {
            while (y < map.Height && map[x, y] != '^')
                y++;
            if (y == map.Height) return _floor;
            if (_splitters.ContainsKey((x, y)))
                return _splitters[(x, y)];
            var result = new Splitter(x, y);
            _splitters.Add((x, y), result);
            result.Left = DropBeam(map, x - 1, y);
            result.Left.AddSource(result);
            result.Right = DropBeam(map, x + 1, y);
            result.Right.AddSource(result);
            return result;
        }

        public IEnumerable<Splitter> GetSplitters() => _splitters.Values;
        
        public long GetPotential()
        {
            _floor.ProcessPotential();
            return _head.Potential;
        }
    }
    
    file class Splitter(long x, long y)
    { 
        public IList<Splitter> Sources = new List<Splitter>();
        public Splitter Left;
        public Splitter Right;
        private long? leftPotential = null;
        private long? rightPotential = null;

        public long Potential => (x == -1 && y == -1) ? 1
            : (leftPotential ?? -100) + (rightPotential ?? -100);

        public void AddSource(Splitter newSource)
        {
            if (!Sources.Contains(newSource))
                Sources.Add(newSource);
        }

        public void ProcessPotential()
        {
            foreach (var root in Sources)
                root.ProvidePotential(this);
        }

        private void ProvidePotential(Splitter source)
        {
            if (leftPotential.HasValue && rightPotential.HasValue)
                return;
            if (source == Left)
                leftPotential = Left.Potential;
            if (source == Right)
                rightPotential = Right.Potential;
            if(leftPotential.HasValue && rightPotential.HasValue)
                foreach (var root in Sources)
                    root.ProvidePotential(this);
        }
        
        public override string ToString() => $"[{x},{y}]";
    }
}
