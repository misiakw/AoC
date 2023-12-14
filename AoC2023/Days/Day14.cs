using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using AoC.Common.Maps;

namespace AoC2023.Days
{
    public class Day14 : AbstractDay<long, MapInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, MapInput<long>> builder)
        {
            builder.New("./Inputs/Day14/example1.txt", "example1")
               .Part1(136);
                //.Part2(-1);
            builder.New("./Inputs/Day14/output.txt", "output")
                .Part1(107951);
                //.Part2(790194712336);
        }

        public override long Part1(MapInput<long> input)
        {
            var map = input.GetMap();

            map = MoveUp(map);
            var result = 0L;
            for (var y = 0; y < map.Height; y++)
                result += BeamWeight(map, y);

            return result;
        }

        public override long Part2(MapInput<long> input)
        {
            throw new NotImplementedException();
        }

        private IMap<char> MoveUp(IMap<char> map)
        {
            for (var by = 0; by < map.Height; by++)
                for (var x= 0; x < map.Width; x++)
                    if(map[x, by] == 'O')
                    {
                        var ny = by;
                        while (ny - 1 >= 0 && map[x, ny-1] == '.')
                            ny--;
                        if(ny != by)
                        {
                            map[x, ny] = 'O';
                            map[x, by] = '.';
                        }
                    }

            return map;
        }

        private long BeamWeight(IMap<char> map, int y)
        {
            var result = 0;
            for (var x = 0; x < map.Width; x++)
                if (map[x, y] == 'O')
                    result++;
            return result * (map.Height - y);
        }

    }
}
