using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day10 : DayBase
    {
        public Day10() : base(10)
        {
            Input("example1")
                .RunPart(1, 13140)
            .Input("output")
                .RunPart(1, 14620)
                .RunPart(2, "BJFRHRFU");
        }

        public override object Part1(Input input)
        {
            var x = 1;
            int nextStop = 20;
            var signalStrength = 0;

            var tick = 1;
            foreach(var line in input.Lines){
                var change = 0;
                if(line == "noop")
                    PrintPoint(tick++, x);
                else{
                    PrintPoint(tick++, x);
                    PrintPoint(tick++, x);
                    change = int.Parse(line.Split(" ")[1]);
                    x+=change;
                }
                
                if (tick>=nextStop){
                    var val = (tick==nextStop? x: x-change);
                    signalStrength += nextStop * val;
                    nextStop+= 40;
                }
            }
            return signalStrength;
        }

        public override object Part2(Input input)
        {
            //result visible on console
            return "BJFRHRFU";
        }

        private void PrintPoint(int tick, int x){
            if(tick%40>=x && tick%40 <= x+2)
                Console.Write("#");
            else
                Console.Write(".");
            if(tick %40 == 0) Console.WriteLine($" <-- {tick}");
        }
    }
}
