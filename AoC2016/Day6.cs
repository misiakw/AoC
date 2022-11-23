using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using Step = AoC2016.Days.Day1.Step;

namespace AoC2016
{
    public class Day6 : DayBase
    {
        public Day6() : base(6)
        {
            Input("example1")
                .RunPart(1, "easter")
                .RunPart(2, "advent")
            .Input("output")
                .RunPart(1, "tzstqsua")
                .RunPart(2, "myregdnr");
        }

        public override object Part1(Input input)
        {
            var stats = new Dictionary<char, int>[input.Lines[0].Length];
            for(var i=0; i<stats.Length; i++)
                stats[i] = new Dictionary<char, int>();

            foreach(var line in input.Lines){
                for(var i=0; i<line.Length; i++){
                    if(stats[i].ContainsKey(line[i]))
                        stats[i][line[i]]++;
                    else
                        stats[i][line[i]] = 1;
                }
            }

            input.Cache = stats;

            var result = string.Empty;
            foreach(var dict in stats){
                result += dict.OrderByDescending(d => d.Value).First().Key;
            }
            return result;
        }

        public override object Part2(Input input)
        {
             var stats = (Dictionary<char, int>[])input.Cache;

             var result = string.Empty;
            foreach(var dict in stats){
                result += dict.OrderBy(d => d.Value).First().Key;
            }
            return result;
        }
    }
}
