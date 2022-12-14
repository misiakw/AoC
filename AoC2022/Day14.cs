using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day14 : DayBase
    {
        public Day14() : base(14)
        {
            Input("example1")
                .RunPart(1, 24)
                .RunPart(2, 93)
            .Input("output")
                .RunPart(1, 610)
                .RunPart(2, 27194);
        }

        public override object Part1(Input input)
        {
            var map = GetMap(input);

            var ctr = 0;
            while (DropSand(map))
                ctr++;

            return ctr;
        }

        public override object Part2(Input input)
        {   
            var map = GetMap(input);

            var ctr = 0;
            var abyss = (int)map.Bounds[1].Item2 + 1;
            while (DropSand(map, abyss))
                ctr++;

            return ctr;
        }

        private Array2D<char> GetMap(Input input){
            var map = new Array2D<char>('.');

            foreach(var line in input.Lines){
                var points = line.Split(" -> ").Select(
                    p => p.Split(",").Select(d => int.Parse(d)).ToArray()
                ).ToArray();

                for(var i=1; i<points.Length; i++){
                    var dx = GetDrawDir(points[i][0], points[i-1][0]);
                    var dy = GetDrawDir(points[i][1], points[i-1][1]);
                    var p = new int[2] {points[i-1][0], points[i-1][1]};
                   do{
                        map[p[0], p[1]] = '#';
                        p[0]+=dx;
                        p[1]+=dy;
                        map[p[0], p[1]] = '#';
                    } while(p[0] != points[i][0] || p[1] != points[i][1]);
                }

            }
            return map;
        }

        private bool DropSand(Array2D<char> map) => DropSand(map, null);

        private bool DropSand(Array2D<char> map, int? abyss){
            int sx = 500;
            int sy = 0;
            var bottom = abyss ?? map.Bounds[1].Item2 + 1;

            if (abyss.HasValue && map[sx, sy] != '.') return false;

            while(sy < bottom){
                if(map[sx, sy+1] == '.')
                    sy++;
                else if (map[sx-1, sy+1] == '.'){
                    sx--;
                    sy++;
                }
                else if (map[sx+1, sy+1] == '.'){
                    sx++;
                    sy++;
                }
                else{
                    map[sx, sy] = 'o';
                    return true;
                }
            }
            if(abyss.HasValue) map[sx, sy] = 'o';

            return abyss.HasValue;
        }

        private int GetDrawDir(int s, int f){
            var d = s - f;
            return d == 0 ? d : d > 0 ? 1 : -1;
        }
    }
}
