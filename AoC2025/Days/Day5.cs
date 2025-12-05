using AoCBase2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AoC2025.Days
{
    internal class Day5 : IDay
    {
        public static void RunAoC()
        {
            AocRuntime.Day<Day5>(5)
                .Callback(1, (d, t) => d.Part1(t.GetLines()))
                .Callback(2, (d, t) => d.Part2())
                .Test("example", "Inputs/Day5/example.txt")//.Part(1)//.Part(2)
                .Test("Pawla", "Inputs/Day5/pawla.txt")//.Part(1)//.Part(2)
                .Test("input", "Inputs/Day5/input.txt")//.Part(1)//.Part(2)
                .Run();
        }

        private IList<(long s, long e)> freshRange = new List<(long s, long e)>();
        private string Part1(IEnumerable<string> lines)
        {
            var linePos = 0;
            foreach (var line in lines)
            {
                linePos++;
                if (string.IsNullOrEmpty(line)) break;
                var ends = line.Trim().Split("-").Select(long.Parse).ToArray();

                var overlapping = freshRange.Where(r => (r.s <= ends[0] && r.e >= ends[0]) || (r.s <= ends[1] && r.e >= ends[1])).ToList();
                overlapping.AddRange(freshRange.Where(r => r.s >  ends[0] && r.e < ends[1]).ToList());
                if (overlapping.Any())
                {
                    var min = overlapping.Select(o => o.s).Min();
                    var max = overlapping.Select(o => o.e).Max();

                    var newRange = (s: min < ends[0] ? min : ends[0],
                        e: max > ends[1] ? max : ends[1]);
                    foreach (var o in overlapping)
                        freshRange.Remove(o);
                    freshRange.Add(newRange);
                }
                else
                    freshRange.Add((s: ends[0], e: ends[1]));
            }

            var ctr = 0;
            foreach (var line in lines.Skip(linePos))
            {
                var search = long.Parse(line);
                if (freshRange.Any(f => f.s <= search && f.e >= search))
                    ctr++;
            }

            return ctr.ToString();
        }

        private string Part2()
        {
            long ctr = 0;
            foreach (var r in freshRange)
                if(r.s == r.e)
                    ctr++;
                else
                    ctr += (r.e - r.s) + 1;
            return ctr.ToString();
        }
    }
}
