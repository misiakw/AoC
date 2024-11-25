using System;
using System.Linq;
using AoC.LegacyBase;
using AoC.Common.Maps;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day17 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day17/example1.txt")
                    .Part(1).Correct(102)
                //.Part(2).Correct(51);
                .Test("output", "./Inputs/Day17/output.txt");
                //.Part(1); //1027 too low
                //.Part(2);
        }

        protected StaticMap<byte> map;
        protected StaticMap<int> heatLossMap;
        protected string shortestPath;
        int minPath = int.MaxValue;

        public override string Part1(TestState input)
        {
            var lines = input.GetLines().ToArray();
            map = new StaticMap<byte>(lines[0].Length, lines.Count());
            heatLossMap = new StaticMap<int>(map.Width, map.Height, int.MaxValue);
            for(var y=0; y<map.Height; y++)
                for(var x=0; x<map.Width; x++){
                    map[x, y] = (byte)(lines[y][x] - '0');
                }

            for (var y = 0; y < map.Height; y++)
                for (var x = 0; x < map.Width; x++)
                {
                    map[x, y] = (byte)(lines[y][x] - '0');
                }

            int nX = 0;
            int nY = 0;
            int dX = 1;
            int dY = 0;
            minPath = 0;
            while (nX < map.Width && nY < map.Height)
            {
                if (dX == 1 || nX + 1 == map.Width)
                {
                    dX = 0;
                    dY = 1;
                }
                else
                {
                    dX = 1;
                    dY = 0;
                }
                if (nY + 1 == map.Height)
                {
                    if (nX + 1 == map.Width)
                        break;
                    dX = 1;
                    dY = 0;
                }
                nX += dX;
                nY += dY;
                shortestPath += $"{nX},{nY},#|";
                minPath += map[nX, nY];
            }

            CalculateHeatLoss(1, 0, Dir.Right, 0, 0, "|");
            CalculateHeatLoss(0, 1, Dir.Down, 0, 0, "|");


            PrintMap();
            return heatLossMap[map.Width-1, map.Height-1].ToString();
        }

        public override string Part2(TestState input)
        {
            throw new NotImplementedException();
        }

        int maxStreight = 3;
        private void CalculateHeatLoss(int x, int y, Dir dir, int currentStreight, int loss, string path){
            if(x < 0 || y < 0 || x==map.Width || y == map.Height || currentStreight>=maxStreight)
                return;
            loss += map[x, y];
            if(loss >= minPath || loss >= heatLossMap[x, y])
                return;
            if(loss < heatLossMap[x, y]){
                heatLossMap[x, y] = loss;
                if (x == map.Width - 1 && y == map.Height - 1)
                {
                    shortestPath = path;
                    minPath = loss;
                }
            }
            if(x == map.Width-1 && y == map.Height-1)
                return;

            var ch = dir switch{
                Dir.Up => '^',
                Dir.Down => 'v',
                Dir.Right => '>',
                Dir.Left => '<'
            };
            path += $"{x},{y},{ch}|";

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
                chars[int.Parse(pos[0]), int.Parse(pos[1])] = pos[2][0];
            }


            for(var y = 0; y<map.Height; y++){
                for(var x=0; x<map.Width; x++)
                    Console.Write(chars[x, y]);
                Console.WriteLine();
            }
        }
    }
} 