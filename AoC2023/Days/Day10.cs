using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day10 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day10/example1.txt")
               .Part1(4);
            //   .Part2(2);
            builder.New("example2", "./Inputs/Day10/example2.txt")
               .Part1(8);
            //   .Part2(2);
            builder.New("output", "./Inputs/Day10/output.txt")
                .Part1(7005);
             //   .Part2(933);
        }
        public override long Part1(IComparableInput<long> input)
        {
            var start = (0, 0);
            var lines = ReadLines(input);
            var map = new Dir[lines[0].Length, lines.Count];
            //ReadMap
            for (var y = 0; y < lines.Count; y++)
                for (var x = 0; x < lines[y].Length; x++)
                {
                    map[x, y] = lines[y][x] switch
                    {
                        '|' => Dir.North | Dir.South,
                        '-' => Dir.East | Dir.West,
                        'L' => Dir.North | Dir.East,
                        'J' => Dir.North | Dir.West,
                        '7' => Dir.South | Dir.West,
                        'F' => Dir.South | Dir.East,
                        _ => Dir.None
                    };
                    if (lines[y][x] == 'S')
                        start = (x, y);
                }

            var pos = PrepareStart(map, start, lines[0].Length, lines.Count);
            map[start.Item1, start.Item2] = pos[0].Item3 | pos[1].Item3;
            var ctr = 0;
            do
            {
                for(var i=0; i<2; i++)
                {
                    var newPos = pos[i].Item3 switch
                    {
                        Dir.North => (pos[i].Item1, pos[i].Item2 - 1),
                        Dir.South => (pos[i].Item1, pos[i].Item2 + 1),
                        Dir.East => (pos[i].Item1 + 1, pos[i].Item2),
                        Dir.West => (pos[i].Item1 - 1, pos[i].Item2)
                    };
                    var newDir = map[newPos.Item1, newPos.Item2] & pos[i].Item3 switch
                    {
                        Dir.North => ~Dir.South,
                        Dir.South => ~Dir.North,
                        Dir.East => ~Dir.West,
                        Dir.West => ~Dir.East
                    };
                    pos[i] = (newPos.Item1, newPos.Item2, newDir);
                }
                ctr++;
            } while (pos[0].Item1 != pos[1].Item1 || pos[0].Item2 != pos[1].Item2);

            return ctr;
        }

        public override long Part2(IComparableInput<long> input)
        {
            throw new NotImplementedException();
        }

        [Flags]
        enum Dir
        {
            None = 0,
            North = 1,
            East = 2,
            South = 4,
            West = 8
        }
        private (int, int, Dir)[] PrepareStart(Dir[,]map, (int, int) start, int width, int heigth)
        {
            var result = new (int, int, Dir)[2];

            if (start.Item1 - 1 >= 0 && map[start.Item1 - 1, start.Item2].HasFlag(Dir.East))
                if (result[0].Item3 == Dir.None)
                    result[0] = (start.Item1, start.Item2, Dir.West);
                else
                    result[1] = (start.Item1, start.Item2, Dir.West);
            if (start.Item1 + 1 < width && map[start.Item1 + 1, start.Item2].HasFlag(Dir.West))
                if (result[0].Item3 == Dir.None)
                    result[0] = (start.Item1, start.Item2, Dir.East);
                else
                    result[1] = (start.Item1, start.Item2, Dir.East);

            if (start.Item2 - 1 >= 0 && map[start.Item1, start.Item2 - 1].HasFlag(Dir.South))
                if (result[0].Item3 == Dir.None)
                    result[0] = (start.Item1, start.Item2, Dir.North);
                else
                    result[1] = (start.Item1, start.Item2, Dir.North);
            if (start.Item2 + 1 < heigth && map[start.Item1, start.Item2 + 1].HasFlag(Dir.North))
                if (result[0].Item3 == Dir.None)
                    result[0] = (start.Item1, start.Item2, Dir.South);
                else
                    result[1] = (start.Item1, start.Item2, Dir.South);
            return result;
        }
    }
}
