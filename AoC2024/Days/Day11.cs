using AoCBase2;

namespace AoC2024.Days;

public class Day11 : IDay
{
    public static void RunAoC() => AocRuntime.Day<Day11>(11)
        .Callback(1, async (d, t) => d.Part1(await t.ReadLineAsync()))
        .Callback(2, async (d, t) => d.Part2(await t.ReadLineAsync()))
        .Test("example")
        .Test("input").Skip(2)
        //.Part(1).Correct(222461)
        //.Part(2).Correct()
        .Run();

    private IList<Stone> Stones;
    public string Part1(string input)
    {
        return input.Trim().Split(" ").Select(s => new Stone(s).GetSizeAfterRounds(25)).Sum().ToString();
    }

    public string Part2(string input)
    {
        var stone = new Stone(7).GetSizeAfterRounds(75);//, true);
        return stone.ToString();
    }

    private class Stone
    {
        public Stone(long newLong)
        {
            LongVal = newLong;
            StringVal = LongVal.ToString();
        }
        public Stone(string newString) : this(long.Parse(newString)){ }

        private Stone SetVal(long newVal)
        {
            LongVal = newVal;
            StringVal = LongVal.ToString();
            return this;
        }
        private Stone SetVal(string newVal) => SetVal(long.Parse(newVal));
        public long LongVal { get; protected set; }
        public string StringVal { get; protected set; }

        public long GetSizeAfterRounds(byte rounds, bool print = false)
        {
            // if(LongVal == 0) return SetVal(1).GetSizeAfterRounds(--rounds);
            // if (StringVal.Length % 2 == 0)
            // {
            //     var len = StringVal.Length / 2;
            //     var left = StringVal.Substring(0, len);
            //     var right = StringVal.Substring(len);
            //     return SetVal(left).GetSizeAfterRounds(--rounds) + new Stone(right).GetSizeAfterRounds(rounds);
            // }
            // return SetVal(LongVal*2024).GetSizeAfterRounds(--rounds);
            
            IList<Stone> stones = new List<Stone>() { this };
            IList<Stone> stones2;
            
            
            for (var i = 0; i < rounds; i++)
            {
                stones2 = new List<Stone>();
                foreach (var stone in stones)
                {
                    if (stone.LongVal == 0) stones2.Add(stone.SetVal(1));
                    else if (stone.StringVal.Length % 2 == 0)
                    {
                        var len = stone.StringVal.Length / 2;
                        var left = stone.StringVal.Substring(0, len);
                        var right = stone.StringVal.Substring(len);
                        stones2.Add(stone.SetVal(left));
                        stones2.Add(new Stone(right));
                    }else stones2.Add(stone.SetVal(stone.LongVal*2024));
                }
                if(print)
                    Console.WriteLine(string.Join(" ", stones2.Select(s => s.LongVal)));
                stones = stones2;
            }
            
            return stones.Count;
        }
    }
}