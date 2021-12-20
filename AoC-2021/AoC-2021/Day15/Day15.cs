using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC_2021.Day15
{
    [BasePath("Day15")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day15 : DayBase
    {
        public Day15(string filePath) : base(filePath)
        {
            var width = LineInput[0].Length;
            var height = LineInput.Count();
            var tmpMap = new int[width, height];

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    tmpMap[x, y] = LineInput[y][x] - '0';
            Map = new ExtendibleMap(tmpMap, width, height);
        }

        private readonly ExtendibleMap Map;

        [ExpectedResult(TestName = "Example", Result = "40")]
        [ExpectedResult(TestName = "Input", Result = "403")]
        public override string Part1(string testName)
        {
            return Dijksta(Map).ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "315")]
        [ExpectedResult(TestName = "Input", Result = "2840")]
        public override string Part2(string testName)
        {
            Map.ResizeMap(5);
            return Dijksta(Map).ToString();
        }

        private int Dijksta(ExtendibleMap map)
        {
            var weight = new Dictionary<string, int>();
            var toProcess = new Dictionary<string, int>();
            var toConsider = new Dictionary<string, int>();

            for (int y = 0; y < map.Height; y++)
                for(int x = 0; x< map.Width; x++)
                {
                    weight.Add($"{x},{y}", map[x, y]);
                    toProcess.Add($"{x},{y}", int.MaxValue);
                }

            toProcess["0,0"] = 0;
            toConsider.Add("0,0", 0);

            while (toProcess.Any())
            {
                var lowest = toConsider.OrderBy(kv => kv.Value).First();
                toProcess.Remove(lowest.Key);
                toConsider.Remove(lowest.Key);

                if (lowest.Key == $"{map.Width - 1},{map.Height - 1}")
                    return lowest.Value;

                var pos = lowest.Key.Split(",").Select(s => int.Parse(s)).ToArray();

                PrepareNear(pos[0]+1, pos[1], toProcess, lowest.Value, weight, toConsider);
                PrepareNear(pos[0]-1, pos[1], toProcess, lowest.Value, weight, toConsider);
                PrepareNear(pos[0], pos[1]+1, toProcess, lowest.Value, weight, toConsider);
                PrepareNear(pos[0], pos[1]-1, toProcess, lowest.Value, weight, toConsider);
            }
            return -1;
        }
        
        private void PrepareNear(int x, int y, IDictionary<string, int> toProcess, int path, IDictionary<string, int> weights, IDictionary<string, int> toConsider)
        {
            if (!toProcess.ContainsKey($"{x},{y}")) return;

            path += weights[$"{x},{y}"];

            if (toProcess[$"{x},{y}"] <= path) return;

            toProcess[$"{x},{y}"] = path;
            toConsider[$"{x},{y}"] = path;
        }

        private class ExtendibleMap
        {
            private int[,] map;
            public int Width;
            public int Height;
            public ExtendibleMap(int[,] Map, int width, int height)
            {
                map = Map;
                Width = width;
                Height = height;
            }

            public int this[long x, long y]
            {
                get
                {
                    return map[x, y];
                }
            }

            public void ResizeMap(int times)
            {
                var newWidth = Width * times;
                var newHeight = Height * times;

                var newMap = new int[newWidth, newHeight];

                for(int y=0; y<newHeight; y++)
                    for(int x=0; x<newWidth; x++)
                    {
                        var addons = x / Width + y / Width;
                        var value = (map[x % Width, y % Height] + addons);
                        while (value > 9) value -= 9;
                        newMap[x, y] = value;
                    }

                map = newMap;
                Width = newWidth;
                Height = newHeight;
            }
        }
    }
}
