using AoC.Base;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day6 : LegacyDayBase
    {
        public Day6() : base(6)
        {
            Input("example1")
                .RunPart(1, 7)
                .RunPart(2, 19)
            .Input("example2")
                .RunPart(1, 5)
                .RunPart(2, 23)
            .Input("example3")
                .RunPart(1, 6)
                .RunPart(2, 23)
            .Input("example4")
                .RunPart(1, 10)
                .RunPart(2, 29)
            .Input("example5")
                .RunPart(1, 11)
                .RunPart(2, 26)
            .Input("output")
                .RunPart(1, 1760)
                .RunPart(2);
        }

        public override object Part1(LegacyInput input)
        {
            var ctr = 0;
            var window = new Queue<char>();

            foreach(var ch in input.Raw.Trim()){
                ctr++;
                window.Enqueue(ch);
                if(window.Count() == 4){
                    if(window.Distinct().Count() == 4)
                        return ctr;
                    window.Dequeue();
                }
            }

            return -1;
        }

        public override object Part2(LegacyInput input)
        {
            var ctr = 0;
            var window = new Queue<char>();

            foreach(var ch in input.Raw.Trim()){
                ctr++;
                window.Enqueue(ch);
                if(window.Count() == 14){
                    if(window.Distinct().Count() == 14)
                        return ctr;
                    window.Dequeue();
                }
            }

            return -1;
        }
    }
}
