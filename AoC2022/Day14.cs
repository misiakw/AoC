using AoC.Base;
using AoC.Common;
using ImageMagick;
using System.Linq;

namespace AoC2022
{
    public class Day14 : LegacyDayBase
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

        public override object Part1(LegacyInput input)
        {
            var map = GetMap(input);

            var ctr = 0;
            while (DropSand(map))
                ctr++;

            PrintImg(map, input.InputDir, $"{input.Name}-Part1");
            return ctr;
        }

        public override object Part2(LegacyInput input)
        {   
            var map = GetMap(input);

            PrintImg(map, input.InputDir, $"{input.Name}-empty");

            var ctr = 0;
            var abyss = (int)map.Bounds[1].Item2 + 1;
            while (DropSand(map, abyss))
                ctr++;

            PrintImg(map, input.InputDir, $"{input.Name}-Part2");
            return ctr;
        }

        private Array2D<char> GetMap(LegacyInput input){
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

        private void PrintImg(Array2D<char> map, string dir, string name){
            var printer = new ImagePrinter(dir);
            printer.DrawImage((int)map.Width*10, (int)map.Height * 10, name,
            img => {
                var startX = map.Bounds[0].Item1;
                var startY = map.Bounds[1].Item1;
                for(var y=startY; y <= map.Bounds[1].Item2; y++)
                    for(var x=startX; x <= map.Bounds[0].Item2; x++){
                        var rect = new DrawableRectangle((x-startX)*10, (y-startY)*10,(x-startX)*10+10, (y-startY)*10+10);
                        MagickColor color = new MagickColor("red");
                        switch(map[x, y]){
                            case '#':
                                color = new MagickColor("gray");
                                break;
                            case 'o':
                                color = new MagickColor("yellow");
                                break;
                        }
                        if(map[x, y] != '.') 
                            img.Draw(new DrawableStrokeColor(color), new DrawableStrokeWidth(1), new DrawableFillColor(color), rect);
                    }
            });
        }
    }
}
