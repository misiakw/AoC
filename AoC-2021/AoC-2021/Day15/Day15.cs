using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [ExpectedResult(TestName = "Input", Result = "403")] //403
        public override string Part1(string testName)
        {
            var dist = new int[Map.Width, Map.Height];
            for (var y = 0; y < Map.Height; y++)
                for (var x = 0; x < Map.Width; x++)
                    dist[x, y] = int.MaxValue;
            CalculateDist(Map.Width - 1, Map.Height - 1, dist, 0, Map.Width, Map.Height);

            return (dist[0, 0] - Map[0, 0]).ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "315")]
        //[ExpectedResult(TestName = "Input", Result = "3259")]
        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        private void CalculateDist(long x, long y, int[,] dist, int callerDist, long cx, long cy)
        {
            var len = callerDist + Map[x, y];
            if (len >= dist[x, y]) return;
            dist[x, y] = len;

            if (x + 1 < Map.Width && x + 1 != cx)
                CalculateDist(x + 1, y, dist, len, x, y);
            if (x - 1 >= 0 && x - 1 != cx)
                CalculateDist(x - 1, y, dist, len, x, y);
            if (y + 1 < Map.Height && y + 1 != cy)
                CalculateDist(x, y + 1, dist, len, x, y);
            if (y - 1 >= 0 && y - 1 != cy)
                CalculateDist(x, y - 1, dist, len, x, y);
        }

        private class ExtendibleMap
        {
            private readonly int[,] map;
            public readonly int Width;
            public readonly int Height;
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
                    var dx = x / Width;
                    var dy = y / Width;

                    return map[x%Width, y%Height];
                }
            }
        }
    }
}
