using System.Text.RegularExpressions;
using AoCBase2;

namespace AoC2024.Days;

public class Day13 : IDay
{
    public static void RunAoC() => AocRuntime.Day<Day13>(13)
        .Callback(1, (d, t) => d.Part1Async(t.GetLines()))
        //.Callback(2, (d, t) => d.Part2())
        .Test("example")
        .Test("input").Skip()
        //.Part(1).Correct()
        //.Part(2).Correct()
        .Run();

    public string Part1Async(IEnumerable<string> input)
    {
        var clawRegex = new Regex($"Button (?<button>A|B): X\\+(?<x>\\d+), Y\\+(?<y>\\d+)");
        var prizeRegex = new Regex($"Prize: X=(?<x>\\d+), Y=(?<y>\\d+)");

        for (var i = 0; i < input.Count()/4; i++)
        {
            var takes = input.Skip(i * 4).Take(3).ToArray();
            var buttonA = PosFromMatch(clawRegex.Match(takes[0]));
            var buttonB = PosFromMatch(clawRegex.Match(takes[1]));
            var prize = PosFromMatch(prizeRegex.Match(takes[2]));
            var options = GetOptions(buttonA, buttonB, prize);
        }

        return null;
    }

    private (int x, int y) PosFromMatch(Match match) 
        => (int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));

    private IEnumerable<(int a, int b)>? GetOptions((int x, int y) a, (int x, int y) b, (int x, int y) result)
    {
        if (100 * (a.x + b.x) < result.x || 100 * (a.y + b.y) < result.y) yield break;
        
        var minA = Math.Min(result.x/a.x, result.y/a.y);
        
        yield return (5,5);
    }
}