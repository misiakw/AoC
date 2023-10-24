using AoC.Base;
using AoC.Common;
using System.Collections.Generic;
using System.Linq;
using Range = AoC.Common.Range;

namespace AoC2022
{
    public class Day18 : LegacyDayBase
    {
        public Day18() : base(18)
        {
            Input("example1")
                .RunPart(1, 10)
            .Input("example2")
                .RunPart(1, 64)
                .RunPart(2, 58)
            .Input("output")
                .RunPart(1, 4242)
                .RunPart(2, 2428);
        }

        public override object Part1(LegacyInput input)
        {
            var map = new Array3D<int>(0);

            var coords = input.Lines.Select(
                l => l.Trim().Split(",").Select(int.Parse).ToArray()
            ).ToList();
            foreach(var coord in coords){
                var sides = Neighbours(coord)
                    .Where(pos => map[pos[0], pos[1], pos[2]] == 0)
                    .Count();
                map[coord[0], coord[1], coord[2]] = sides;
                foreach(var n in Neighbours(coord))
                    if (map[n[0], n[1], n[2]] > 0)
                        map[n[0], n[1], n[2]]--;
            }
            
            input.Cache = map;

            return map.ToList().Sum();
        }

        public override object Part2(LegacyInput input)
        {
            var map = (Array3D<int>)(input.Cache ?? new Array3D<int>(0));

            var bounds = map.Bounds.Select(b => new Range(b.Min-1, b.Max+1)).ToArray();

            return GetShoreLen(map, bounds[0].Min, bounds[1].Min, bounds[2].Min, bounds);
        }

        private IEnumerable<int[]> Neighbours(int[] arr) => Neighbours(arr[0], arr[1], arr[2]);
        private IEnumerable<int[]> Neighbours(int x, int y, int z){
            yield return new int[3]{x-1, y, z};
            yield return new int[3]{x+1, y, z};
            yield return new int[3]{x, y-1, z};
            yield return new int[3]{x, y+1, z};
            yield return new int[3]{x, y, z-1};
            yield return new int[3]{x, y, z+1};
        }

        private int GetShoreLen(Array3D<int> map, int[] pos, Range[] bounds)
            => GetShoreLen(map, pos[0], pos[1], pos[2], bounds);
        private int GetShoreLen(Array3D<int> map, long x, long y, long z, Range[] bounds){
            map[x, y, z] = -1;
            var toTest = Neighbours((int)x, (int)y, (int)z)
                .Where(c => c[0] >= bounds[0].Min && c[0] <= bounds[0].Max
                    && c[1] >= bounds[1].Min && c[1] <= bounds[1].Max
                    && c[2] >= bounds[2].Min && c[2] <= bounds[2].Max).ToList();

            var result = 0;
            foreach(var coords in toTest)
                if(map[coords[0], coords[1], coords[2]] > 0)
                    result ++;
                else if(map[coords[0], coords[1], coords[2]] == 0)
                    result += GetShoreLen(map, coords, bounds);

            return result;
        }
    }
}
