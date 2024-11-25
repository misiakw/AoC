using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC.Common;
using AoCBase2;


namespace AoC2023.Days
{
    public class Day1
    {
        public string Part1(IAsyncEnumerable<string> lines)
        {
            var sum = 0;
            foreach (var line in lines.ToEnumerable())
            {
                var tmp = (line.First(c => c >= '0' && c <= '9') - '0') * 10;
                tmp += (line.Last(c => c >= '0' && c <= '9') - '0');
                sum += tmp;
            }
            return sum.ToString();
        }

        public async Task<string> Part2Async(IAsyncEnumerable<string> lines)
        {
            var replacer = new RollReplacer(new List<(string, string)>
            {
                ("one", "1"), ("two", "2"), ("three", "3"), ("four", "4"),
                ("five", "5"), ("six", "6"), ("seven", "7"), ("eight", "8"), ("nine", "9")
            });

            var sum = 0;
            await foreach (var line in lines)
            {
                var left = -1;
                var right = -1;
                var len = 1;
                var tmp = string.Empty;
                var lineLen = line.Length;

                while(left == -1 || right == -1)
                {
                    if (left == -1)
                    {
                        tmp = replacer.Replace(line.Substring(0, len));
                        if (tmp.Any(c => c >= '0' && c <= '9'))
                            left = (tmp.First(c => c >= '0' && c <= '9') - '0') * 10;
                    }
                    if(right == -1)
                    {
                        tmp = replacer.Replace(line.Substring(lineLen - len));
                        if (tmp[0] >= '0' && tmp[0] <= '9')
                            right = tmp[0] - '0';
                    }
                    len++;
                }

                sum += left + right;
            }
            return sum.ToString();
        }
        public static void ProceedAoC()
        {
            AocRuntime.Day<Day1>(1)
                .Callback(1, (d, t) => d.Part1(t.GetLinesAsync()))
                .Callback(2, (d, t) => d.Part2Async(t.GetLinesAsync()))
                .Test("example1", "./Inputs/Day1/example1.txt").Part(1).Correct(142)
                .Test("example2", "./Inputs/Day1/example2.txt").Part(2).Correct(281)
                .Test("output", "./Inputs/Day1/output.txt")
                    .Part(1).Correct(55834)
                    .Part(2).Correct(53221)
                .Run();
        }
    }
}
