﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day18 : DayBase
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

        public override object Part1(Input input)
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

        public override object Part2(Input input)
        {
            var map = (Array3D<int>)input.Cache;

            var bounds = new long[3,2];
            bounds[0,0] = map.Bounds[0].Item1-1;
            bounds[0,1] = map.Bounds[0].Item2+1;
            bounds[1,0] = map.Bounds[1].Item1-1;
            bounds[1,1] = map.Bounds[1].Item2+1;
            bounds[2,0] = map.Bounds[2].Item1-1;
            bounds[2,1] = map.Bounds[2].Item2+1;

            var shore = GetShore(map, bounds[0,0]-1, bounds[1, 0], bounds[2, 0], bounds);

            return shore.Values.Sum();
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

        private IDictionary<string, int> GetShore(Array3D<int> map, long x, long y, long z, long[,] bounds){
            map[x, y, z] = -1;
            var toTest = Neighbours((int)x, (int)y, (int)z)
                .Where(c => c[0] >= bounds[0, 0] && c[0] <= bounds[0, 1]
                    && c[1] >= bounds[1, 0] && c[1] <= bounds[1, 1]
                    && c[2] >= bounds[2, 0] && c[2] <= bounds[2, 1]).ToList();

            var count = 0;
            var result = new Dictionary<string, int>();
            foreach(var coords in toTest){
                if(map[coords[0], coords[1], coords[2]] > 0){
                    count ++;
                }else if(map[coords[0], coords[1], coords[2]] == 0){
                    foreach(var kv in GetShore(map, coords[0], coords[1], coords[2], bounds))
                        result.Add(kv.Key, kv.Value);
                }
            }

            if(count > 0 && !result.ContainsKey($"{x},{y},{z}"))
                result.Add($"{x},{y},{z}", count);

            return result;
        }
    }
}
