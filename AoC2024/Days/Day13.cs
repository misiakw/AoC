using System.Text.RegularExpressions;
using AoCBase2;

namespace AoC2024.Days;

public class Day13 : IDay
{
    public static void RunAoC() => AocRuntime.Day<Day13>(13)
        .Callback(1, (d, t) => d.Part1(t.GetLines()))
        //.Callback(2, (d, t) => d.Part2())
        .Test("example")
        .Test("input")
        //.Part(1).Correct(37901)
        //.Part(2).Correct()
        .Run();

    public string Part1(IEnumerable<string> input)
    {
        var clawRegex = new Regex($"Button (?<button>A|B): X\\+(?<x>\\d+), Y\\+(?<y>\\d+)");
        var prizeRegex = new Regex($"Prize: X=(?<x>\\d+), Y=(?<y>\\d+)");

        int sum = 0;
        for (var i = 0; i <= input.Count()/4; i++)
        {
            var takes = input.Skip(i * 4).Take(3).ToArray();
            var buttonA = PosFromMatch(clawRegex.Match(takes[0]));
            var buttonB = PosFromMatch(clawRegex.Match(takes[1]));
            var prize = PosFromMatch(prizeRegex.Match(takes[2]));
            var options = GetOptions(buttonA, buttonB, prize);
            if (options.Any())
                sum += options.Select(o => o.a * 3 + o.b).Min();
        }

        return sum.ToString();
    }

    private (int a, int b) PosFromMatch(Match match) 
        => (int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));

    private IEnumerable<(int a, int b)>? GetOptions((int x, int y) a, (int x, int y) b, (int x, int y) result)
    {
        for (var ia = 0; ia <= 100; ia++)
        {
            for (var ib = 0; ib <= 100; ib++)
            {
                if (ia * a.x + ib * b.x == result.x && ia * a.y + ib * b.y == result.y)
                    yield return (ia, ib);
                if (ia*a.x + ib*b.x > result.x && ia*a.y + ib*b.y > result.y)
                    break;
            }
        }
    }
}