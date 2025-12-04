using AoC.Common;
using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Days
{
    internal class Day4: IDay
    {
        public static void RunAoC()
        {
            AocRuntime.Day<Day4>(4)
                .Callback(1, (d, t) => d.Part1(t.GetMap(true)))
                .Callback(2, (d, t) => d.Part2(t.GetMap(true)))
                .Test("example", "Inputs/Day4/example.txt")//.Part(1)//.Part(2)
                .Test("input", "Inputs/Day4/input.txt")//.Part(1)//.Part(2)
                .Run();
        }

        private string Part1(IMap<char> map)
        {
            return RemoveStep(map).Count().ToString();
        }
        private string Part2(IMap<char> map)
        {
            var ctr = 0;
            IList<(int x, int y)> toRemove;
            do
            {
                toRemove = RemoveStep(map).ToList();
                ctr += toRemove.Count();
                foreach (var point in toRemove)
                    map[point.x, point.y] = '.';
            }
            while(toRemove.Any());

            return ctr.ToString();
        }
        private IEnumerable<(int x, int y)> RemoveStep(IMap<char> map)
        {
            for (var y = 0; y <= map.Height; y++)
                for (var x = 0; x <= map.Width; x++)
                    if (map[x, y] == '@')
                        if (neighbourCount(map, x, y) < 4)
                            yield return (x, y);
        }

        private int neighbourCount(IMap<char> map, long x, long y)
        {
            var ctr = 0;
            for (var ly = y - 1; ly <= y + 1; ly++)
                for (var lx = x - 1; lx <= x + 1; lx++)
                    if (map[lx,ly] == '@')
                        ctr++;
            return ctr - 1;
        }
    }
}
