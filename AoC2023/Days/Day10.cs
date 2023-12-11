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
            var lines = ReadLines(input);
            var map = new Day10Map(lines);
            //ReadMap


            var pos = new (long, long, Dir)[2];
            foreach (var x in new List<Dir> { Dir.North, Dir.South, Dir.East, Dir.West })
                if(map[map.Start.X, map.Start.Y].directory.HasFlag(x))
                    if (pos[0].Item3 == Dir.None)
                        pos[0] = (map.Start.X, map.Start.Y, x);
                    else
                        pos[1] = (map.Start.X, map.Start.Y, x);
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
                    var newDir = map[newPos.Item1, newPos.Item2].directory & pos[i].Item3 switch
                    {
                        Dir.North => ~Dir.South,
                        Dir.South => ~Dir.North,
                        Dir.East => ~Dir.West,
                        Dir.West => ~Dir.East
                    };
                    pos[i] = (newPos.Item1, newPos.Item2, newDir);
                    map.SetPipeStatus(newPos.Item1, newPos.Item2, true);
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
        

        private void PrintMap((Dir, bool)[,] map, int width, int heigth)
        {
            for(var y=0; y<heigth; y++)
            {
                for (var x = 0; x < width; x++)
                    Console.Write(map[x, y].Item2 ? "#" : '.');
                Console.WriteLine();
            }
        }

        private struct MapPoint
        {
            public Dir directory;
            public bool isPipe;
        }

        private class Day10Map
        {

            public readonly long Width;
            public readonly long Height;
            private readonly MapPoint[,] Data;
            public readonly Point Start;

            public Day10Map(IList<string> lines)
            {
                Width = lines[0].Length;
                Height = lines.Count;
                Data = new MapPoint[Width, Height];

                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                    {
                        Data[x, y] = new MapPoint
                        {
                            directory = lines[y][x] switch
                            {
                                '|' => Dir.North | Dir.South,
                                '-' => Dir.East | Dir.West,
                                'L' => Dir.North | Dir.East,
                                'J' => Dir.North | Dir.West,
                                '7' => Dir.South | Dir.West,
                                'F' => Dir.South | Dir.East,
                                _ => Dir.None
                            },
                            isPipe = false
                        };
                        if (lines[y][x] == 'S')
                            Start = new Point(x, y);
                    }
                PrepareStart();
            }

            public MapPoint this[long x, long y]{
                get => this.Data[x, y];
                set => this.Data[x, y] = value;
            }

            private void PrepareStart()
            {
                Data[Start.X, Start.Y].isPipe = true;
                if (Start.X - 1 >= 0 && Data[Start.X - 1, Start.Y].directory.HasFlag(Dir.East))
                    Data[Start.X, Start.Y].directory |=  Dir.West;
                if (Start.X + 1 < Width && Data[Start.X + 1, Start.Y].directory.HasFlag(Dir.West))
                    Data[Start.X, Start.Y].directory |= Dir.East;

                if (Start.Y - 1 >= 0 && Data[Start.X, Start.Y - 1].directory.HasFlag(Dir.South))
                    Data[Start.X, Start.Y].directory |= Dir.North;
                if (Start.Y + 1 < Height && Data[Start.X, Start.Y + 1].directory.HasFlag(Dir.North))
                    Data[Start.X, Start.Y].directory |= Dir.South;
            }

            public void SetPipeStatus(long x, long y, bool isPipe)
            {
                var tmp = this[x, y];
                tmp.isPipe = isPipe;
                this[x, y] = tmp;
            }
        }
    }
}
