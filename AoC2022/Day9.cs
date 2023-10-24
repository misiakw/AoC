using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day9 : LegacyDayBase
    {
        public Day9() : base(9)
        {
            Input("example1")
                .RunPart(1, 13)
                .RunPart(2, 1)
            .Input("example2")
                .RunPart(2, 36)
            .Input("output")
                .RunPart(1, 6175)
                .RunPart(2, 2578);
        }

        public override object Part1(LegacyInput input)
        {
            var rope = new Rope(1);

            foreach(var line in input.Lines){
                var parts = line.Split(" ").ToArray();
                rope.Move(parts[0].ToLower()[0], int.Parse(parts[1]));
            }
            return rope.TailMoveCount;
        }

        public override object Part2(LegacyInput input)
        {
            var rope = new Rope(9);

            foreach(var line in input.Lines){
                var parts = line.Split(" ").ToArray();
                rope.Move(parts[0].ToLower()[0], int.Parse(parts[1]));
            }
            return rope.TailMoveCount;
        }

        private class Rope{
            private Point Head;
            private Point Tail;
            private Point[] knots;
            private Dictionary<string, int> TailMoves = new Dictionary<string, int>();

            public Rope(int knots = 1){
                this.knots = new Point[knots+1];
                for(var i = 0; i<=knots; i++)
                    this.knots[i] = new Point(0,0);
                Head = this.knots[0];
                Tail = this.knots[knots];
            }

            public void Move(char dir, int steps){
                //Console.WriteLine($"Move {dir} {steps}");
                var dx = 0;
                var dy = 0;
                switch (dir){
                    case 'u':
                        dy = 1;
                        break;
                    case 'd':
                        dy = -1;
                        break;
                    case 'l':
                        dx = -1;
                        break;
                    case 'r':
                        dx = 1;
                        break;
                }
                for(var i=0; i<steps; i++){
                    Head.Move(dx, dy);
                    MoveTail();
                }
            }
            private void MoveTail(){
                for(var i=1; i<knots.Length; i++){
                    var front = knots[i-1];
                    var end = knots[i];

                    var dx = front.X - end.X;
                    var dy = front.Y - end.Y;
                    if(Math.Abs(dx)>1 || Math.Abs(dy)>1){
                        if (dx != 0){ //move horizontal
                            end.Move(dx > 0 ? 1: -1, 0);
                        }
                        if(dy != 0){//move vertical
                            end.Move(0, dy > 0 ? 1: -1);
                        }
                    }
                }
               //Console.WriteLine($"MoveTail {dx} {dy} {Head} {Tail}");
                if (TailMoves.ContainsKey(Tail.ToString())){
                    TailMoves[Tail.ToString()] ++;
                }else{
                    TailMoves.Add(Tail.ToString(), 1);
                }
            }
            public long TailMoveCount => TailMoves.Where(kv => kv.Value != 0).Count();
        }
    }
}
