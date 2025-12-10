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
            .Callback(1, (d, t) => d.Part1(t.GetLines()))
            //.Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example", "Inputs/Day9/example.txt") //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day9/input.txt").Skip() //.Part(1)//.Part(2)
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

        var rigdsfsdfht = map.RayCast(4, 3, Dir.Down); //powinno byc 5 prosty cast w dol
        var right = map.RayCast(7, 1, Dir.Down); //powinno byc 5 cast w dol po linii ale ma 
        var lala = map.RayCast(11, 7, Dir.Down); //powinno byc null
        var sdafs = map.RayCast(2, 11, Dir.Down); //Powinno być null  //
        var dsjjsjf = map.RayCast(9, 1, Dir.Down); //powinno być 11 
        
        return "";
    }
}
//private long? RayCast(long x, long y, )

    file class Map
    {
        private IDictionary<long, List<Line>> _dictConstX;
        private IDictionary<long, List<Line>> _dictConstY;
        public Point[] Points { get; init; }
        public Map (IEnumerable<string> input)
        {
            _dictConstX = new Dictionary<long, List<Line>>();
            _dictConstY = new Dictionary<long, List<Line>>();
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
                    AddToDictionary(_dictConstX, point.x, previous.y, point.y);
                else
                    AddToDictionary(_dictConstY, point.y, previous.x, point.x);
                previous = point;
            }

            if (previous.x == first.x)
                AddToDictionary(_dictConstX, first.x, previous.y, first.y);
            else
                AddToDictionary(_dictConstY, first.y, previous.x, first.x);

            Points = points.ToArray();
        }

        private void AddToDictionary(IDictionary<long, List<Line>> dict, long key, long a, long b)
        {
            if (!dict.ContainsKey(key)) dict.Add(key, new List<Line>());
            if (a < b)
                dict[key].Add(new Line(a, b));
            else
                dict[key].Add(new Line(b, a));
        }

        public long? RayCast(long x, long y, Dir dir)
        {
            IDictionary<long, List<Line>> dict = null;
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
                :  null;
        }
    }

    file class Line
    {
        public long s { get; init; }
        public long e { get; init; }
        public Dir inside = Dir.Unknown;

        public Line(long s, long e)
        {
            this.s = s;
            this.e = e;
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

file enum Dir
{
    Unknown, Left, Right, Up, Down
}