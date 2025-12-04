using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Days
{
    internal class Day3 : IDay
    {
        public static void RunAoC()
        {
            AocRuntime.Day<Day3>(3)
                .Callback(1, (d, t) => d.Part1(t.GetLines()))
                .Callback(2, (d, t) => d.Part2(t.GetLines()))
                .Test("example", "Inputs/Day3/example1.txt")
                //.Part(1)
                //.Part(2)
                .Test("input", "Inputs/Day3/input.txt")
                //.Part(1)
                //.Part(2)
                .Run();
        }

        private string Part1(IEnumerable<string> lines)
             => Process(2, lines).ToString();
        private string Part2(IEnumerable<string> lines)
             => Process(12, lines).ToString();

        private long Process(int len, IEnumerable<string> lines)
        {
            var sum = 0l;
            foreach(var input in lines) 
            {
                var line = input.Select(c => (long)(c - '0')).ToArray();
                var size = 0l;

                for (var i = len; i-- > 0;)
                {
                    line = TrimShit(line, i);
                    size = size * 10 + line[0];
                    line = line.Skip(1).ToArray();
                }
                sum += size;
            }
            return sum;
        }

        public long[] TrimShit(long[] shit, int skips)
        {
            var max = shit.SkipLast(skips).Max(c => c);
            var index = 0;
            while (shit[index++] != max) ;
            return shit.Skip(--index).ToArray();
        }
    }
}
