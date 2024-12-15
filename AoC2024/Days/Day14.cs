using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using AoCBase2;

namespace AoC2024.Days;

public class Day14 : IDay
{
    public static void RunAoC() => AocRuntime.Day<Day14>(14)
        .Callback(1, (d, t) => d.Part1(t.GetLines()))
        .Callback(2, (d, t) => d.Part2(t.GetLines()))
        .Test("single").Skip()
        .Test("example").Skip(2)
        .Test("input")
        //.Part(1).Correct(224438715)
        //.Part(2).Correct(7603?)
        .Run();

    public string Part1(IEnumerable<string> input)
    {
        var robots = input.Select(l => new Robot(l)).ToArray();
        const int width = 101;
        const int height = 103;

        foreach (var robot in robots)
            robot.Proceed(width, height, 100);

        var quadrantWidth = (width - 1) / 2;
        var quadrantHeight = (height - 1) / 2;


        int[,] quadrants = new int[2, 2] { { 0, 0 }, { 0, 0 } };

        foreach (var robot in robots)
        {
            if (robot.x == quadrantWidth || robot.y == quadrantHeight) continue;
            if (robot.x < quadrantWidth)
            {
                if (robot.y < quadrantHeight) quadrants[0, 0]++;
                else quadrants[0, 1]++;
            }
            else
            {
                if (robot.y < quadrantHeight) quadrants[1, 0]++;
                else quadrants[1, 1]++;
            }
        }

        return (quadrants[0, 0] * quadrants[1, 0] * quadrants[0, 1] * quadrants[1, 1]).ToString();
    }

    public string Part2(IEnumerable<string> input)
    {
        var robots = input.Select(l => new Robot(l)).ToArray();
        const int width = 101;
        const int height = 103;
        
        foreach (var robot in robots)
            robot.Proceed(width, height, 7603);
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
                Console.Write(robots.Any(r => r.x == x && r.y == y) ? "*" : "."); 
            Console.WriteLine();
        }

        return "7603";
    }


    private class Robot
    {
        public int x, y;
        public readonly int vx, vy;

        public Robot(string descr)
        {
            var match = new Regex($"p=(?<startx>-?\\d+),(?<starty>-?\\d+) v=(?<vx>-?\\d+),(?<vy>-?\\d+)")
                .Match(descr);
            x = int.Parse(match.Groups["startx"].Value);
            y = int.Parse(match.Groups["starty"].Value);
            vx = int.Parse(match.Groups["vx"].Value);
            vy = int.Parse(match.Groups["vy"].Value);
        }

        public void Proceed(int width, int height, int times = 1)
        {
            x = (x + (vx * times)) % width;
            if (x < 0) x += width;
            y = (y + (vy * times)) % height;
            if (y < 0) y += height;
        }

        public override string ToString() => $"[{x},{y} => {vx},{vy}]";
    }
}

