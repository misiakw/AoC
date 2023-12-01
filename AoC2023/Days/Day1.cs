using System.Collections.Generic;
using System.Linq;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day1 : AbstractDay<int, IComparableInput<int>>
    {
        public override int Part1(IComparableInput<int> input)
        {
            var t = input.ReadLines();
            t.Wait();
            var lines = t.Result.ToArray();

            var sum = 0;
            foreach (var line in lines)
            {
                var tmp = (line.First(c => c >= '0' && c <= '9') - '0') * 10;
                tmp += (line.Last(c => c >= '0' && c <= '9') - '0');
                sum += tmp;
            }
            return sum;
        }

        public override int Part2(IComparableInput<int> input)
        {
            var replacer = new RollReplacer(new List<(string, string)>
            {
                ("one", "1"), ("two", "2"), ("three", "3"), ("four", "4"),
                ("five", "5"), ("six", "6"), ("seven", "7"), ("eight", "8"), ("nine", "9")
            });

            var t = input.ReadLines();
            t.Wait();
            var lines = t.Result.ToArray();

            var sum = 0;
            foreach (var line in lines)
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
            return sum;
        }

        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day1/example1.txt")
               .Part1(142);
            builder.New("example2", "./Inputs/Day1/example2.txt")
               .Part2(281);
            builder.New("output", "./Inputs/Day1/output.txt")
                .Part1(55834)
                .Part2(53221);
        }
    }
}
