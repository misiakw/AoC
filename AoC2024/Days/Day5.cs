using System.Data;
using AoCBase2;

namespace AoC2024.Days;

public class Day5 :IDay
{
    public static void RunAoC()=> AocRuntime.Day<Day5>(5, (n, f) => new Day5(f))
        .Callback(1, (d, t) => d.Part1())
        .Callback(2, (d, t) => d.Part2())
        .Test("example")
        //.Part(1).Skip()
        .Test("input")
//               .Part(1).Correct(7307)
//               .Part(2).Correct(88802350)
        .Run();

    private IDictionary<int, IList<int>> Rules = new Dictionary<int, IList<int>>();
    private IList<IList<int>>  Updates = new List<IList<int>>();

    public Day5(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var i = 0;
        while (!string.IsNullOrEmpty(lines[i]))
        {
            var parts = lines[i++].Split('|').Select(s => int.Parse(s.Trim())).ToArray();
            if(!Rules.ContainsKey(parts[0])) Rules.Add(parts[0], new List<int>());
            Rules[parts[0]].Add(parts[1]);
        }
        i++;
        for(; i<lines.Length; i++)
            Updates.Add(lines[i].Split(',').Select(int.Parse).ToList());
    }

    public string Part1()
    {
        var sum = 0;
        foreach (var update in Updates)
            if (IsCorrect(update))
                sum += update.Skip((update.Count - 1) / 2).First();
        return sum.ToString();
    }
    
    public string Part2()
    {
        var sum = 0;
        foreach (var update in Updates)
            if (!IsCorrect(update))
                sum += GetFixedMid(update);

        return sum.ToString();
    }

    private bool IsCorrect(IList<int> update)
    {
        var previous = new List<int>();
        foreach (var page in update)
        {
            if(Rules.ContainsKey(page))
                if (previous.Intersect(Rules[page]).Any())
                    return false;
            previous.Add(page);
        }
        return true;
    }

    private int GetFixedMid(IList<int> update)
    {
        var outputStack = new Stack<int>();
        var moveStack = new Stack<int>();
        foreach (var page in update)
        {
            if (Rules.ContainsKey(page))
            {
                var rule = Rules[page];
                while (outputStack.Intersect(rule).Any())
                {
                    moveStack.Push(outputStack.Pop());
                }

                outputStack.Push(page);
                while (moveStack.Any())
                    outputStack.Push(moveStack.Pop());
            }
            else
                outputStack.Push(page);
        }
        return outputStack.Skip((outputStack.Count - 1) / 2).First();
    }
}