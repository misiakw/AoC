using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Day2
{
    [Day("Day2")]
    [Input("test", typeof(Day2TestInput))]
    [Input("aoc", typeof(Day2TargerInput))]
    public class Day2: IDay
    {
        public string Task1(IInput input)
        {
            var prog = new Program(input.Input, 12, 2);

            while (prog.CanNext())
                prog.Next();

            return prog.Result.ToString();
        }

        public string Task2(IInput input)
        {
            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    var prog = new Program(input.Input, noun, verb);

                    while (prog.CanNext())
                        prog.Next();

                    if (prog.Result == 19690720)
                        return ((100 * noun) + verb).ToString();
                }
            }

            return string.Empty;
        }


        private class Program
        {
            private long[] mem;
            private int pos = 0;

            public long Result => mem[0];

            public Program(string input, int noun, int verb)
            {
                mem = input.Split(",")
                    .Select(x => x.Trim())
                    .Select(long.Parse)
                    .ToArray();

                mem[1] = noun;
                mem[2] = verb;
            }

            public bool CanNext()
            {
                return mem[pos] != 99;
            }

            public void Next()
            {
                var posA = mem[pos + 1];
                var posB = mem[pos + 2];
                var dst = mem[pos + 3];

                switch (mem[pos])
                {
                    //Add
                    case 1:
                        mem[dst] = mem[posA] + mem[posB];
                        break;
                    //mul
                    case 2:
                        mem[dst] = mem[posA] * mem[posB];
                        break;
                }

                pos += 4;
            }
        }
    }
}