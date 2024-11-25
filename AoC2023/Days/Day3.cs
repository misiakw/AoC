using System.Collections.Generic;
using System.Linq;
using AoCBase2;


namespace AoC2023.Days
{
    public class Day3
    {
        public static void ProceedAoC()
        {
            AocRuntime.Day<Day3>(3)
                .Callback(1, (d, t) => d.Part1(t.GetLinesAsync()))
                .Callback(2, (d, t) => d.Part2(t.GetLinesAsync()))
                .Test("example1", "./Inputs/Day3/example1.txt")
                    .Part(1).Correct(4361)
                    .Part(2).Correct(467835)
                .Test("output", "./Inputs/Day3/output.txt")
                    .Part(1).Correct(527369)
                    .Part(2).Correct(73074886)
                .Run();
        }

            public string Part1(IAsyncEnumerable<string> input)
        {
            var inp = RedInput(input);
            var symbols = inp.Item1;
            var nums = inp.Item2;

            var partNums = nums.Where(n => symbols.Any(s => n.IsAdjascent(s))).ToList();

            return partNums.Sum(p => p.Val).ToString();
        }

        public string Part2(IAsyncEnumerable<string> input)
        {
            var inp = RedInput(input);
            var stars = inp.Item1.Where(s => s.Char == '*').ToList();
            var nums = inp.Item2;

            var result = 0;
            foreach(var sym in stars)
            {
                var parts = nums.Where(n => n.IsAdjascent(sym)).ToList();
                if (parts.Count == 2)
                    result += parts[0].Val * parts[1].Val;
            }

            return result.ToString();
        }

        private (IList<Symbol>, IList<Num>) RedInput(IAsyncEnumerable<string> input)
        {
            var y = 0;
            var x = 0;
            var symbols = new List<Symbol>();
            var nums = new List<Num>();
            foreach (var line in input.ToEnumerable())//ReadLines(input))
            {
                var numStr = string.Empty;
                x = 0;
                foreach (var ch in line)
                {
                    if (ch >= '0' && ch <= '9')
                        numStr += ch;
                    else
                    {
                        if (!string.IsNullOrEmpty(numStr))
                        {
                            nums.Add(new Num(x - 1, y, numStr));
                            numStr = string.Empty;
                        }
                        if (ch != '.')
                            symbols.Add(new Symbol(x, y, ch));
                    }
                    x++;
                }
                if (!string.IsNullOrEmpty(numStr))
                {
                    nums.Add(new Num(x - 1, y, numStr));
                    numStr = string.Empty;
                }
                y++;
            }
            return (symbols, nums);
        }

        private class Symbol
        {
            public readonly int X, Y;
            public readonly char Char;
            public Symbol(int x, int y, char ch)
            {
                this.X = x;
                this.Y = y;
                this.Char = ch;
            }
        }

        private class Num
        {
            public readonly int minX, maxX, minY, maxY, Val;
            public Num(int x, int y, string val)
            {
                this.Val = int.Parse(val);
                this.maxX = x+1;
                this.minX = x-val.Length;
                this.minY = y-1;
                this.maxY = y + 1;
            }

            public bool IsAdjascent(Symbol sym)
            {
                if (sym.X >= this.minX  && sym.X <= this.maxX )
                    if (sym.Y >= this.minY  && sym.Y <= this.maxY)
                        return true;
                return false;
            }
        }
    }
}
