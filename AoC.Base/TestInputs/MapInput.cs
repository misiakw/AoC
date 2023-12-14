using AoC.Common;
using AoC.Common.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Base.TestInputs
{
    public class MapInput<TResult> : IComparableInput<TResult> where TResult : IComparable
    {
        public MapInput(string name, string filePath) : base(filePath, name)
        { }

        public IMap<char> GetMap(bool isInfinite = false) 
            => GetMap(new MapBuilderParams<char>() { IsInfinite = isInfinite });
        public IMap<char> GetMap(MapBuilderParams<char> mapParams)
        {
            var task = ReadLines();
            task.Wait();
            var lines = task.Result.ToList();

            mapParams.Width = lines[0].Length;
            mapParams.Height = lines.Count;

            var map = MapBuilder<char>.GetEmpty(mapParams);
            for (var y = 0; y < map.Height; y++)
                for (var x = 0; x < map.Width; x++)
                    map[x, y] = lines[y][x];
            return map;
        }
    }
}
