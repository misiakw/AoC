using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2022
{
    public class Day22 : AbstractDay<int, IComparableInput<int>>
    {
        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            //builder.New("test", "./Inputs/Day22/test.txt")
            //    .Part1(6032);
            builder.New("example1", "./Inputs/Day22/example1.txt")
                .Part1(6032);
            builder.New("output", "./Inputs/Day22/output.txt")
                .Part1(0); //116100 too high; 130324 too high
            //    .Part2(0); //75290 Too High
        }

        public override int Part1(IComparableInput<int> input)
        {
            Console.WriteLine((5 + 6) % 10);
            Console.WriteLine((5 - 6) % 10);

            var map = new Map(input);

            while (map.CanProsess())
                map.Process();
            
            return map.GetScore();
        }

        public override int Part2(IComparableInput<int> input)
        {
            throw new NotImplementedException();
        }

        private class Map {
            public readonly int Height = 0;
            public readonly int Width = 0;
            public int pX = 0;
            public int pY = 0;
            private readonly char[,] Data;
            private readonly Queue<int> Steps = new Queue<int>();
            private Dir dir = Dir.Right;

            public Map(IComparableInput<int> input) {
                var t = input.ReadLines();
                t.Wait();
                var lines = t.Result.ToArray();
                Width = lines.Select(l => l.Length).Max();
                Height = lines.Length - 2;

                Data = new char[Width, Height];

                for(var y= 0; y < Height; y++)
                    for (var x = 0; x < Width; x++)
                        Data[x, y] = ' ';

                Height = 0;
                foreach (var line in lines) {
                    if (string.IsNullOrEmpty(line))
                        break;
                   
                    for (var x = 0; x < line.Length; x++)
                            Data[x, Height] = line[x];

                    Height++;
                }

                for(var i=0; i<Width; i++)
                    if(Data[i, 0] == '.')
                    {
                        pX = i;
                        break;
                    }

                int steps = 0;
                foreach (var ch in t.Result.Last().ToCharArray())
                {
                    if (ch == 'L' || ch == 'R')
                    {
                        if (steps > 0)
                            Steps.Enqueue(steps);
                        steps = 0;
                        Steps.Enqueue(ch == 'L' ? -1 : -2);
                    }
                    else
                        steps = steps * 10 + (int)(ch - '0');
                }
                if (steps > 0)
                    Steps.Enqueue(steps);
            }

            public void Print()
            {
                for (var y = 0; y < Height; y++)
                {
                    var sb = new StringBuilder();
                    for (var x = 0; x < Width; x++)
                        if(x == pX && y == pY)
                            sb.Append("@");
                        else
                            sb.Append(Data[x, y]);
                    Console.WriteLine(sb.ToString());
                }
            }

            public bool CanProsess() => Steps.Any();

            public void Process()
            {
               var step = Steps.Dequeue();


                Console.WriteLine($"======== Step: {(step >= 0 ? step : step == -1 ? "CCW" : "CW")} ========");

                if (step < 0)
                {
                    var dirInt = (int)dir + (step == -1 ? -1 : 1);
                    dir = (Dir)(dirInt < 0 ? (4 - dirInt) : dirInt % 4);
                    return;
                }
                var dx = dir == Dir.Right ? 1 : dir == Dir.Left ? -1 : 0;
                var dy = dir == Dir.Down ? 1 : dir == Dir.Up ? -1 : 0;

                //o ile są kroki do zrobienia
                while (step-- > 0)
                {
                    var nX = (pX + dx) % Width;
                    var nY = (pY + dy) % Height;
                    if (nX < 0) nX += Width;
                    if (nY < 0) nY += Height;

                    while (Data[nX, nY] == ' ')
                    {
                        nX = (nX + dx) % Width;
                        nY = (nY + dy) % Height;
                        if (nX < 0) nX += Width;
                        if (nY < 0) nY += Height;
                    }

                    if (Data[nX, nY] == '#')
                        break;
                    pX = nX;
                    pY = nY;
                }

               Print();
            }
            public int GetScore()
                => (1000 * (pY+1)) + (4 * (pX+1)) + (dir == Dir.Right ? 0 : dir == Dir.Down ? 1 : dir == Dir.Left ? 2 : 3);
        }

        private enum Dir{
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3,
        }
    }
}
