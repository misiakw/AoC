using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2024.Days
{
    public class Day25 : IDay
    {
        public static void RunAoC() => AocRuntime.Day<Day25>(25, t => new Day25(t.GetLines()))
                .Callback(1, (d, t) => d.Part1())
                //.Callback(2, (d, t) => d.Part2())
                .Test("example")
                .Test("input")
                //.Part(1).Correct(3525)
                //.Part(2).Correct()
                .Run();

        private IList<int[]> Locks = new List<int[]>();
        private IList<int[]> Keys = new List<int[]>();
        public Day25(IEnumerable<string> lines)
        {
            string[] tmp = null;
            bool? islock = null;
            foreach (string line in lines)
            {
                if (islock == null)
                {
                    islock = line[0] == '#';
                    tmp = new string[line.Length];
                    for (var i = 0; i < line.Length; i++)
                        tmp[i] = string.Empty;
                }
                for (var i = 0; i < line.Length; i++)
                    tmp[i] += line[i];
                

                if (string.IsNullOrEmpty(line))
                {
                    if(islock.Value)
                        Locks.Add(Decode(tmp));
                    else
                        Keys.Add(Decode(tmp));
                    islock = null;
                }
            }
            if (islock.Value)
                Locks.Add(Decode(tmp));
            else
                Keys.Add(Decode(tmp));
        }

        private int[] Decode(string[] colls)
            => colls.Select(c => c.Count(c => c == '#') - 1).ToArray();

        public string Part1()
        {
            var ctr = 0;
            foreach (var l in Locks)
                foreach (var k in Keys)
                    if (!IsOverlap(k, l))
                        ctr++;

            return ctr.ToString();
        }

        private bool IsOverlap(int[] k, int[] l)
        {
            for (int i = 0; i < k.Length; i++)
                if (l[i] + k[i] > 5) 
                    return true;
            return false;
        }
    }
}
