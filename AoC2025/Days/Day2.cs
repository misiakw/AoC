using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Days
{
    internal class Day2: IDay
    {
        public static void RunAoC()
        {
            AocRuntime.Day<Day2>(2)
                .Callback(1, (d, t) => d.Part1(t.GetLines()))
                //.Callback(2, (d, t) => d.Part2(t.GetLines()))
                .Test("example", "Inputs/Day2/example.txt")
                .Part(1)
                //.Part(2)
                .Test("input", "Inputs/Day2/input.txt")
                .Part(1)
                //.Part(2)
                .Run();
        }

        private string Part1(IEnumerable<string> lines)
        {
            var ranges = lines.First().Split(',').ToList();
            var sum = 0l;
            foreach (var range in ranges)
            {
                var ends = range.Split('-').Select((long.Parse)).ToArray();
                var start = TwiceNumber.GetNext(ends[0], 2);

                while (start.Value <= ends[1])
                    sum += start++.Value;

            }
            return sum.ToString();
        }
        private string Part2(IEnumerable<string> lines)
        {
            return null;
        }

        private class TwiceNumber
        {
            private long Number { get; init; }
            private int Multiplier { get; init; }
            private int Repeat { get; init; }

            public long Value
            {
                get
                {
                    var result = Number;
                    for (var i = 1; i++ < Repeat;)
                        result = result * Multiplier + Number;
                    return result;
                }
            }

            public static TwiceNumber operator +(TwiceNumber number, long add)
                => new TwiceNumber
                {
                    Number = number.Number + add,
                    Multiplier = number.Number + add < number.Multiplier ? number.Multiplier : number.Multiplier * 10,
                    Repeat = number.Repeat
                };

            public static TwiceNumber operator ++(TwiceNumber number)
                => number + 1;

            private static int[] tenBase = { 1, 10, 100, 1000, 10000, 100000, 1000000, 1000000, 10000000, 10000000, 1000000000 };
            public static TwiceNumber GetNext(long number, int repeat)
            {
                var numLen = (int)Math.Log10(number) +1;

                if(numLen%repeat == 0)
                {
                    var len = numLen/repeat;
                    var mul = tenBase[len*(repeat-1)];

                    var p1 = number / (mul);
                    var p2 = (number % mul) / (mul / tenBase[len]);

                    return new TwiceNumber
                    {
                        Number = p1 >= p2 ? p1 : p1 + 1,
                        Multiplier = mul,
                        Repeat = repeat
                    };
                }
                else
                {
                    var len = numLen / repeat + 1;
                    return new TwiceNumber
                    {
                        Number = tenBase[len-1],
                        Multiplier = tenBase[len],
                        Repeat = repeat
                    };
                }
            }

            public override string ToString()
            {
                var result = "";
                for (var i = 1; i++ < Repeat;)
                    result += $"{Number},";
                return result + Number.ToString();
            }
        }
    }
}
