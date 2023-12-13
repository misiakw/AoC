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
    public class Day13 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day13/example1.txt")
               .Part1(405);
            //.Part2(82000210);
            builder.New("output", "./Inputs/Day13/output.txt")
                .Part1(27300);
            //.Part2(790194712336);
        }
        public override long Part1(IComparableInput<long> input)
        {
            var arrays = GetCharArrays(input);
            return arrays.Sum(arr => arr.GetScore(0));
        }

        public override long Part2(IComparableInput<long> input)
        {
            var arrays = GetCharArrays(input);
            return arrays.Sum(arr => arr.GetScore(1));
        }

        private IEnumerable<CharArray> GetCharArrays(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var map = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return new CharArray(map);
                    map.Clear();
                }
                else
                    map.Add(line);
            }
            yield return new CharArray(map);
        }

        private class CharArray
        {
            public readonly int Width, Height;
            public char[,] Map;

            public CharArray(IList<string> lines)
            {
                Width = lines[0].Length;
                Height = lines.Count();
                Map = new char[Width, Height];
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        Map[x, y] = lines[y][x];
            }

            public int GetScore(int smugesToFix)
            {
                var vertical = SearchForMirrorCollumns(smugesToFix); // |
                var horizontal = SearchForMirrorRows(smugesToFix); // -

                return (vertical > 0 ? vertical : 0) + 100 * (horizontal > 0 ? horizontal : 0);
            }

            private int SearchForMirrorCollumns(int smugesToFix) => SearchForMirror(Height, Width, (x, i) => Map[x, i], smugesToFix);
            private int SearchForMirrorRows(int smugesToFix) => SearchForMirror(Width, Height, (y, i) => Map[i, y], smugesToFix);
            private int SearchForMirror(int eqLimit, int searchLimit, Func<int, int, char> dataFunc, int smugesToFix)
            {
                for (var i = 0; i < searchLimit - 1; i++) {
                        var isMirrored = true;
                        for (var j = 0; i - j >= 0 && i + j + 1 < searchLimit; j++)
                        {
                            var smuges = SmugeCount(i - j, i + j + 1, dataFunc, eqLimit);
                            if (smuges != 0)
                            {
                                isMirrored = false;
                                break;
                            }
                        }
                        if (isMirrored)
                            return i + 1;
                }

                return -1;
            }

            private int SmugeCount(int a, int b, Func<int, int, char> data, int limit)
            {
                var ctr = 0;
                for (var i = 0; i < limit; i++)
                    if (data(a, i) != data(b, i))
                        ctr++;
                return ctr; ;
            }
        }
    }
}
