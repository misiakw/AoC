using AoC_2021.Attributes;
using AoC_2021.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AoC_2021.Day9
{
    [BasePath("Day9")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day9 : DayBase
    {
        public Day9(string filePath) : base(filePath)
        {
            Width = this.LineInput[0].Length;
            Height = this.LineInput.Count;
            Map = new int[Width, Height];

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    Map[x, y] = LineInput[y][x] - '0';
        }

        private readonly int[,] Map;
        private readonly int Width;
        private readonly int Height;
        private readonly IList<Common.Point> LowerPoints = new List<Common.Point>();

        [ExpectedResult(TestName = "Example", Result = "15")]
        [ExpectedResult(TestName = "Input", Result = "560")]
        public override string Part1(string testName)
        {
            var riskSum = 0;

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                    if (IsLowest(x, y))
                    {
                        riskSum += Map[x, y] + 1;
                        LowerPoints.Add(new Common.Point(x, y));
                    }

            return riskSum.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "1134")]
        [ExpectedResult(TestName = "Input", Result = "959136")]
        public override string Part2(string testName)
        {
            var basinMap = new int[Width, Height];
            int basinNum = 0;
            var basinSizes = new List<int>();

            foreach (var lowPoint in LowerPoints)
                basinSizes.Add(ExtendBasin(lowPoint.X, lowPoint.Y, ++basinNum, basinMap));

            PrintMap(basinMap, basinNum, testName + "_p2");
            return basinSizes.OrderByDescending(x => x).Take(3).Aggregate(1, (x, y) => x * y).ToString();
        }

        private bool IsLowest(int x, int y)
        {
            if (x > 0 && Map[x - 1, y] <= Map[x, y]) return false;
            if (x < Width - 1 && Map[x + 1, y] <= Map[x, y]) return false;
            if (y > 0 && Map[x, y - 1] <= Map[x, y]) return false;
            if (y < Height - 1 && Map[x, y + 1] <= Map[x, y]) return false;
            return true;
        }

        private int ExtendBasin(long x, long y, int basinNum, int[,] basinMap)
        {
            if (Map[x, y] == 9) return 0;
            if (basinMap[x, y] != 0) return 0;

            var result = 1;
            basinMap[x, y] = basinNum;

            if (x > 0) result += ExtendBasin(x - 1, y, basinNum, basinMap);
            if (x < Width - 1) result += ExtendBasin(x + 1, y, basinNum, basinMap);
            if (y > 0) result += ExtendBasin(x, y - 1, basinNum, basinMap);
            if (y < Height - 1) result += ExtendBasin(x, y + 1, basinNum, basinMap);

            return result;
        }

        private static bool isOpened = false;
        private void PrintMap(int[,] basinMap, int basinSize, string testName)
        {
            var fileName = Path.Combine(InputDir, "Img", $"{testName}.png");
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            if (File.Exists(fileName))
                File.Delete(fileName);

            var colors = new Color().GetColors(basinSize).ToArray();
            var boxSize = Width >= Height
                ? (int)800 / Width
                : (int)800 / Height;


            using (var image = new Bitmap(Width*boxSize, Height*boxSize))
            {
                using (var canvas = Graphics.FromImage(image))
                {
                    canvas.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);
                    for(var x=0; x<Width; x++)
                        for(var y=0; y<Height; y++)
                        {

                            var color = Color.FromArgb((Map[x, y]+1) * 25, colors[basinMap[x, y]]);
                            canvas.FillRectangle(new SolidBrush(color), x * boxSize, y * boxSize, boxSize, boxSize);
                        }
                }
                image.Save(fileName);
            }

            if (!isOpened)
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(fileName));
                isOpened = true;
            }
        }
    }
}
