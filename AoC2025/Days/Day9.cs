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
using System.Xml.Schema;
using LineDict = System.Collections.Generic.Dictionary<long, System.Collections.Generic.IList<(long s, long e)>>;

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
            .Test("input", "Inputs/Day9/input.txt") //.Part(1)//.Part(2)
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
        (var linesX, var linesY, var points) = LoadInput(input);


        return "";
    }
    

    private (LineDict x, LineDict y, Point[] points) LoadInput(IEnumerable<string> input)
    {
        var linesX = new LineDict();
        var linesY = new LineDict();
        var points = new List<Point>();

        Point first = null;
        Point previous = null;
        foreach (var line in input)
        {
            var point = new Point(line);
            points.Add(point);
            if (previous == null)
            {
                first = point;
                previous = point;
                continue;
            }

            if (previous.x == point.x)
                AddToDictionary(linesX, point.x, previous.y, point.y);
            else
                AddToDictionary(linesY, point.y, previous.x, point.x);
            previous = point;
        }

        if (previous.x == first.x)
            AddToDictionary(linesX, first.x, previous.y, first.y);
        else
            AddToDictionary(linesY, first.y, previous.x, first.x);

        return (linesX, linesY, points.ToArray());
    }

    private void AddToDictionary(LineDict dict, long key, long a, long b)
    {
        if(!dict.ContainsKey(key)) dict.Add(key, new List<(long s, long e)>());
        if (a < b)
            dict[key].Add((a, b));
        else 
            dict[key].Add((b, a));
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