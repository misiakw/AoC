using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Common;

namespace AoC2022
{
    public class Day15 : DayBase
    {
        public Day15() : base(15)
        {
            Input("example1")
                .RunPart(1, 26)
                .RunPart(2, 56000011)
            .Input("output")
                .RunPart(1, 6275922);
        }

        public override object Part1(Input input)
        {
            var searchLine = input.Name == "example1" ? 10 : 2000000;
            input.Cache = PrepareInput(input.Lines);
            var sensors = (IList<Sensor>)((Tuple<List<Sensor>, List<Beacon>>)input.Cache).Item1;
            var beacons = (IList<Beacon>)((Tuple<List<Sensor>, List<Beacon>>)input.Cache).Item2;

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
            var maxVal = input.Name == "example1" ? 20 : 4000000;
            var sensors = (IList<Sensor>)((Tuple<List<Sensor>, List<Beacon>>)input.Cache).Item1;
            var beacons = (IList<Beacon>)((Tuple<List<Sensor>, List<Beacon>>)input.Cache).Item2;

            var map = new Array2D<byte>(0);

            foreach(var sensor in sensors.Take(3))
                map = sensor.FillMap(map, maxVal);

            Console.WriteLine(map.Draw(b => b == 0 ? ".": $"{(char)b}"));

            return maxVal;
        }

        private Tuple<List<Sensor>, List<Beacon>> PrepareInput(IList<string> lines){
            var sensors = new List<Sensor>();
            var beacons = new Dictionary<string, Beacon>();

            foreach(var line in lines){
                var parts = line.Replace("Sensor at ", "")
                    .Replace(" closest beacon is at ", "")
                    .Replace("x=", "").Replace(" y=", "").Split(":")
                    .Select(s => s.Split(",").Select(long.Parse).ToArray())
                    .ToArray();


                if(!beacons.ContainsKey($"{parts[1][0]}|{parts[1][1]}"))
                    beacons.Add($"{parts[1][0]}|{parts[1][1]}", new Beacon(parts[1][0], parts[1][1]));
                sensors.Add(new Sensor(parts[0][0], parts[0][1], beacons[$"{parts[1][0]}|{parts[1][1]}"]));
            }

            return Tuple.Create(sensors, beacons.Values.ToList());
        }

        protected class Sensor : Point
        {
            private Beacon beacon;
            public readonly long Range;
            public Sensor(long X, long Y, Beacon beacon) : base(X, Y)
            {
                this.beacon = beacon;
                this.Range = Math.Abs(beacon.X - X) + Math.Abs(beacon.Y - Y);
            }
            public bool IsInRange(long x, long y) => Math.Abs(x - X) + Math.Abs(y - Y) <= Range;
        }
        protected class Beacon : Point
        {
            public Beacon(long X, long Y) : base(X, Y)
            {
            }
        }
    }
}
