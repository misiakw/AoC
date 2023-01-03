using System;
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
                .RunPart(2, 54)
                //.RunPart(1, 18)
            .Input("output")
                //.RunPart(1, 257)
                .RunPart(2);
        }

        public override object Part1(Input input){
            var solver = new Day24Solver(input);

            return solver.GoToEnd(0);
        }

        public override object Part2(Input input)
        {
            var solver = new Day24Solver(input);

            var toEnd = solver.GoToEnd(0);
            Console.WriteLine($"Went to end in {toEnd} minutes");
            var back = solver.GoBack(toEnd);
            Console.WriteLine($"Went back in {back} minutes");
            toEnd = solver.GoToEnd(back);
            Console.WriteLine($"Went to end in {toEnd} minutes");
            return toEnd;
        }

        protected class Day24Solver{
            
            protected IDictionary<long, Dirs[,]> windMap = new Dictionary<long, Dirs[,]>();
            protected IDictionary<string, long> preTestedPaths = new Dictionary<string, long>();
            protected int rows;
            protected int cols;
            protected int topWindState = 0;
            protected long longestSoFar = 900; //because reasons -it worked and int.maxvalue used too much memory and result of first input suggests it should be ok...

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

        public long GoToEnd(long step){
            var result = GoAhead(0, -1, cols-1, rows, step, true);
            longestSoFar = 900;
            preTestedPaths = new Dictionary<string, long>();
            return result;
        }

        public long GoBack(long step){
            var result = GoAhead(cols-1, rows, 0, -1, step, true);
            longestSoFar = 900;
            preTestedPaths = new Dictionary<string, long>();
            return result;
        }

        private long GoAhead(int x, int y, int dx, int dy, long step, bool wasStationary = false){
            if(!wasStationary){
                if(x == dx && y==dy){
                    if(step < longestSoFar) longestSoFar = step;
                    return step;
                }
                if(x<0 || y<0 || x== cols || y == rows)
                    return int.MaxValue;
            }
            var state = GetWind(step);
            if (step > longestSoFar)
                    return int.MaxValue;
            if(x>=0 && y>=0 && x<cols && y < rows && state[x, y] != Dirs.None)
                    return int.MaxValue;
            var key = $"{x}|{y}|{step}";
            if(preTestedPaths.ContainsKey(key))
                return preTestedPaths[key];

            var results = new List<long>();

            results.Add(GoAhead(x+1, y, dx, dy, step+1));            
            results.Add(GoAhead(x-1, y, dx, dy, step+1));
            results.Add(GoAhead(x, y+1, dx, dy, step+1));
            results.Add(GoAhead(x, y-1, dx, dy, step+1));
            results.Add(GoAhead(x, y, dx, dy, step+1, true));

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
