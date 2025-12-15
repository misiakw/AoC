using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;

namespace AoC2025.Days;
[DayNum(11)]
public class Day11: IDay
{
    public void RunAoC()
    {
        AocRuntime.Day<Day11>(11)
            .Callback(1, (d, t) => d.Part1(t.GetLines()))
            .Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example part1", "Inputs/Day11/example.txt").Skip(2) //.Part(1)//.Part(2)
            .Test("example part2", "Inputs/Day11/example2.txt").Skip() //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day11/input.txt").Skip(2) //.Part(1)//.Part(2)
            .Run();
    }

    public string Part1(IEnumerable<string> input)
    {
        var dict = ProcessInput(input);
        var paths = GetPossiblePaths("you", "", dict, "out");
        return paths.Distinct().Count().ToString();
    }
    
    public string Part2(IEnumerable<string> input)
    {
        return "";
    }

    private IDictionary<string, IList<string>> ProcessInput(IEnumerable<string> input)
    {
        var outDict = new Dictionary<string, IList<string>>();
        foreach (var line in input)
        {
            var parts =  line.Split(':').Select(l => l.Trim()).ToArray();
            outDict.Add(parts[0], parts[1].Split(" ").ToList());
        }

        return outDict;
    }

    private IEnumerable<string> GetPossiblePaths(string current, string path, IDictionary<string, IList<string>> dict, string target)
    {
        if (current == target) return new[] { path };
        if(current == "out") return new string[0]; //Part2 blocker
        var results = new List<string>();
        foreach (var next in dict[current])
        {
            var output = GetPossiblePaths(next, $"{path}>{current}", dict, target);
            results.AddRange(output);
        }
        return results;
    }
}