using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;
using AoC.Common.Abstractions;

namespace AoC2025.Days
{
    [DayNum(6)]
    internal class Day6 : IDay
    {
        public void RunAoC()
        {
            AocRuntime.Day<Day6>(6)
                .Callback(1, (d, t) => d.Part1(t.GetLines()))
                .Callback(2, (d, t) => d.Part2(t.GetMap()))
                .Test("example", "Inputs/Day6/example.txt") //.Part(1)//.Part(2)
                .Test("input", "Inputs/Day6/input.txt") //.Part(1)//.Part(2)
                .Run();
        }

        private string Part1(IEnumerable<string> lines)
        {
            var dict = new Dictionary<int, IList<long>>();
            int? len = null;
            var sum = 0l;
            foreach (var line in lines)
            {
                if (line[0] == '*' || line[0] == '+')
                {
                    var actions = line.Split(' ').Where(n => !string.IsNullOrEmpty(n)).ToArray();
                    for (var pos = 0; pos < len; pos++)
                    {
                        var midsum = actions[pos] == "*" ? 1l : 0l;
                        foreach (var num in dict[pos])
                            midsum = actions[pos] == "*" ? midsum * num : midsum + num;
                        sum += midsum;
                    }
                    return sum.ToString();
                }
                else
                {
                    var numbers = line.Split(' ').Where(n => !string.IsNullOrEmpty(n)).Select(long.Parse).ToArray();
                    if (!len.HasValue)
                    {
                        len = numbers.Length;
                        for (var pos = 0; pos < len; pos++)
                            dict.Add(pos, new List<long>());
                    }
                    for (var pos = 0; pos < len; pos++)
                        dict[pos].Add(numbers[pos]);
                }
            }


            return "";
        }

        private string Part2(IMap<char> sheet)
        {
            var col = sheet.Width-1;
            var endOfNum = sheet.Height - 2;
            var sum = 0L;

            var nums = new List<long>();
            while(col >= 0)
            {
                var doMath = sheet[col, endOfNum+1] != ' ';

                var num = 0L;
                for (var y = 0; y <= endOfNum; y++)
                    if(sheet[col, y] != ' ')
                        num = num * 10 + sheet[col, y]-'0';
                nums.Add(num);

                if (doMath)
                {
                    if (sheet[col, endOfNum + 1] == '+')
                        sum += nums.Sum();
                    else
                    {
                        var i = 1L;
                        foreach (var x in nums)
                            i *= x;
                        sum += i;
                    }
                    nums = new List<long>();
                    col--;
                }
                col--;
            }


            return sum.ToString();
        }
    }
}
