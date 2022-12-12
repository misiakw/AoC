using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;
using AoC2022.Days.Day12;

namespace AoC2022
{
    public class Day12 : DayBase
    {
        public Day12() : base(12)
        {
            Input("example1")
                .RunPart(1, 31)
                .RunPart(2, 29)
            .Input("output")
                .RunPart(1, 391)
                .RunPart(2, 386);
        }

        public override object Part1(Input input)
        {
            var map = new Array2D<byte>(byte.MaxValue);
            Point start = null;
            Point end = null;
            for(var y=0; y<input.Lines.Count(); y++)
                for(var x=0; x<input.Lines[y].Length; x++)
                    if (input.Lines[y][x] == 'S'){
                        start = new Point(x, y);
                        map[x, y] = 0;
                    }else if (input.Lines[y][x] == 'E'){
                        end = new Point(x, y);
                        map[x, y] = 'z'-'a';
                    }else{
                        map[x, y] = (byte)(input.Lines[y][x]-'a');
                    }

            var result = new Array2D<int>(int.MaxValue);
            result.PopulateLowestPath(start.X, start.Y, 0, map);

            return result[end];
        }

        public override object Part2(Input input)
        {
            var map = new Array2D<byte>(byte.MaxValue);
            Point start = null;
            var ends = new List<Point>();
            for(var y=0; y<input.Lines.Count(); y++)
                for(var x=0; x<input.Lines[y].Length; x++)
                    if (input.Lines[y][x] == 'S' || input.Lines[y][x] == 'a'){
                        ends.Add(new Point(x, y));
                        map[x, y] = 0;
                    }else if (input.Lines[y][x] == 'E'){
                        start = new Point(x, y);
                        map[x, y] = 'z'-'a';
                    }else{
                        map[x, y] = (byte)(input.Lines[y][x]-'a');
                    }

            var result = new Array2D<int>(int.MaxValue);
            result.PopulateReversePath(start.X, start.Y, 0, map);

            return ends.Select(e => result[e]).Min();
        }
    }
}
namespace AoC2022.Days.Day12{
    public static class Extensions{
        public static void PopulateLowestPath(
            this Array2D<int> me, 
            long x, 
            long y, 
            int stepsSoFar, 
            Array2D<byte> map
        ){
            if(me[x, y] <= stepsSoFar)
                return;
            
            me[x, y] = stepsSoFar;
            
            if(map[x-1, y]-map[x, y] <= 1)
                me.PopulateLowestPath(x-1, y, stepsSoFar+1, map);
            if(map[x+1, y]-map[x, y] <= 1)
                me.PopulateLowestPath(x+1, y, stepsSoFar+1, map);
            if(map[x, y-1]-map[x, y] <= 1)
                me.PopulateLowestPath(x, y-1, stepsSoFar+1, map);
            if(map[x, y+1]-map[x, y] <= 1)
                me.PopulateLowestPath(x, y+1, stepsSoFar+1, map);
        }

        public static void PopulateReversePath(
            this Array2D<int> me, 
            long x, 
            long y, 
            int stepsSoFar, 
            Array2D<byte> map
        ){
            if(me[x, y] <= stepsSoFar || map[x, y] == byte.MaxValue)
                return;

            me[x, y] = stepsSoFar;
            if(map[x, y] == 0)
                return;


            if(map[x, y] - map[x-1, y] <= 1)
                me.PopulateReversePath(x-1, y, stepsSoFar+1, map);
            if(map[x, y] - map[x+1, y] <= 1)
                me.PopulateReversePath(x+1, y, stepsSoFar+1, map);
            if(map[x, y] - map[x, y-1] <= 1)
                me.PopulateReversePath(x, y-1, stepsSoFar+1, map);
            if(map[x, y] - map[x, y+1] <= 1)
                me.PopulateReversePath(x, y+1, stepsSoFar+1, map);
        }
    }
}
