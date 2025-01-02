using AoCBase2;

namespace AoC2024.Days;

public class Day19: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day19>(19)
        .Callback(1, (d, t) => d.Part1(t.GetLines())).Skip()
        .Callback(2, (d, t) => d.Part2(t.GetLines()))
        .Test("example")
        .Test("input")
        //.Part(1).Correct(363)
        //.Part(2).Correct()
        .Run();
    
    public string Part1(IEnumerable<string> input)
    {
        IList<string> towels = input.First().Split(',').Select(s => s.Trim()).ToList();
        int ctr = 0;

        foreach (var pattern in input.Skip(2))
            if (CanMake(towels, pattern))
                ctr++;
        return ctr.ToString();
    }
    public string Part2(IEnumerable<string> input)
    {
        IList<string> towels = input.First().Split(',').Select(s => s.Trim()).ToList();
        int ctr = 0;

        foreach (var pattern in input.Skip(2))
            if(CanMake(towels, pattern))
                ctr += AmountOfPatternOptions(towels, pattern);
        return ctr.ToString();
    }

    private bool CanMake(IList<string> towels, string pattern)
    {
        foreach (var towel in towels.Where(pattern.StartsWith))
        {
            if (towel == pattern)
                return true;
            if (CanMake(towels, pattern.Substring((towel.Length))))
                return true;
        }

        return false;
    }

    private int AmountOfPatternOptions(IList<string> towels, string pattern)
    {
        var ctr = 0;
        foreach (var towel in towels.Where(pattern.StartsWith))
        {
            if (towel == pattern)
                ctr++;
            else
                ctr += AmountOfPatternOptions(towels, pattern.Substring((towel.Length)));
        }

        return ctr;
    }
}