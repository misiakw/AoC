using AoC.Common;
using AoC.Common.Maps;
using AoC.Common.Maps.StaticMap;
using AoCBase2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static AoC2024.Days.Day15;

namespace AoC2024.Days
{
    internal class Day15: IDay
    {
        public static void RunAoC() => AocRuntime.Day<Day15>(15, t => new Day15(t.GetLines()))
                .Callback(1, (d, t) => d.Part1())//.Skip()
                .Callback(2, (d, t) => d.Part2())
                .Test("smallexample").Skip(2)
                .Test("example").Skip(2)
                .Test("examplePart2").Skip(1)
                .Test("input").Skip(2)
                //.Part(1).Correct(1485257)
                //.Part(2).Correct()
                .Run();

        private StaticMap<char> map;
        private (int x, int y) robot;
        private string steps = string.Empty;
        public Day15(IEnumerable<string> lines)
        {
            map = ReadMap(lines.ToArray());
            foreach (string line in lines.Skip((int)map.Height + 1))
                steps += line.Trim();
        }

        private StaticMap<char> ReadMap(string[] lines)
        {
            var height = 0;
            var width = lines.First().Length;
            while (!string.IsNullOrEmpty(lines[height])) height++;

            var map = new StaticMap<char>(width, height);
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    map[x, y] = lines[y][x];
                    if (lines[y][x] == '@')
                        robot = (x, y);
                }
            return map;
        }

        public string Part1()
        {
            (int x, int y) myRobot = (robot.x, robot.y);

            var warehouse = new WarehouseMap(map, 1);

            foreach (var ch in steps)
            {
                (int x, int y) dir = ch switch
                {
                    '<' => (-1, 0),
                    '>' => (1, 0),
                    '^' => (0, -1),
                    'v' => (0, 1)
                };

                var nx = myRobot.x + dir.x;
                var ny = myRobot.y + dir.y;
                if (warehouse.map[nx, ny].CanMove(ch))
                {
                    warehouse.map[nx, ny].Move(ch);
                    myRobot = (nx, ny);
                }
            }

            return warehouse.map.Where(b => b is BoxBlock).Select(b => ((BoxBlock)b).Coord).Sum().ToString();
        }
        public string Part2()
        {
            (int x, int y) myRobot = (robot.x, robot.y);

            var warehouse = new WarehouseMap(map, 2);

            foreach (var ch in steps)
            {
                (int x, int y) dir = ch switch
                {
                    '<' => (-1, 0),
                    '>' => (1, 0),
                    '^' => (0, -1),
                    'v' => (0, 1)
                };

                var nx = myRobot.x + dir.x;
                var ny = myRobot.y + dir.y;
                if (warehouse.map[nx, ny].CanMove(ch))
                {
                    warehouse.map[nx, ny].Move(ch);
                    myRobot = (nx, ny);
                }
            }

            Console.WriteLine(warehouse.map.Draw(b => b is WallBlock ? "#" : b is EmptyBlock ? "." : "O"));

            return warehouse.map.Where(b => b is BoxBlock).Select(b => ((BoxBlock)b).Coord).Sum().ToString();
        }

        internal class WarehouseMap
        {
            public StaticMap<IBlock> map;
            public WarehouseMap(StaticMap<char> initialMap, int width)
            {
                map = new StaticMap<IBlock>(initialMap.Width*width, initialMap.Height);
                for(var y=0; y<initialMap.Height; y++)
                    for(var x=0; x<initialMap.Width; x++)
                    {
                        if (initialMap[x, y] == 'O') {
                            var box = new BoxBlock(this, x, y, width);
                            for (var d = 0; d < width; d++)
                                map[x*width + d, y] = box;
                        }
                        else if (initialMap[x, y] == '#')
                            for (var d = 0; d < width; d++)
                                map[x*width + d, y] = new WallBlock();
                        else
                            for (var d = 0; d < width; d++)
                                map[x*width + d, y] = new EmptyBlock();
                    }
            }
            
        }


        internal interface IBlock
        {
            internal void Move(char dir);
            internal bool CanMove(char dir);
        }
        internal class EmptyBlock : IBlock{
            public bool CanMove(char dir) => true;
            public void Move(char dir) { }
        }
        internal class WallBlock : IBlock{
            public bool CanMove(char dir) => false;
            public void Move(char dir) { }
        }
        internal class BoxBlock : IBlock
        {
            public BoxBlock(WarehouseMap warehouse, int x, int y, int width)
            {
                this.warehouse = warehouse;
                this.x = x;
                this.y = y;
                this.width = width;
            }
            private int x, y, width;
            private WarehouseMap warehouse;

            private IList<IBlock> GetAffected(char dir) { 
                var list = new List<IBlock>();

                if (dir == '<')
                    list.Add(warehouse.map[x - 1, y]);
                else if (dir == '>')
                    list.Add(warehouse.map[x + 1, y]);
                else if (dir == 'v')
                    for (var d = 0; d < width; d++)
                        list.Add(warehouse.map[x+d, y + 1]);
                else if (dir == '^')
                    for (var d = 0; d < width; d++)
                        list.Add(warehouse.map[x+d, y - 1]);
                return list.Distinct().ToList();
            }

            public void Move(char dir)
            {
                var affected = GetAffected(dir);
                foreach (var box in affected)
                    box.Move(dir);

                //leave empty behind
                warehouse.map[x, y] = new EmptyBlock();
                if (dir == '<')
                    x -= 1;
                else if (dir == '>')
                    x += 1;
                else if (dir == 'v')
                    y += 1;
                else if (dir == '^')
                    y -= 1;

                //ocupate space
                warehouse.map[x, y] = this;
            }

            public bool CanMove(char dir)
                => GetAffected(dir).All(b => b.CanMove(dir));

            public int Coord => 100 * y + x;
        }
    }
}
