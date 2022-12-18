using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day4 : DayBase
    {
        public Day4() : base(4)
        {
            Input("example1")
                .RunPart(1, 2)
                .RunPart(2, 4)
            .Input("output")
                .RunPart(1, 528)
                .RunPart(2, 881);
        }

        public override object Part1(Input input)
        {
            IDictionary<int, IList<int>> dict = new Dictionary<int, IList<int>>();
            var containing = 0;
            var overlapping = 0;

            foreach(var line in input.Lines){
                var elves = line.Split(",")
                    .Select(l => l.Split("-")
                        .Select(d => int.Parse(d))
                            .ToArray())
                        .ToArray();

                var range1 = new List<int>();
                for(var x=elves[0][0]; x<=elves[0][1]; x++)
                    range1.Add(x);

                var range2 = new List<int>();
                for(var x=elves[1][0]; x<=elves[1][1]; x++)
                    range2.Add(x);

                var overlapped = range1.Intersect(range2).Count();
                if(overlapped > 0)
                    overlapping ++;
                if(overlapped == range1.Count() || overlapped == range2.Count())
                    containing++;
            }

            input.Cache = overlapping;

            return containing;
        }

        public override object Part2(Input input)
        {
            return (int)(input.Cache ?? 0);
        }
    }
}
