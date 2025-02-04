using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day4
{
    [Day("Day4")]
    [Input("AOC", typeof(Day4Input))]
    public class Day4 : IDay
    {
        private IList<string> codes = new List<string>();
        public string Task1(IInput input)
        {
            var range = input.Input.Split("-").Select(long.Parse).ToArray();
            var possibles = new List<string>();
            for(var i=range[0]; i<=range[1]; i++)
            {
                var test = i.ToString();
                if (test.GroupBy(c => c).Count() < 6)
                {
                    for (var t = 0; t < 5; t++)
                    {
                        if (test[t + 1] < test[t])
                            break;
                        if (t == 4)
                            possibles.Add(test);
                    }
                }
            }

            foreach(var possible in possibles)
            {
                for (var t = 0; t < 5; t++)
                {
                    if (possible[t + 1] == possible[t])
                    {
                        codes.Add(possible);
                        break;
                    }
                }
            }

            return codes.Count().ToString();
        }



        public string Task2(IInput input)
        {
            var result = new List<string>();
            foreach(var test in codes)
            {
                if(test.GroupBy(d => d).Select(g => g.Count()).Any(c => c == 2))
                {
                    result.Add(test);
                }
            }

            return result.Count().ToString();
        }
    }
}
