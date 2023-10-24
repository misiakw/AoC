using AoC.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day1 : IDay<object, LegacyInput>
    {
        public LegacyInput[] GetTests()
        {
            var tests = new List<LegacyInput>();
            var example = new LegacyInput($"./Inputs/Day1/output.txt", "output");
            example.Tests[0] = LegacyInput.TestType.Verbal;
            example.Result[0] = Tuple.Create((object)71471, typeof(int));
            example.Tests[1] = LegacyInput.TestType.Verbal;
            example.Result[1] = Tuple.Create((object)211189, typeof(int));
            tests.Add(example);

            return tests.ToArray();
        }

        public object Part1(LegacyInput input)
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

        public object Part2(LegacyInput input)
        {
            var elves = (IList<int>) (input?.Cache ?? new List<int>());
            return elves.OrderByDescending(x => x).Take(3).Sum();
        }

        public IRuntime GetRuntime() => new Runtime<object, LegacyInput>(this);
    }
}
