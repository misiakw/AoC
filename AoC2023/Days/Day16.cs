using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using AoC.Common.Maps;

namespace AoC2023.Days
{
    public class Day16 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day16/example1.txt")
               .Part1(46)
                .Part2(51);
            builder.New("output", "./Inputs/Day16/output.txt")
                .Part1(7415)
                .Part2(7943);
        }
        public override long Part1(IComparableInput<long> input)
        {
            var map = ReadMap(input);

            return Energize(map[0, 0], Dir.Right, map);
        }

        public override long Part2(IComparableInput<long> input)
        {
            var map = ReadMap(input);
            var lits = new List<int>();

            for (var col = 0; col < map.Width; col++)
            {
                lits.Add(Energize(map[col, 0], Dir.Down, map));
                lits.Add(Energize(map[col, map.Height - 1], Dir.Up, map));
            }
            for (var row = 0; row < map.Height; row++)
            {
                lits.Add(Energize(map[0, row], Dir.Right, map));
                lits.Add(Energize(map[map.Width-1, row], Dir.Left, map));
            }

            return lits.Max();
        }

        private StaticMap<BlockBase> ReadMap(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var map = new StaticMap<BlockBase>(lines[0].Length, lines.Count);

            for (var y = 0; y < lines.Count; y++)
                for (var x = 0; x < lines[y].Length; x++)
                    map[x, y] = lines[y][x] switch
                    {
                        '.' => new EmptyBlock(x, y, map),
                        '\\' => new MirrorBlock(x, y, map, lines[y][x]),
                        '/' => new MirrorBlock(x, y, map, lines[y][x]),
                        '|' => new SplitterBlock(x, y, map, lines[y][x]),
                        '-' => new SplitterBlock(x, y, map, lines[y][x]),
                    };
            return map;
        }

        private int Energize(BlockBase start, Dir directory, StaticMap<BlockBase> map)
        {
            var toConsider = new List<(BlockBase, Dir)>() { (start, directory) };

            while (toConsider.Any())
            {
                var next = new List<(BlockBase, Dir)>();
                foreach (var block in toConsider)
                {
                    var newOnes = block.Item1.PassLight(block.Item2);
                    if (newOnes.Any())
                        next.AddRange(newOnes);
                }
                toConsider = next;
            }

            var ctr = 0;
            for (var y = 0; y < map.Height; y++)
                for (var x = 0; x < map.Width; x++)
                {
                    if (map[x, y].IsLit)
                        ctr++;
                    map[x, y].Reset();
                }

            return ctr;
        }

        private enum Dir
        {
            Up, Down, Left, Right
        }

        private abstract class BlockBase
        {
            public readonly long X, Y;
            public readonly StaticMap<BlockBase> map;
            public bool IsLit = false;
            protected IList<Dir> PassingThrought = new List<Dir>();
            public BlockBase(long x, long y, StaticMap<BlockBase> map)
            {
                this.map = map;
                this.X = x;
                this.Y = y;
            }
            public IList<(BlockBase, Dir)> PassLight(Dir directory)
            {
                IsLit = true;
                if (PassingThrought.Contains(directory))
                    return new List<(BlockBase, Dir)>();
                PassingThrought.Add(directory);

                return GetNext(directory).Where(d => d.HasValue).Select(d => d.Value).ToList();
            }
            public abstract IEnumerable<(BlockBase, Dir)?> GetNext(Dir directory);

            protected bool CanUp => Y - 1 >= 0;
            protected bool CanDown => Y + 1 < map.Height;
            protected bool CanLeft => X - 1 >= 0;
            protected bool CanRight => X + 1 < map.Width;

            public void Reset()
            {
                PassingThrought = new List<Dir>();
                IsLit = false;
            }
        }

        private class EmptyBlock : BlockBase
        {
            public EmptyBlock(long x, long y, StaticMap<BlockBase> map) : base(x, y, map) { }
            public override IEnumerable<(BlockBase, Dir)?> GetNext(Dir directory)
            {
                yield return directory switch
                {
                    Dir.Up => CanUp ? (map[X, Y - 1], directory) : null,
                    Dir.Down => CanDown ? (map[X, Y + 1], directory) : null,
                    Dir.Left => CanLeft ? (map[X - 1, Y], directory) : null,
                    Dir.Right => CanRight ? (map[X + 1, Y], directory) : null,
                };
            }
        }

        private class MirrorBlock : BlockBase
        {
            private readonly char mirror;
            public MirrorBlock(long x, long y, StaticMap<BlockBase> map, char mirror) : base(x, y, map)
            {
                this.mirror = mirror;
            }
            public override IEnumerable<(BlockBase, Dir)?> GetNext(Dir directory)
            {
                if (this.mirror == '/')
                    yield return directory switch
                    {
                        Dir.Up => CanRight ? (map[X + 1, Y], Dir.Right) : null,
                        Dir.Down => CanLeft ? (map[X - 1, Y], Dir.Left) : null,
                        Dir.Left => CanDown ? (map[X, Y + 1], Dir.Down) : null,
                        Dir.Right => CanUp ? (map[X, Y - 1], Dir.Up) : null,
                    };
                else // \
                    yield return directory switch
                    {
                        Dir.Up => CanLeft ? (map[X - 1, Y], Dir.Left) : null,
                        Dir.Down => CanRight ? (map[X + 1, Y], Dir.Right) : null,
                        Dir.Left => CanUp ? (map[X, Y - 1], Dir.Up) : null,
                        Dir.Right => CanDown ? (map[X, Y + 1], Dir.Down) : null
                    };
            }
        }

        private class SplitterBlock : BlockBase
        {
            private readonly char splitter;
            public SplitterBlock(long x, long y, StaticMap<BlockBase> map, char splitter) : base(x, y, map)
            {
                this.splitter = splitter;
            }
            public override IEnumerable<(BlockBase, Dir)?> GetNext(Dir directory)
            {
                if (splitter == '|')
                {
                    if (directory == Dir.Left || directory == Dir.Right)
                    {
                        yield return CanUp ? (map[X, Y - 1], Dir.Up) : null;
                        yield return CanDown ? (map[X, Y + 1], Dir.Down) : null;
                    }
                    else
                    {
                        yield return directory switch
                        {
                            Dir.Up => CanUp ? (map[X, Y - 1], Dir.Up) : null,
                            Dir.Down => CanDown ? (map[X, Y + 1], Dir.Down) : null
                        };
                    }
                }
                else
                {
                    if (directory == Dir.Up || directory == Dir.Down)
                    {
                        yield return CanLeft ? (map[X - 1, Y], Dir.Left) : null;
                        yield return CanRight ? (map[X + 1, Y], Dir.Right) : null;
                    }
                    else
                    {
                        yield return directory switch
                        {
                            Dir.Left => CanLeft ? (map[X - 1, Y], Dir.Left) : null,
                            Dir.Right => CanRight ? (map[X + 1, Y], Dir.Right) : null
                        };
                    }
                }
            }
        }
    }
}
