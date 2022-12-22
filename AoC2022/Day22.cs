using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day22 : DayBase
    {
        public Day22() : base(22)
        {
            Input("example1")
                .RunPart(1, 5)
            .Input("output");
        }

        public override object Part1(Input input)
        {
            var tmp = ReadInput(input);
            throw new NotImplementedException();
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        private Tuple<Array2D<char>, List<string>> ReadInput(Input input)
        {
            var map = new Array2D<char>(' ');

            var y = 0;
            foreach (var line in input.Lines)
            {
                var x = 0;
                foreach (var ch in line)
                {
                    if (ch != ' ')
                        map[x, y] = ch;
                    x++;
                }
                y++;
            }

            var steps = new List<string>();
            var tmp = string.Empty;
            foreach (var ch in input.Lines.Last())
            {
                tmp += ch;
                if (ch == 'L' || ch == 'R')
                {
                    steps.Add(tmp);
                    tmp = string.Empty;
                }
            }

            return Tuple.Create(map, steps);
        }
    }
}
