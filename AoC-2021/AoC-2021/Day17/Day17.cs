using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day17
{
    [BasePath("Day17")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day17 : DayBase
    {
        public Day17(string filePath) : base(filePath)
        {
            var inp = LineInput[0].Substring(13).Trim().Split(", ")
                .Select(c => c.Substring(2).Split("..")).ToArray();
            minX = int.Parse(inp[0][0]);
            maxX = int.Parse(inp[0][1]);
            minY = int.Parse(inp[1][0]);
            maxY = int.Parse(inp[1][1]);
        }

        private int minX, maxX, minY, maxY;
        private int vMaxY;

        [ExpectedResult(TestName = "Example", Result = "45")]
        [ExpectedResult(TestName = "Input", Result = "25200")]
        public override string Part1(string testName)
        {
            vMaxY = Math.Abs(minY) - 1;
            var v = vMaxY;

            var topHeight = 0;
            while (v > 0) topHeight += v--;

            return topHeight.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "112")]
        //[ExpectedResult(TestName = "Input", Result = "403")]
        public override string Part2(string testName)
        {
            var vMinY = minY;
            var vMaxX = maxX;

            var differentSpeeds = new List<string>();

            for (var y = vMinY; y <= vMaxY; y++)
                for (var x = 1; x <= vMaxX; x++)
                    if (WillHit(x, y) && !differentSpeeds.Contains($"{x},{y}"))
                    {
                        differentSpeeds.Add($"{x},{y}");
                        //Console.WriteLine($"{x},{y}");
                    }

            return differentSpeeds.Count().ToString() ;
        }

        private bool WillHit(int vX, int vY)
        {
            int x = 0;
            int y = 0;
            while(x<=maxX && y >= minY)
            {
                if (x >= minX && x <= maxX && y <= maxY && y >= minY)
                    return true;
                x += vX;
                y += vY;
                if (vX > 0) vX--;
                vY--;
            }
            return false ;
        }
    }
}
