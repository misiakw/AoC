using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;
using Range = AoC.Common.Range;


namespace AoC2023.Days
{
    public class Day5 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day5/example1.txt")
                .Part1(35)
                .Part2(46);
            builder.New("output", "./Inputs/Day5/output.txt")
                .Part1(510109797)
                .Part2(9622622);
        }

        public override long Part1(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var seeds = lines[0].Split(" ").Skip(1).Select(x => new Seed(long.Parse(x), 1)).ToArray();

            var i = 2;
            //SeedToSoil
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Id, (s, v) => { s.Soil = v; return s; });
            i++;
            //SoilToFert
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Soil, (s, v) => { s.Fert = v; return s; });
            i++;
            //FertToWater
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Fert, (s, v) => { s.Water = v; return s; });
            i++;
            //WaterToLight
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Water, (s, v) => { s.Ligth = v; return s; });
            i++;
            //LightToTemp
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Ligth, (s, v) => { s.Temp = v; return s; });
            i++;
            //TempToHumidity
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Temp, (s, v) => { s.Humid = v; return s; });
            i++;
            //HumidityToLocation
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Humid, (s, v) => { s.Location = v; return s; });

            return seeds.Min(s => s.Location.Min);
        }

        private Mapper GetMapper(IList<string> lines, ref int pos)
        {
            var buff = new List<string>();
            while (pos < lines.Count && !string.IsNullOrEmpty(lines[pos]))
                buff.Add(lines[pos++]);
            return new Mapper(buff.ToArray());
        }
        private class Mapper
        {
            private readonly IList<(Range, Range)> _maps = new List<(Range, Range)>();
            public readonly string Name;

            public Mapper(string[] lines)
            {
                Name = lines[0].Split(" ")[0];
                var map = new List<(long, long, long)>();
                foreach (var line in lines.Skip(1))
                {
                    var tiles = line.Split(" ").Select(long.Parse).ToArray();
                    map.Add((tiles[1], tiles[0], tiles[2]));
                    var sourceRange = new Range(tiles[1], tiles[1] + tiles[2] - 1);
                    var destRange = new Range(tiles[0], tiles[0] + tiles[2] - 1);
                    _maps.Add((sourceRange, destRange));
                }
            }

            //source, dest
            public (Range, Range)[] GetMapRanges(Range source)
                => _maps.Where(m => m.Item1.Overlap(source))
                    .ToArray();
        }

        private Seed[] LoadAndMap(Seed[] seeds, ref int pos, IList<string> lines, Func<Seed, Range> source, Func<Seed, Range, Seed> dest)
        {
            var mapper = GetMapper(lines, ref pos);
            var results = new List<Seed>();
            for (var x = 0; x < seeds.Count(); x++)
            {
                var sourceRange = source(seeds[x]);
                var ranges = mapper.GetMapRanges(sourceRange);

                if(!ranges.Any())
                    results.Add(dest(seeds[x], new Range(sourceRange.Min, sourceRange.Max)));
                else
                {
                    var maps = ranges.OrderBy(r => r.Item1.Min).ToList();
                    var currentStart = sourceRange.Min;
                    foreach(var map in maps) {
                        //nonmapped start
                        if(currentStart < map.Item1.Min)
                        {
                            var toSkip = map.Item1.Min - currentStart;
                            results.Add(dest(seeds[x], new Range(currentStart, currentStart + toSkip)));
                            currentStart += toSkip;
                        }
                        //map captured range
                        var startOffset = currentStart - map.Item1.Min;
                        var startPoint = map.Item2.Min + startOffset;
                        var length = (sourceRange.Max <= map.Item1.Max)
                            ? sourceRange.Max - currentStart
                            : map.Item1.Max - currentStart;

                        results.Add(dest(seeds[x], new Range(startPoint, startPoint + length)));
                        currentStart += length;
                    }
                    //verify if nonmapped part exist
                    if(currentStart < sourceRange.Max)
                        results.Add(dest(seeds[x], new Range(currentStart, sourceRange.Max)));
                }
            }
            return results.ToArray();
        }

        public override long Part2(IComparableInput<long> input)
        {
            var lines = ReadLines(input);
            var nums = lines[0].Split(" ").Skip(1).Select(long.Parse).ToArray();
            var seedList = new List<Seed>();
            for (int x = 0; x < nums.Length; x += 2)
                seedList.Add(new Seed(nums[x], nums[x + 1]));
            var seeds = seedList.ToArray();

            var i = 2;
            //SeedToSoil
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Id, (s, v) => { s.Soil = v; return s; });
            i++;
            //SoilToFert
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Soil, (s, v) => { s.Fert = v; return s; });
            i++;
            //FertToWater
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Fert, (s, v) => { s.Water = v; return s; });
            i++;
            //WaterToLight
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Water, (s, v) => { s.Ligth = v; return s; });
            i++;
            //LightToTemp
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Ligth, (s, v) => { s.Temp = v; return s; });
            i++;
            //TempToHumidity
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Temp, (s, v) => { s.Humid = v; return s; });
            i++;
            //HumidityToLocation
            seeds = LoadAndMap(seeds, ref i, lines, s => s.Humid, (s, v) => { s.Location = v; return s; });

            return seeds.Any() ? seeds.Min(s => s.Location.Min): -1;
        }

        private struct Seed
    {
            public Range Id, Soil, Fert, Water, Ligth, Temp, Humid, Location;
            public long Range;
            public Seed(long id, long range)
            {
                Id = new Range(id, id + range - 1);
                Range = range;
                Soil = new Range(-1, -1);
                Fert = new Range(-1, -1);
                Water = new Range(-1, -1);
                Ligth = new Range(-1, -1);
                Temp = new Range(-1, -1);
                Humid = new Range(-1, -1);
                Location = new Range(-1, -1);
            }
        }
    }
}
