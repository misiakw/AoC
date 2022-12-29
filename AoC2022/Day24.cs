﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day24 : DayBase
    {
        public Day24() : base(24)
        {
            Input("small")
                //.RunPart(1)
            .Input("example1")
                .RunPart(1, 18)
            .Input("output")
                .RunPart(1, 257);
        }

        public override object Part1(Input input){
            var solver = new Day24Solver(input);

            return solver.GoAhead(0, 0, solver.WaitToStart());
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        protected class Day24Solver{
            
            protected IDictionary<long, Dirs[,]> windMap = new Dictionary<long, Dirs[,]>();
            protected IDictionary<string, long> preTestedPaths = new Dictionary<string, long>();
            protected int rows;
            protected int cols;
            protected int topWindState = 0;
            protected long longestSoFar = 2000; //because reasons -it worked and int.maxvalue used too much memory...

            public Day24Solver(Input input)
        {
            rows = input.Lines.Count() -2;
            cols = input.Lines.First().Length-2;

            var baseWind = new Dirs[cols, rows];
            var y = 0;
            foreach(var line in input.Lines.Skip(1).Take(rows)){
                var x = 0;
                foreach(var ch in line.Substring(1, cols)){
                    switch(ch){
                        case '.':
                            baseWind[x, y] = Dirs.None;
                            break;
                        case '>':
                            baseWind[x, y] = Dirs.Right;
                            break;
                        case '<':
                            baseWind[x, y] = Dirs.Left;
                            break;
                        case '^':
                            baseWind[x, y] = Dirs.Up;
                            break;
                        case 'v':
                            baseWind[x, y] = Dirs.Down;
                            break;
                    }
                    x++;
                }
                y++;
            }
            windMap.Add(0, baseWind);
        }

            public long WaitToStart(){
            int time = 0;
            var map = GetWind(time);
            while(map[0,0] != Dirs.None){
                map = GetWind(++time);
            }
            return time;
        }

        public long GoAhead(int x, int y, long step, bool wasStationary = false){
            if(!wasStationary){
                if(x == cols-1 && y==rows){
                    if(step < longestSoFar) longestSoFar = step;
                    return step;
                }
                if(x<0 || y<0 || x== cols || y == rows)
                    return int.MaxValue;
            }
            var state = GetWind(step);
            if (step > longestSoFar || state[x, y] != Dirs.None)
                return int.MaxValue;
            var key = $"{x}|{y}|{step}";
            if(preTestedPaths.ContainsKey(key))
                return preTestedPaths[key];

            var results = new List<long>();

            results.Add(GoAhead(x+1, y, step+1));            
            results.Add(GoAhead(x-1, y, step+1));
            results.Add(GoAhead(x, y+1, step+1));
            results.Add(GoAhead(x, y-1, step+1));
            results.Add(GoAhead(x, y, step+1, true));

            var result = results.Min();
            preTestedPaths.Add(key, result);
            return result;
        }

        private Dirs[,] GetWind(long step){
            while(topWindState < step){
                var latest = windMap[topWindState];
                var newMap = new Dirs[cols, rows];
                for(var y = 0; y<rows; y++){
                    for(var x = 0; x<cols; x++){
                        if(latest[x, y] == Dirs.None)
                            continue;
                        
                        if(latest[x, y].HasFlag(Dirs.Up)){
                            if(y == 0)
                                newMap[x, rows-1] |= Dirs.Up;
                            else
                                newMap[x, y-1] |= Dirs.Up;
                        }
                        
                        if(latest[x, y].HasFlag(Dirs.Down)){
                            if(y == rows-1)
                                newMap[x, 0] |= Dirs.Down;
                            else
                                newMap[x, y+1] |= Dirs.Down;
                        }
                        
                        if(latest[x, y].HasFlag(Dirs.Left)){
                            if (x == 0)
                                newMap[cols-1, y] |= Dirs.Left;
                            else
                                newMap[x-1, y] |= Dirs.Left;
                        }
                        
                        if(latest[x, y].HasFlag(Dirs.Right)){
                            if(x == cols-1)
                                newMap[0, y] |= Dirs.Right;
                            else
                                newMap[x+1, y] |= Dirs.Right;
                        }                        
                    }
                }
                windMap.Add(++topWindState, newMap);
            }
            return windMap[step];
        }

            private string DrawMap(Dirs[,] wind){
            var str = string.Empty;
            for(var y = 0; y<rows; y++){
                for(var x = 0; x<cols; x++)
                    switch (wind[x, y]){
                        case Dirs.None:
                        str += '.';
                        break;
                    case Dirs.Up:
                        str += '^';
                        break;
                    case Dirs.Down:
                        str += 'v';
                        break;
                    case Dirs.Left:
                        str += '<';
                        break;
                    case Dirs.Right:
                        str += '>';
                        break;
                    default:
                        var u = wind[x, y] & Dirs.Up;
                        var dirs = new bool[4]{
                            wind[x, y].HasFlag(Dirs.Up), wind[x, y].HasFlag(Dirs.Down), 
                             wind[x, y].HasFlag(Dirs.Left), wind[x, y].HasFlag(Dirs.Right),
                        };
                        str += $"{dirs.Where(d => d).Count()}";
                        break;
                    }
                str += "\n";
            }
            return str;
        }
        }
        
        [Flags]
        protected enum Dirs{
            None = 0,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }
    }
}
