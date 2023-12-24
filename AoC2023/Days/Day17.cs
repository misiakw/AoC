using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common.Maps;

namespace AoC2023.Days
{
    public class Day17 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day17/example1.txt")
                .Part1(102);
                //.Part2(51);
            builder.New("output", "./Inputs/Day17/output.txt");
                //.Part1(); //1027 too low
                //.Part2();
        }
        
        protected StaticMap<byte> map;
        protected StaticMap<int> heatLossMap;
        protected string shortestPath;

        public override long Part1(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            map = new StaticMap<byte>(lines[0].Length, lines.Count());
            heatLossMap = new StaticMap<int>(map.Width, map.Height, int.MaxValue);
            for(var y=0; y<map.Height; y++)
                for(var x=0; x<map.Width; x++){
                    map[x, y] = (byte)(lines[y][x] - '0');
                }

            CalculateHeatLoss(1, 0, Dir.Right, 0, 0, "|");
            CalculateHeatLoss(0, 1, Dir.Down, 0, 0, "|");


            PrintMap();
            return heatLossMap[map.Width-1, map.Height-1];
        }

        public override long Part2(IComparableInput<long> input)
        {
            throw new NotImplementedException();
        }

        int maxStreight = 3;
        private void CalculateHeatLoss(int x, int y, Dir dir, int currentStreight, int loss, string path){
            if(x < 0 || y < 0 || x==map.Width || y == map.Height || currentStreight>=maxStreight)
                return;
            loss += map[x, y];
            if(path.Contains($"|{x},{y}|"))
                return;
            if(loss < heatLossMap[x, y]){
                heatLossMap[x, y] = loss;
                if(x == map.Width-1 && y == map.Height-1)
                    shortestPath = path;
            }
            if(x == map.Width-1 && y == map.Height-1)
                return;

            var ch = dir switch{
                Dir.Up => '^',
                Dir.Down => 'v',
                Dir.Right => '>',
                Dir.Left => '<'
            };
            path += $"{x},{y}|";

            foreach(var next in new Dir[4]{Dir.Up, Dir.Right, Dir.Down, Dir.Left}){
                if(next == Dir.Up && dir != Dir.Down)
                    CalculateHeatLoss(x, y-1, next, next!= dir ? 0 : currentStreight+1, loss, path);
                if(next == Dir.Down && dir != Dir.Up)
                    CalculateHeatLoss(x, y+1, next, next!= dir ? 0 : currentStreight+1, loss, path);

                if(next == Dir.Left && dir != Dir.Right)
                    CalculateHeatLoss(x-1, y, next, next!= dir ? 0 : currentStreight+1, loss, path);
                if(next == Dir.Right && dir != Dir.Left)
                    CalculateHeatLoss(x+1, y, next, next!= dir ? 0 : currentStreight+1, loss, path);

            }

            /*if(dir == Dir.Up || dir == Dir.Down){
                CalculateHeatLoss(x-1, y, Dir.Left, 0, loss, path);
                CalculateHeatLoss(x+1, y, Dir.Right, 0, loss, path);
                if(dir == Dir.Up && currentStreight < maxStreight)
                    CalculateHeatLoss(x, y-1, Dir.Up, currentStreight+1, loss, path);
                if(dir == Dir.Down && currentStreight < maxStreight)
                    CalculateHeatLoss(x, y+1, Dir.Down, currentStreight+1, loss, path);
            }
            else
            {
                CalculateHeatLoss(x, y-1, Dir.Up, 0, loss, path);
                CalculateHeatLoss(x, y+1, Dir.Down, 0, loss, path);
                if(dir == Dir.Right && currentStreight < maxStreight)
                    CalculateHeatLoss(x+1, y, Dir.Right, currentStreight+1, loss, path);
                if(dir == Dir.Left && currentStreight < maxStreight)
                    CalculateHeatLoss(x-1, y, Dir.Left, currentStreight+1, loss, path);
            }*/
        }

        protected enum Dir{
            Up, Down, Left, Right
        }

        private void PrintMap(){
            var chars = new char[map.Width, map.Height];
            for(var y = 0; y<map.Height; y++)
                for(var x=0; x<map.Width; x++)
                    chars[x, y] = (char)(map[x, y]+'0');
            foreach(var pos in shortestPath.Split('|').Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Split(','))){
                chars[int.Parse(pos[0]), int.Parse(pos[1])] = '#';
            }


            for(var y = 0; y<map.Height; y++){
                for(var x=0; x<map.Width; x++)
                    Console.Write(chars[x, y]);
                Console.WriteLine();
            }
        }
    }
} 