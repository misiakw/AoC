using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using Step = AoC2016.Days.Day1.Step;

namespace AoC2016
{
    public class Day1 : Day<Step>
    {
        public Day1() : base(1, false)
        {
            Input("example1")
                .RunPart(1, 5)
            .Input("example2")
                .RunPart(1, 2)
            .Input("example3")
                .RunPart(1, 12)
            .Input("example4")
                .Silent(1)
                .RunPart(2, 4)
            .Input("output")
                .RunPart(1, 226)
                .RunPart(2, 79);
        }

        public override Step Parse(string val) => new Step{
                Rot = val[0],
                Len = int.Parse(val.Substring(1))
            };

        public override IList<string> Split(string val) => val.Split(",");
        public override object Part1(IList<Step> data, Input input)
        {
            var compass = new Compass();
            foreach(var step in data)
                compass.processStep(step);

            input.Cache = compass;
            return compass.Dist;
        }


        public override object Part2(IList<Step> data, Input input)
        {
            var compass = (Compass) input.Cache;

            return compass?.CrossDist?.FirstOrDefault();
        }

        private class Compass{
            private int X, Y;
            private char dir = 'N';
            private Array2D<bool> map = new Array2D<bool>();
            public IList<int> CrossDist = new List<int>();

            public int Dist {
                get =>  Math.Abs(X) + Math.Abs(Y);
            }

            public void processStep(Step step){
                Rotate(step.Rot);

                switch(dir){
                    case 'N': processStep(1, 0, step.Len); break;
                    case 'S': processStep(-1, 0, step.Len); break;
                    case 'E': processStep(0, 1, step.Len); break;
                    case 'W': processStep(0, -1, step.Len); break;
                }                
            }
            private void processStep(int dx, int dy, int len){
                for(var i =0; i < len; i++){
                    if(map[X,Y])
                        CrossDist.Add(Dist);
                    map[X, Y] = true;
                    X += dx;
                    Y += dy;
                }
            }
            private void Rotate(char rot){
                if(rot == 'R')
                    switch (dir){
                        case 'N': dir = 'E'; break;
                        case 'E': dir = 'S'; break;
                        case 'S': dir = 'W'; break;
                        case 'W': dir = 'N'; break;
                    }
                else
                    switch (dir){
                        case 'N': dir = 'W'; break;
                        case 'E': dir = 'N'; break;
                        case 'S': dir = 'E'; break;
                        case 'W': dir = 'S'; break;
                    }
            }
        }
    }
}
namespace AoC2016.Days.Day1{
    public class Step{
        public char Rot;
        public int Len;
    }
}
