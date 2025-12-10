using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;
using AoC.Common.Abstractions;
using AoC.Common.Maps;
using ImageMagick.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AoC2025.Days;

[DayNum(9)]
file class Day9 : IDay
{
    public void RunAoC()
    {
        AocRuntime.Day<Day9>(9)
            //.Callback(1, (d, t) => d.Part1(t.GetLines()))
            .Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example", "Inputs/Day9/example.txt").Skip() //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day9/input.txt")//.Skip() //.Part(1)//.Part(2)
            .Run();
    }

    public string Part1(IEnumerable<string> input)
    {
        var points = new List<Point>();
        var order = new List<(long size, Point p1, Point p2)>();
        foreach (var line in input)
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
        var map = new Map(input);
        var pointDict = map.Points.ToDictionary(k => (k.x, k.y), v => v);
        var rectangles = new List<(long size, Point p1, Point p2)>();


 //       foreach(var p in map.Points)
 //           Console.WriteLine(p.ToString());


        foreach (var start in map.Points) {
            var toCompare = map.Points.Where(p => p.x > start.x && p.y > start.y).ToList();
            foreach (var end in toCompare)
                rectangles.Add((start.GetRectangleSize(end), start, end));
        }


        var ctr = 0;
        rectangles = rectangles.OrderByDescending(r =>  r.size).ToList();

        foreach(var rect in rectangles)
        {
           /* if(!rect.p1.isRed || !rect.p2.isRed)
            {
                if (!pointDict.ContainsKey((rect.p1.x, rect.p2.y)) || !pointDict.ContainsKey((rect.p2.x, rect.p1.y)))
                {
                    continue;
                }
                if (!pointDict[(rect.p1.x, rect.p2.y)].isRed || !pointDict[(rect.p2.x, rect.p1.y)].isRed)
                {
                    continue;
                }
            }*/

            var breakPoints = map.Points.Where(p => p.x > rect.p1.x && p.y > rect.p1.y && p.x < rect.p2.x && p.y < rect.p2.y).ToArray();
            if (breakPoints.Any())
            {
                continue;
            }

            if(ctr++ < 4)
            Console.WriteLine(rect);
        }

        return "";
    }
}
//private long? RayCast(long x, long y, )

file class Map
{
    //private IDictionary<long, List<(long s, long e)>> _dictConstX;
    //private IDictionary<long, List<(long s, long e)>> _dictConstY;
    public Point[] Points { get; init; }
    public Map(IEnumerable<string> input)
    {
        //_dictConstX = new Dictionary<long, List<(long s, long e)>>();
        //_dictConstY = new Dictionary<long, List<(long s, long e)>>();
        var linesX = new List<(long x, long s, long e)>();
        var linesY = new List<(long y, long s, long e)>();
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
                linesX.Add(GetLine(point.x, point.y, previous.y));
            else
                linesY.Add(GetLine(point.y, point.x, previous.x));
            previous = point;
        }

        if (previous.x == first.x)
            linesX.Add(GetLine(first.x, first.y, previous.y));
        else
            linesY.Add(GetLine(first.y, first.x, previous.x));


        var pointsToAdd = new List<(long x, long y)>();
        foreach(var point in points)
        {
            foreach (var lineToCut in linesX.Where(l => l.x != point.x && l.s < point.y && l.e > point.x))
                pointsToAdd.Add((lineToCut.x,point.y));
            foreach (var lineToCut in linesY.Where(l => l.y != point.y && l.s < point.x && l.e > point.x))
                pointsToAdd.Add((point.x,lineToCut.y));
        }

        foreach (var p in pointsToAdd.Distinct())
            if (!points.Any(pts => pts.x == p.x && pts.y == p.y))
                points.Add(new Point(p.x, p.y));

            //ToDo: split lines on smaller chunks to geet aditional points
            //tougth: do i need lines? cannot i split lines based on points?

            Points = points.OrderBy(p => p.y).OrderBy(p => p.x).ToArray();
    }

    private (long, long, long) GetLine(long key, long a, long b)
        => (key, a < b ? a : b, a > b ? a : b);

    /*private void AddToDictionary(IDictionary<long, List<(long s, long e)>> dict, long key, long a, long b)
    {
        if (!dict.ContainsKey(key)) dict.Add(key, new List<(long s, long e)>());
        if (a < b)
            dict[key].Add((a, b));
        else
            dict[key].Add((b, a));
    }

    public long? RayCast(long x, long y, Dir dir)
    {
        IDictionary<long, List<(long s, long e)>> dict = null;
        Func<(long s, long e), bool> lineFunc = null;
        if (dir == Dir.Left || dir == Dir.Right) //is horizontal
        {
            dict = _dictConstX;
            lineFunc = new Func<(long s, long e), bool>(l => l.s <= y && l.e >= y);
        }
        else
        {
            dict = _dictConstY;
            lineFunc = new Func<(long s, long e), bool>(l => l.s <= x && l.e >= x);
        }
        var isKeyInRange = dir switch
        {
            Dir.Right => new Func<long, bool>(k => k > x),
            Dir.Left => new Func<long, bool>(k => k < x),
            Dir.Down => new Func<long, bool>(k => k > y),
            Dir.Up => new Func<long, bool>(k => k < y),
        };

        //ToDo: linie muszą mieć stronę "wewnętrzną" i stronę "zewnętrzną"
        //ToDo: poprawny flow sprawdzenia:
        // Jeśli znalazłem linię, sprawdzam czy jej "wnętrze" jest zgodne z moim kierunkiem, jak tak, patrze na kolejną
        // ToDo: sprawdzic czy input ma linie nakladajace sie na siebie

        var range = dict.Where(kv => isKeyInRange(kv.Key) && kv.Value.Any(lineFunc))
            .Select(kv => kv.Key).ToList();
        return range.Count > 0
            ? (dir == Dir.Right || dir == Dir.Down) ? range.Min() : range.Max()
            : null;
    }*/
}

file class Point
{
    public readonly long x;
    public readonly long y;
    public bool isRed { get; init; }

    public Point(long x, long y)
    {
        this.x = x;
        this.y = y;
        isRed = false;
    }
    public Point(string line)
    {
        var pos = line.Split(',').Select(long.Parse).ToArray();
        this.x = pos[0];
        this.y = pos[1];
        isRed = true;
    }
    public long GetRectangleSize(Point second)
    {
        var dx = (x < second.x ? second.x - x : x - second.x) + 1;
        var dy = (y < second.y ? second.y - y : y - second.y) + 1;
        return dx * dy;
    }

    public override string ToString() => $"Point({x},{y})({(isRed? "R": "G")})";
}

file enum Dir
{
    Unknown, Left, Right, Up, Down
}