using AoCBase2;

namespace AoC2024.Days;

public class Day7: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day7>(7)
        .Callback(1, (d, t) => d.Part1(t.GetLinesAsync()))
        .Callback(2, (d, t) => d.Part2(t.GetLinesAsync()))
        .Test("example")
        .Test("input")
        //.Part(1).Correct(1545311493300)
        //.Part(2).Correct(169122112716571)
        .Run();

    public async Task<string> Part1(IAsyncEnumerable<string> input)
    {
        var sum = 0l;
        await foreach (var line in input)
        {
            var split = line.Trim().Split(':');
            var final = long.Parse(split[0]);
            var tiles = split[1].Trim().Split(' ').Select(long.Parse).ToArray();

            if (CanCalibrate(final, tiles.First(), tiles.Skip(1)))
                sum += final;
        }

        return sum.ToString();
    }
    
    public async Task<string> Part2(IAsyncEnumerable<string> input)
    {
        var sum = 0l;
        await foreach (var line in input)
        {
            var split = line.Trim().Split(':');
            var final = long.Parse(split[0]);
            var tiles = split[1].Trim().Split(' ').Select(long.Parse).ToArray();

            if (CanCalibrateWithConcat(final, tiles.First(), tiles.Skip(1)))
                sum += final;
        }

        return sum.ToString();
    }

    private bool CanCalibrate(long final, long sum, IEnumerable<long> remains)
    {
        if (remains.Count() == 0 ) return sum == final;
        if (sum > final) return false;
        if (CanCalibrate(final, sum + remains.First(), remains.Skip(1))) return true;
        if (CanCalibrate(final, sum * remains.First(), remains.Skip(1))) return true;
        return false;
    } 
    private bool CanCalibrateWithConcat(long final, long sum, IEnumerable<long> remains)
    {
        if (remains.Count() == 0 ) return sum == final;
        if (sum > final) return false;
        if (CanCalibrateWithConcat(final, sum + remains.First(), remains.Skip(1)))
            return true;
        if (CanCalibrateWithConcat(final, sum * remains.First(), remains.Skip(1)))
            return true;
        if (CanCalibrateWithConcat(final, long.Parse($"{sum}{remains.First()}"), remains.Skip(1)))
            return true;
        
        return false;
    } 
}