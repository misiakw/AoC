using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;

namespace AdventOfCode.Day3
{
    [Day("Day3")]
    [Input("Test", typeof(Day3TestInput))]
    [Input("Result 159", typeof(Day3Test1Input))]
    [Input("Result 135", typeof(Day3Test2Input))]
    [Input("AOC", typeof(Day3AocInput))]
    public class Day3 : IDay
    {

        private IList<IDictionary<string, int>> steps = new List<IDictionary<string, int>>();
        private IDictionary<string, int> _map = new Dictionary<string, int>();
        private Wire wire1 = null;
        private Wire wire2 = null;

        public string Task1(IInput input)
        {
            

            foreach (var wire in input.Input.Split("\n").Select(x => x.Trim()).ToList())
            {
                if (wire1 == null)
                    wire1 = new Wire(wire);
                else
                    wire2 = new Wire(wire);
            }

            var crosses = wire1.Locations.Concat(wire2.Locations)
                .GroupBy(x => x.Key)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            var distance = crosses
                .Where(x => !x.Equals("0,0"))
                .Select(p => p.Split(",")
                    .Select(x => Math.Abs(long.Parse(x)))
                    .Sum())
                .OrderBy(p => p).First();

            return distance.ToString();
        }

        public string Task2(IInput input)
        {
            var dists = wire1.Locations.Concat(wire2.Locations)
                .GroupBy(x => x.Key)
                .Where(g => g.Count() > 1)
                .Where(x => !x.Key.Equals("0,0"))
                .Select(g => g.Select(x => x.distance).Sum())
                .ToList();


            return dists.Min().ToString();
        }

        private class WirePoint
        {
            public int X;
            public int Y;
            public string Key;
            public List<WirePoint> Near = new List<WirePoint>();
            public int distance = int.MaxValue;

            public WirePoint(int x, int y)
            {
                X = x;
                Y = y;
                Key = $"{x},{y}";
            }
        }

        private class Wire
        {
            public List<WirePoint> Locations = new List<WirePoint>();

            public Wire(string descr)
            {
                var point = new WirePoint(0, 0);
                Locations.Add(point);

                foreach (var part in descr.Split(","))
                {
                    var direction = part[0];
                    var len = int.Parse(part.Substring(1));

                    point = DrawStep(point, direction, len);
                }

                Locations = Locations.GroupBy(l => l.Key).Select(g => {
                    var result = g.First();
                    result.Near = g.SelectMany(x => x.Near).ToList();
                    return result;
                }).ToList();

                var searchList = Locations.Where(p => p.X == 0 && p.Y == 0).ToList();
                var counter = 0;
                while (searchList.Any())
                {
                    searchList = SetDistance(searchList, counter++);
                }
            }

            private WirePoint DrawStep(WirePoint point, char dir, int dist)
            {
                int dX = 0;
                int dY = 0;
                switch (dir)
                {
                    case 'R':
                        dX = 1;
                        break;
                    case 'L':
                        dX = -1;
                        break;
                    case 'U':
                        dY = 1;
                        break;
                    case 'D':
                        dY = -1;
                        break;
                }
                for (var i = 1; i <= dist; i++)
                {
                    var nX = point.X + dX;
                    var nY = point.Y + dY;

                    var newPoint = new WirePoint(nX, nY);
                    point.Near.Add(newPoint);
                    newPoint.Near.Add(point);

                    point = newPoint;
                    Locations.Add(point);
                }

                return point;
            }

            private List<WirePoint> SetDistance(List<WirePoint> points, int ctr)
            {
                var result = new List<WirePoint>();
                foreach (var point in points.Where(x => x.distance > ctr ))
                {
                    point.distance = ctr;
                    result.AddRange(point.Near.Where(n => n.distance > ctr + 1));
                }
                return result.GroupBy(r => r.Key).Select(g => g.First()).ToList();
            }
        }
    }
}