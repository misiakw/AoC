using AoCBase2;

namespace AoC2024.Days;

public class Day11 : IDay
{
    public static void RunAoC() => AocRuntime.Day<Day11>(11)
        .Callback(1, async (d, t) => d.Part1(await t.ReadLineAsync()))
        .Callback(2, async (d, t) => d.Part2(await t.ReadLineAsync()))
        .Test("example")
        .Test("input")
        //.Part(1).Correct(222461)
        //.Part(2).Correct()
        .Run();

    public string Part1(string input)
    {
        return GetSizeAfterRounds(
                input.Trim().Split(" ").Select(long.Parse).ToArray(), 25, false)
            .ToString();
    }

    public string Part2(string input)
    {
        return GetSizeAfterRounds(
                input.Trim().Split(" ").Select(long.Parse).ToArray(), 75, false)
            .ToString();
    }
    
    public long GetSizeAfterRounds(long[] nums, byte rounds, bool print = false)
    {
            
        IList<(long amount, long num)> stones = nums.Select(num => (1l, num)).ToList();
        IList<(long amount, long num)> stones2;
            
        for (var i = 0; i < rounds; i++)
        {
            stones2 = new List<(long amount, long num)>();
            foreach (var stone in stones)
            {
                if (stone.num == 0) stones2.Add((stone.amount, 1));
                else if (stone.num.ToString().Length % 2 == 0)
                {
                    var str = stone.num.ToString();
                    var len = str.Length / 2;
                    stones2.Add((stone.amount, long.Parse(str.Substring(0, len))));
                    stones2.Add((stone.amount, long.Parse(str.Substring(len))));
                }else stones2.Add((stone.amount, stone.num*2024));
            }
            stones = stones2.GroupBy(s => s.num)
                .Select(g => (g.Sum(s => s.amount), g.Key))
                .ToList();
            
            if(print)
                Console.WriteLine(string.Join(" ", stones));
        }
            
        return stones.Sum(s => s.amount);
    }
}