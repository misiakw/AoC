using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;
using AoC.Common.Abstractions;
using AoC.Common.Maps;
using ImageMagick.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Days;

[DayNum(9)]
file class Day9 : IDay
{
    public void RunAoC()
    {
        AocRuntime.Day<Day9>(9)
            //.Callback(1, (d, t) => d.Part1(t.GetLines()))
            .Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example", "Inputs/Day9/example.txt") //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day9/input.txt").Skip() //.Part(1)//.Part(2)
            .Run();
    }

    public string Part1(IEnumerable<string> input)
    {
        var points = new List<Point>();
        var order = new List<(long size, Point p1, Point p2)>();
        foreach(var line in input)
        {
            var point = new Point(line);
            foreach (var second in points)
                order.Add((point.GetRectangleSize(second), point, second));
            points.Add(point);
        }
        return order.OrderByDescending(p => p.size).First().size.ToString();
    }

    public string Part2(IEnumerable<string> input)
    {
        var linesX = new Dictionary<long, IList<(long s, long e)>>();
        var linesY = new Dictionary<long, IList<(long s, long e)>>();

        Point first = null;
        Point previous = null;
        foreach (var line in input)
        {
            var point = new Point(line);
            if(previous == null)
            {
                first = point;
                previous = point;
                continue;
            }

            Console.WriteLine($"line from {previous}=>{point}");
            previous = point;
        }
        Console.WriteLine($"line from {previous}=>{first}");

        return "";
    }
}

file class Point
{
    public readonly long x;
    public readonly long y;

    public Point(string line)
    {
        var pos = line.Split(',').Select(long.Parse).ToArray();
        this.x = pos[0];
        this.y = pos[1];
    }
    public long GetRectangleSize(Point second)
    {
        var dx = (x < second.x ? second.x - x : x - second.x)+1;
        var dy = (y < second.y ? second.y - y : y - second.y)+1;
        return dx*dy;
    }

    public override string ToString() => $"Point({x},{y})";
}