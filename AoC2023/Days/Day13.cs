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
    public class Day13 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day13/example1.txt")
               .Part1(405)
               .Part2(400);
            builder.New("output", "./Inputs/Day13/output.txt")
                .Part1(27300);
            //.Part2(790194712336);
        }
        public override long Part1(IComparableInput<long> input)
        {
            var arrays = GetCharArrays(input, 0);
            return arrays.Sum(arr => arr.GetScore());
        }

        public override long Part2(IComparableInput<long> input)
        {
            var arrays = GetCharArrays(input, 1);
            return arrays.Sum(arr => arr.GetScore());
        }

        private IEnumerable<CharArray> GetCharArrays(IComparableInput<long> input, int smugesToFix)
        {
            var lines = ReadLines(input);
            var map = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return new CharArray(map, smugesToFix);
                    map.Clear();
                }
                else
                    map.Add(line);
            }
            yield return new CharArray(map, smugesToFix);
        }

        private class CharArray
        {
            public readonly int Width, Height;
            public char[,] Map;
            private int smugesToFix = 0;

            public CharArray(IList<string> lines, int smugesToFix)
            {
                this.smugesToFix = smugesToFix;
                Width = lines[0].Length;
                Height = lines.Count();
                Map = new char[Width, Height];
                for (var y = 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        Map[x, y] = lines[y][x];
            }

            public int GetScore()
            {
                var horizontal = SearchForMirrorRows();// -
                var vertical = SearchForMirrorCollumns(); // |

                return (vertical > 0 ? vertical : 0) + 100 * (horizontal > 0 ? horizontal : 0);
            }

            private int SearchForMirrorCollumns() => SearchForMirror(true);
            private int SearchForMirrorRows() => SearchForMirror(false);
            private int SearchForMirror(bool isCollumn)
            {
                var eqLimit = isCollumn ? Height : Width;
                var searchLimit = isCollumn ? Width : Height;
                var dataFunc = isCollumn
                    ? new Func<int, int, char>((x, i) => Map[x, i])
                    : new Func<int, int, char>((y, i) => Map[i, y]);
                var fixFunc = isCollumn
                    ? new Action<int, int, int>((trgt, src, i) => Map[trgt, i] = Map[src, i])
                    : new Action<int, int, int>((trgt, src, i) => Map[i, trgt] = Map[i, src]);

                for (var i = 0; i < searchLimit - 1; i++)
                {
                    var isMirrored = true;
                    for (var j = 0; i - j >= 0 && i + j + 1 < searchLimit; j++)
                    {
                        var smuges = SmugeCount(i - j, i + j + 1, dataFunc, eqLimit);
                        if (smuges <= smugesToFix && smuges > 0)
                        {
                            Console.WriteLine(Draw());
                            FixSmuge(i - j, i + j + 1, dataFunc, eqLimit, fixFunc);
                            Console.WriteLine($"Fix {(isCollumn ? "collumn" : "row")} {i + j + 1} into {i - j}");
                            Console.WriteLine(Draw());
                            smugesToFix -= smuges;
                            if(smugesToFix == 0)
                            {
                                i = -1;
                                isMirrored = false;
                                break;
                            }
                        }
                        if (smuges > smugesToFix)
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

            private void FixSmuge(int trgt, int src, Func<int, int, char> data, int limit, Action<int, int, int> change)
            {
                for(var i=0; i<limit; i++)
                {
                    if (data(trgt, i) != data(src, i))
                        change(trgt, src, i);
                }
            }

            public string Draw()
            {
                var sb = new StringBuilder();
                for(var y=0; y<Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                        sb.Append(Map[x, y]);
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }
    }
}
