using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2024.Days
{
    internal class Day1 : IDay
    {

        public static void RunAoC()
        {
            AocRuntime.Day<Day1>(1)
                .Callback(1, (d, t) => d.Part1(t.GetLinesAsync()))
                .Callback(2, (d, t) => d.Part2())
                .Test("example1", "Inputs/Day1/example1.txt")
                    .Part(1).Correct(11)
                    .Part(2)//.Correct(31)
                .Test("output", "Inputs/Day1/output.txt")
                    .Part(1)//.Correct(1873376)
                    .Part(2)//.Correct(18997088)
                .Run();
        }

        private List<int> left = new List<int>();
        private List<int> right = new List<int>();

        private async Task ReadLists(IAsyncEnumerable<string> lines){
            await foreach(var line in lines){
                var parts = line.Trim().Split("   ").Take(2).Select(int.Parse).ToArray();
                left.Add(parts[0]);
                right.Add(parts[1]);
            }

            left.Sort();
            right.Sort();

        }

        public async Task<string> Part1(IAsyncEnumerable<string> lines){
           await ReadLists(lines);

            var dif = 0;
            for(var i=0; i<left.Count; i++){
                dif += Math.Abs(left[i]-right[i]);
            }

            return dif.ToString();
        }

        public async Task<string> Part2(){
            var sum = 0;
            foreach(var l in left)
                sum += l * right.Count(r => r == l);
            return sum.ToString();
        }
    }
}
