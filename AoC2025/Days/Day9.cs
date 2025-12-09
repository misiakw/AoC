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

        var right = map.RayCast(7, 1, Dir.Down);
        var lala = map.RayCast(11, 7, Dir.Down);
        
        return "";
    }
}
//private long? RayCast(long x, long y, )

    file class Map
    {
        private LineDict _dictX;
        private LineDict _dictY;
        public Point[] Points { get; init; }
        public Map (IEnumerable<string> input)
        {
            _dictX = new LineDict();
            _dictY = new LineDict();
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
                    AddToDictionary(_dictX, point.x, previous.y, point.y);
                else
                    AddToDictionary(_dictY, point.y, previous.x, point.x);
                previous = point;
            }

            if (previous.x == first.x)
                AddToDictionary(_dictX, first.x, previous.y, first.y);
            else
                AddToDictionary(_dictY, first.y, previous.x, first.x);

            Points = points.ToArray();
        }

        private void AddToDictionary(LineDict dict, long key, long a, long b)
        {
            if (!dict.ContainsKey(key)) dict.Add(key, new List<(long s, long e)>());
            if (a < b)
                dict[key].Add((a, b));
            else
                dict[key].Add((b, a));
        }

        public long? RayCast(long x, long y, Dir dir)
        {
            var dict = (dir == Dir.Left || dir == Dir.Right) ?  _dictX : _dictY;
            var lineFunc = (dir == Dir.Left || dir == Dir.Right)
                ? new Func<(long s, long e), bool>(l => l.s <= y && l.e >= y)
                : new Func<(long s, long e), bool>(l => l.s <= x && l.e >= x);
            var isKeyInRange = dir switch
            {
                Dir.Right => new Func<long, bool>(k => k > x),
                Dir.Left => new Func<long, bool>(k => k < x),
                Dir.Down => new Func<long, bool>(k => k > y),
                Dir.Up => new Func<long, bool>(k => k < y),
            };
            
            //ToDo: sprawdzić czy nie jadę "po linii" - przykładowo 7,1 Down powinno zwracać 5 a nie 3
            // warto na dzien dobry sprawdzić czy punkt nie jest na linii i "przesunąc" go we własciwą stronę (min/max na koniec linii)

            var range = dict.Where(kv => isKeyInRange(kv.Key) && kv.Value.Any(lineFunc))
                .Select(kv => kv.Key).ToArray();
            return range.Length > 0 
                ? (dir == Dir.Right || dir == Dir.Down) ? range.Min() : range.Max() 
                :  null;
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
    Left, Right, Up, Down
}