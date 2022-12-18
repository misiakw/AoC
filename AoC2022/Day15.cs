using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;
using Range = AoC.Common.Range;

namespace AoC2022
{
    public class Day15 : DayBase
    {
        public Day15() : base(15)
        {
            Input("example1")
                .RunPart(1, 26)
                .RunPart(2, 56000011L)
            .Input("output")
                .RunPart(1, 6275922)
                .RunPart(2, 11747175442119L);
        }

        public override object Part1(Input input)
        {
            var searchLine = input.Name == "example1" ? 10 : 2000000;
            input.Cache = PrepareInput(input.Lines);
            var sensors = (IList<Sensor>)((Tuple<List<Sensor>, List<Point>>)input.Cache).Item1;
            var beacons = (IList<Point>)((Tuple<List<Sensor>, List<Point>>)input.Cache).Item2;

            var considerable = sensors.Where(s => Math.Abs(searchLine - s.Y) <= s.Range).ToList();

            var line = new Dictionary<long, bool>();
            foreach(var sensor in considerable){
                for(long x = sensor.X; sensor.IsInRange(x, searchLine); x++)
                    if(!line.ContainsKey(x))
                        line.Add(x, true);
                
                for(long x = sensor.X; sensor.IsInRange(x, searchLine); x--)
                    if(!line.ContainsKey(x))
                        line.Add(x, true);
            }

            return line.Count - beacons.Where(b => b.Y == searchLine && line.Keys.Contains(b.X)).Count();
        }

        public override object Part2(Input input)
        {
            var maxVal = input.Name == "example1" ? 20L : 4000000L;
            var sensors = (IList<Sensor>)(((Tuple<List<Sensor>, List<Point>>)input.Cache).Item1 ?? new List<Sensor>());

            var map = new Dictionary<long, IList<Range>>();

            foreach(var sensor in sensors)
                sensor.AddRangeToMap(map, maxVal);

            var kv = map.First(x => x.Value.Count > 1);

            var y = kv.Key;
            var x = kv.Value.Order().First().Max+1;

            return x*4000000 + y;
        }

        private Tuple<List<Sensor>, List<Point>> PrepareInput(IList<string> lines){
            var sensors = new List<Sensor>();
            var beacons = new Dictionary<string, Point>();

            foreach(var line in lines){
                var parts = line.Replace("Sensor at ", "")
                    .Replace(" closest beacon is at ", "")
                    .Replace("x=", "").Replace(" y=", "").Split(":")
                    .Select(s => s.Split(",").Select(long.Parse).ToArray())
                    .ToArray();


                if(!beacons.ContainsKey($"{parts[1][0]}|{parts[1][1]}"))
                    beacons.Add($"{parts[1][0]}|{parts[1][1]}", new Point(parts[1][0], parts[1][1]));
                sensors.Add(new Sensor(parts[0][0], parts[0][1], beacons[$"{parts[1][0]}|{parts[1][1]}"]));
            }

            return Tuple.Create(sensors, beacons.Values.ToList());
        }

        protected class Sensor : Point
        {
            private Point beacon;
            public readonly long Range;
            public Sensor(long X, long Y, Point beacon) : base(X, Y)
            {
                this.beacon = beacon;
                this.Range = Math.Abs(beacon.X - X) + Math.Abs(beacon.Y - Y);
            }
            public bool IsInRange(long x, long y) => Math.Abs(x - X) + Math.Abs(y - Y) <= Range;

            public void AddRangeToMap(IDictionary<long, IList<Range>> map, long delmiter){
                var startY = WithDelmiter(Y-Range, delmiter);
                var stopY = WithDelmiter(Y+Range, delmiter);
                var minX = WithDelmiter(X-Range, delmiter);
                var maxX = WithDelmiter(X+Range, delmiter);
                for(var line=startY; line<=Y; line++){
                    var diff = line-Y + Range;
                    AddRangeToMap(map, new Range(WithDelmiter(X-diff, delmiter), WithDelmiter(X+diff, delmiter)), line);
                }
                for(var line=Y+1; line<=stopY; line++){
                    var diff = Range - (line - Y);
                    AddRangeToMap(map, new Range(WithDelmiter(X-diff, delmiter), WithDelmiter(X+diff, delmiter)), line);
                }
            }

            private void AddRangeToMap(IDictionary<long, IList<Range>> map, Range range, long line){
                if (!map.ContainsKey(line))
                        map.Add(line, new List<Range>());

                var overlapping = map[line].Where(r => r.Overlap(range)).ToList();
                foreach(var overlap in overlapping){
                    map[line].Remove(overlap);
                    range = range + overlap;
                }

                var previous = map[line].FirstOrDefault(r => r.Max+1 == range.Min);
                if(previous != null){
                    map[line].Remove(previous);
                    range = range + previous;
                }
                var next = map[line].FirstOrDefault(r => r.Min-1 == range.Max);
                if(next != null){
                    map[line].Remove(next);
                    range = range + next;
                }

                map[line].Add(range);
            }
            private long WithDelmiter(long val, long delmiter){
                if (val < 0) return 0;
                if(val > delmiter) return delmiter;
                return val;
            }
        }
    }
}
