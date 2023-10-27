using AoC.Base;
using AoC.Base.TestInputs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day1 : AbstractDay<object, LegacyInput>
    {
        public override void PrepateTests(InputBuilder<object, LegacyInput> builder)
        {
            builder.New("./Inputs/Day1/output.txt", "output")
                .Part1(71471)
                .Part2(211189);
        }

        public override object Part1(LegacyInput input)
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

        public override object Part2(LegacyInput input)
        {
            var elves = (IList<int>) (input?.Cache ?? new List<int>());
            return elves.OrderByDescending(x => x).Take(3).Sum();
        }
    }
}
