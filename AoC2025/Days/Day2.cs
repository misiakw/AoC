using AoC.Base;
using AoC.Base.Abstraction;

namespace AoC2025.Days
{
    internal class Day2: IDay
    {
        public void RunAoC()
        {
            AocRuntime.Day<Day2>(2)
                .Callback(1, (d, t) => d.Part1(t.GetLines()))
                .Callback(2, (d, t) => d.Part2(t.GetLines()))
                .Test("example", "Inputs/Day2/example.txt")
                .Test("input", "Inputs/Day2/input.txt")
                .Run();
        }

        private string Part1(IEnumerable<string> lines)
            => Process(lines.First(), false).ToString();
        private string Part2(IEnumerable<string> lines)

            => Process(lines.First(), true).ToString();
        private long Process(string input, bool multiprocess)
        {
            var ranges = input.Split(',').ToList();
            var sum = 0l;
            foreach (var range in ranges)
            {
                var ends = range.Split('-').Select((long.Parse)).ToArray();

                for (var i = ends[0]; i <= ends[1]; i++)
                {
                    if (isFunny(i, multiprocess))
                    {
                        sum += i;
                    }
                }
            }
            return sum;
        }

        private bool isFunny(long number, bool multiRepeat = false)
        {
            var len = (int)Math.Log10(number) + 1;

            var last = number % 10;
            var numArr = number / 10;
            var part = last;
            var mul = 10;
            for(var i=1; i<=len/2; i++)
            {
                var curr = numArr % 10;
                if(curr == last) //check if looped
                {
                    var loopLen = (int)Math.Log10(part) + 1;
                    if(len%loopLen == 0) // check if chunk size ok
                    {
                        var result = part;
                        if(multiRepeat)
                            for (var j = 1; j < len / loopLen; j++)
                                result = result * mul + part;
                        else
                            result = result * mul + part;
                        if (result == number)
                            return true; ;
                    }
                }
                part = curr * mul + part;
                mul *= 10;
                numArr /= 10;
            }

            return false;
        }
    }
}
