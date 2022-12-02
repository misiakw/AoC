using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;

namespace AoC2022
{
    public class Day1 :  DayBase
    {
        public Day1() : base(1)
        {
            Input("example")
                .RunPart(1, 24000)
                .RunPart(2, 45000)
            .Input("output")
                .RunPart(1, 71471)
                .RunPart(2, 211189);
        }

        public override object Part1(Input input)
        {
            var elves = new List<int>();
            var current = 0;
            foreach(var line in input.Lines){
                if(string.IsNullOrEmpty(line)){
                    elves.Add(current);
                    current = 0;
                }else{
                    current += int.Parse(line);
                }
            }
            elves.Add(current);

            input.Cache = elves;

            return elves.OrderByDescending(x => x).First();
        }

        public override object Part2(Input input)
        {
            var elves = (IList<int>) input.Cache;
            return elves.OrderByDescending(x => x).Take(3).Sum();
        }
    }
}
