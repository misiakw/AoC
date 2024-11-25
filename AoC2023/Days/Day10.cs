using System;
using System.Collections.Generic;
using System.Linq;
using AoC.LegacyBase;
using AoC.Common;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day10 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day10/example1.txt")
                    .Part(1).Correct(4)
                .Test("example2", "./Inputs/Day10/example1-2.txt")
                    .Part(1).Correct(8)
                .Test("example2", "./Inputs/Day10/example2-1.txt")
                    .Part(2).Correct(4)
                .Test("example2", "./Inputs/Day10/example2-2.txt")
                    .Part(2).Correct(8)
                .Test("output", "./Inputs/Day10/output.txt")
                    .Part(1).Correct(7005)
                .Part(2).Correct(417);
        }
        private Day10Map map;
        public override string Part1(TestState input)
        {
            var lines = input.GetLinesAsync().ToEnumerable().ToList();
            map = new Day10Map(lines);
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

            return ctr.ToString();
        }


        public override string Part2(TestState input)
        {
            if (map == null)
                Part1(input);
            var sX = 0;
            var sY = map.Start.Y;
            while (!map[sX, sY].isPipe)
                sX++;

            switch(map[sX, sY].directory)
            {
                case Dir.North | Dir.South:
                case Dir.North | Dir.East:
                    map.FloodFill(1, Dir.NorthEast, sX, sY);
                    break;
                case Dir.South | Dir.East:
                    map.FloodFill(1, Dir.SouthEast, sX, sY);
                    break;
                default:
                    throw new InvalidProgramException("oops cos nie zatrybilo w matematyce");
            }

            return map.FilledFullCount().ToString();
        }

        [Flags]
        enum Dir
        {
            None = 0,
            North = 1,
            East = 2,
            South = 4,
            West = 8,
            NorthEast = 3,
            SouthEast = 6,
            SouthWest = 12,
            NorthWest = 9
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

        private class MapPoint
        {
            public Dir directory;
            public bool isPipe;
            public readonly IDictionary<Dir, byte> Colors = new Dictionary<Dir, byte>();

            public void FillColor(byte color, Dir quadrant)
            {
                if (Colors.ContainsKey(quadrant))
                    return;
                if (!isPipe)
                {
                    Colors.Add(Dir.NorthEast, color);
                    Colors.Add(Dir.SouthEast, color);
                    Colors.Add(Dir.SouthWest, color);
                    Colors.Add(Dir.NorthWest, color);
                    return;
                }

                Colors.Add(quadrant, color);
                switch (quadrant)
                {
                    case Dir.NorthEast:
                        if (!directory.HasFlag(Dir.East))
                            FillColor(color, Dir.SouthEast);
                        if (!directory.HasFlag(Dir.North))
                            FillColor(color, Dir.NorthWest);
                            break;
                    case Dir.SouthEast:
                        if (!directory.HasFlag(Dir.East))
                            FillColor(color, Dir.NorthEast);
                        if (!directory.HasFlag(Dir.South))
                            FillColor(color, Dir.SouthWest);
                        break;
                    case Dir.SouthWest:
                        if (!directory.HasFlag(Dir.West))
                            FillColor(color, Dir.NorthWest);
                        if (!directory.HasFlag(Dir.South))
                            FillColor(color, Dir.SouthEast);
                        break;
                    case Dir.NorthWest:
                        if (!directory.HasFlag(Dir.West))
                            FillColor(color, Dir.SouthWest);
                        if (!directory.HasFlag(Dir.North))
                            FillColor(color, Dir.NorthEast);
                        break;

                }
            }
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

            private bool HasField(long X, long Y)
                => (X >= 0 || X < Width) && (Y >= 0 || Y < Height);

            public void FloodFill(byte color, Dir quadrant, long X, long Y)
            {
                if (!HasField(X, Y))
                    return;
                var tile = this[X, Y];

                if (tile.Colors.ContainsKey(quadrant))
                    return;

                tile.FillColor(color, quadrant);

                if (tile.Colors.ContainsKey(Dir.NorthEast))
                {
                    FloodFill(color, Dir.NorthWest, X + 1, Y);
                    FloodFill(color, Dir.SouthEast, X, Y - 1);
                }
                if (tile.Colors.ContainsKey(Dir.SouthEast))
                {
                    FloodFill(color, Dir.SouthWest, X + 1, Y);
                    FloodFill(color, Dir.NorthEast, X, Y + 1);
                }
                if (tile.Colors.ContainsKey(Dir.SouthWest))
                {
                    FloodFill(color, Dir.SouthEast, X - 1, Y);
                    FloodFill(color, Dir.NorthWest, X, Y + 1);
                }
                if (tile.Colors.ContainsKey(Dir.NorthWest))
                {
                    FloodFill(color, Dir.NorthEast, X - 1, Y);
                    FloodFill(color, Dir.SouthWest, X, Y - 1);
                }
            }

            public long FilledFullCount()
            {
                var ctr = 0;
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        if (this[x, y].Colors.Count() == 4)
                            ctr++;
                return ctr;
            }
        }
    }
}
