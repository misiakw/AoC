using System.Text.RegularExpressions;
using AoCBase2;

namespace AoC2024.Days;

public class Day3:IDay
{
    public static void RunAoC() => AocRuntime.Day<Day3>(3)
            .Callback(1, (d, t) => d.Part1(t.GetLinesAsync()))
            .Callback(2, (d, t) => d.Part2(t.GetLinesAsync()))
            .Test("example", "Inputs/Day3/example.txt")
                .Part(2).Skip()
            .Test("example2", "Inputs/Day3/example2.txt")
                .Part(1).Skip()
            .Test("input", "Inputs/Day3/output.txt")
//               .Part(1).Correct(174336360)
//               .Part(2).Correct(88802350)
            .Run();
    
    public async Task<string> Part1(IAsyncEnumerable<string> input){
        var regex = new Regex(@"mul\((\d*),(\d*)\)");

        var sum = 0;
        await foreach (var line in input)
        {
            var matches = regex.Matches(line);
            foreach (var match in matches.ToArray())
                sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }
        return sum.ToString();
    }

    public async Task<string> Part2(IAsyncEnumerable<string> input)
    {
        //(mul\((\d*),(\d*)\))|(
        var regex = new Regex(@"(mul\((\d*).(\d*)\)|(do\(\))|(don't\(\)))");

        var sum = 0;
        var enabled = true;
        await foreach(var line in input)
        {
            var matches = regex.Matches(line);
            foreach (var match in matches.ToArray())
            {
                if (match.Value.Equals("do()"))
                    enabled = true;
                else if (match.Value.Equals("don't()"))
                    enabled = false;
                else if (enabled)
                    sum += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
        }
        
        return sum.ToString();
    }
}