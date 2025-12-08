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
    internal class Day7: IDay
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
            var sx = 0;
            while (map[sx, 0] != 'S') sx++;

            var result = DropBeam(map, sx, 1);
            return result.ToString();
        }
        private string Part2(IMap<char> map)
        {
            var sx = 0;
            while (map[sx, 0] != 'S') sx++;

            var result = EvaluatePaths(map, sx, 1);
            return result.Count().ToString();
        }

        private int DropBeam(IMap<char> map, int sx, int sy)
        {
            while(sy < map.Height && map[sx, sy] == '.')
            {
                map[sx, sy] = '|';
                sy++;
            }

            if (sy >= map.Height) return 0;
            var ch = map[sx, sy];

            if(map[sx, sy] == '^')
            {
                var splits = 1;
                splits += DropBeam(map, sx - 1, sy);
                splits += DropBeam(map, sx + 1, sy);
                return splits;
            }

            return 0;
        }

        private IDictionary<(long, long), IEnumerable<string>> memory = new Dictionary<(long, long), IEnumerable<string>>();
        private IEnumerable<string> EvaluatePaths(IMap<char> map, int sx, int sy)
        {
            int ny = sy;
            while (ny < map.Height && map[sx, ny] == '.')
            {
                if (memory.ContainsKey((sx, ny)))
                    return memory[(sx, ny)];
                ny++;
            }

            if (ny >= map.Height) return new string[1] {string.Empty};

            if (map[sx, ny] == '^')
            {
                var left = EvaluatePaths(map, sx - 1, ny).Select(s => "L" + s).ToList();
                var right = EvaluatePaths(map, sx + 1, ny).Select(s => "R" + s).ToList();
                var result = left.Concat(right);

                for(var y=sy; y<=ny; y++)
                    memory.Add((sx, y), result);
                return result;
            }

            return new string[0];
        }
    }
}
