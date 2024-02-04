using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using AoC.Common.Maps;

namespace AoC2023.Days
{
    public class Day18 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day18/example1.txt")
                .Part1(62)
                .Part2(952408144115);
            builder.New("output", "./Inputs/Day18/output.txt")
                .Part1(39194);
                //.Part2(0);
        }

        private IEnumerable<InpStruct> readInput1(IComparableInput<long> input){
            var regex = new Regex(@"(.) (\d+) \(\#(.*)\)");
            foreach(var line in ReadLines(input)){
                var match = regex.Match(line);
                yield return new InpStruct(){
                    dir = match.Groups[1].Value[0],
                    len = int.Parse(match.Groups[2].Value)
                };
            }
        }
        private IEnumerable<InpStruct> readInput2(IComparableInput<long> input){
            var regex = new Regex(@"(.) (\d+) \(\#(.*)\)");
            foreach(var line in ReadLines(input)){
                var match = regex.Match(line).Groups[3].Value;
                yield return new InpStruct(){
                    dir = match[5] switch{
                        '0' => 'R',
                        '1' => 'D',
                        '2' => 'L',
                        _ => 'U'
                    },
                    len = int.Parse(string.Join("", match.ToCharArray().Take(5)), System.Globalization.NumberStyles.HexNumber)
                };
            }
        }

        public override long Part1(IComparableInput<long> input)
        {
            var map = MapBuilder<bool>.GetEmpty(new MapBuilderParams<bool>{
                DefaultValue = false,
                IsInfinite = true
            });
            var x = 0l;
            var y = 0l;
            var ctr = 0l;

            foreach(var order in readInput1(input)){
                ctr += order.len;
                switch(order.dir){
                    case 'U':
                        for(var i=0; i<order.len; i++)
                            map[x, y--] = true;
                        break;
                    case 'D':
                        for(var i=0; i<order.len; i++)
                            map[x, y++] = true;
                        break;
                    case 'L':
                        for(var i=0; i<order.len; i++)
                            map[x--, y] = true;
                        break;
                    case 'R':
                        for(var i=0; i<order.len; i++)
                            map[x++, y] = true;
                        break;
                }
            }

            var sX = map.rangeX.Min+1;
            var sY = map.rangeY.Min-1;
            while(!map[sX, sY])
                sY++;
            while(map[sX, sY])
                sY++;

            floodFill(map, sX, sY, ref ctr);
            return ctr;
        }

        public override long Part2(IComparableInput<long> input)
        {
            var map = MapBuilder<bool>.GetEmpty(new MapBuilderParams<bool>{
                DefaultValue = false,
                IsInfinite = true
            });
            var x = 0l;
            var y = 0l;
            var ctr = 0l;

            foreach(var order in readInput2(input)){
                ctr += order.len;
                switch(order.dir){
                    case 'U':
                        for(var i=0; i<order.len; i++)
                            map[x, y--] = true;
                        break;
                    case 'D':
                        for(var i=0; i<order.len; i++)
                            map[x, y++] = true;
                        break;
                    case 'L':
                        for(var i=0; i<order.len; i++)
                            map[x--, y] = true;
                        break;
                    case 'R':
                        for(var i=0; i<order.len; i++)
                            map[x++, y] = true;
                        break;
                }
            }

            /*var sX = map.rangeX.Min+1;
            var sY = map.rangeY.Min-1;
            while(!map[sX, sY])
                sY++;
            while(map[sX, sY])
                sY++;

            floodFill(map, sX, sY, ref ctr);*/
            return ctr;
        }

        private void floodFill(IMap<bool> map, long x, long y, ref long ctr){
            if(map[x, y]) return;
            map[x, y] = true;
            ctr++;
            floodFill(map, x-1, y, ref ctr);
            floodFill(map, x+1, y, ref ctr);
            floodFill(map, x, y-1, ref ctr);
            floodFill(map, x, y+1, ref ctr);
        }

        private bool isInside(IMap<int> map, long x, long y){
            if(x < map.rangeX.Min || x > map.rangeX.Max) return false;
            if(y < map.rangeY.Min || y > map.rangeY.Max) return false;
            var dif = new int[2]{0, 1};
            foreach(var dx in dif)
                foreach(var dy in dif)
                    if(!(dx == dy) && map[x+dx, y+dy] == 0 && !isInside(map, x+dx, y+dy))
                        return false;
            return true;
        }

        private struct InpStruct{
            public char dir;
            public int len;
        }
    }
}
