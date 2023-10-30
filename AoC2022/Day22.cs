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
            builder.New("example1", "./Inputs/Day22/example1.txt")
                .Part1(6032);
            //builder.New("output", "./Inputs/Day22/output.txt")
            //    .Part2(0); //75290 Too High
        }

        public override int Part1(IComparableInput<int> input)
        {
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
            private readonly IList<string> Data = new List<string>();
            private readonly Queue<int> Steps = new Queue<int>();
            private Dir dir = Dir.Right;

            public Map(IComparableInput<int> input) {
                var t = input.ReadLines();
                t.Wait();

                foreach (var line in t.Result) {
                    if (string.IsNullOrEmpty(line))
                        break;
                    if (Height == 0)
                    {
                        for (var i = 0; i < line.Length; i++)
                            if (line[i] != ' ')
                            {
                                pX = i;
                                break;
                            }
                    }
                    Data.Add(line);
                    Height++;
                    if (Width < line.Length)
                        Width = line.Length;
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

                switch (dir)
                {
                    case Dir.Right:
                        MarchRight(step);
                        break;
                    case Dir.Left:
                        MarchLeft(step);
                        break;
                    case Dir.Up:
                        MarchUp(step);
                        break;
                    case Dir.Down:
                        MarchDown(step);
                        break;
                }
                foreach (var line in Data)
                    Console.WriteLine(line);

            }

            private void March(int steps, Func<char> nextChar, Func<bool> eol, Action proceed, Func<bool> rewind, char printCh)
            {
                while (steps > 0)
                {
                    //jesli koniec przewiń na drugą stronę
                    if (eol())
                    {
                        if (!rewind()) //przeskocz na kolejny
                            return; //co jesli rewind w ściane, zakończ
                        continue;
                    }
                    var ch = nextChar();
                    if (ch == '#')//jeśli sciana
                        return; //zakoncz
                    proceed();
                    steps--;
                    var tmp = new StringBuilder(Data[pY]);
                    tmp[pX] = printCh;
                    Data[pY] = tmp.ToString();
                }
            }

            private bool RewindHorizontal(int progress, Func<string, int, bool> canRewind)
            {
                var line = Data[pY];
                var tmp = pX;
                while (canRewind(line, tmp))
                    tmp -= progress;
                if (line[tmp + progress] == '#')
                    return false;
                pX = tmp;
                return true;
            }
            private void MarchRight(int steps)
                => March(steps,
                    () => Data[pY][pX + 1],
                    () => pX + 1 >= Data[pY].Length || Data[pY][pX + 1] == ' ',
                    () => pX++,
                    () => RewindHorizontal(1, (line, tmp) => tmp >= 0 && line[tmp] != ' '),
                    '>');
            private void MarchLeft(int steps)
                => March(steps,
                    () => Data[pY][pX - 1],
                    () => pX - 1 < 0 || Data[pY][pX - 1] == ' ',
                    () => pX--,
                    () => RewindHorizontal(-1, (line, tmp) => tmp < line.Length),
                    '<');

            private bool RewindVertical(int progress, Func<int, bool> canRewind)
            {
                var tmp = pY;
                while (canRewind(tmp))
                    tmp -= progress;
                if (Data[tmp][pX] == '#')
                    return false;
                pY = tmp;
                return true;
            }
            private void MarchUp(int steps)
                => March(steps,
                    () => Data[pY-1][pX],
                    () => pY-1 < 0 || Data[pY-1][pX] == ' ',
                    () => pY--,
                    () => RewindVertical(-1, (tmp) => tmp >= 0 && Data[tmp][pX] != ' '),
                    '^');
            private void MarchDown(int steps)
                => March(steps,
                    () => Data[pY + 1][pX],
                    () => pY - 1 > Height || Data[pY + 1][pX] == ' ',
                    () => pY++,
                    () => RewindVertical(1, (tmp) => tmp >= 0 && Data[tmp][pX] != ' '),
                    'v');

            public int GetScore()
                => 1000 * (pY + 1) + 4 * (pX + 1) + dir == Dir.Right ? 0 : dir == Dir.Down ? 1 : dir == Dir.Left ? 2 : 3;
        }

        private enum Dir{
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3,
        }
    }
}
