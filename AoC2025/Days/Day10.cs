using System.Text;
using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;

namespace AoC2025.Days;

[DayNum(10)]
file class Day10 : IDay
{
    public void RunAoC()
    {
        AocRuntime.Day<Day10>(10)
            //.Callback(1, (d, t) => d.Part1(t.GetLines()))
            .Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example", "Inputs/Day10/example.txt") //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day10/input.txt").Skip() //.Part(1)//.Part(2)
            .Run();
    }

    public string Part1(IEnumerable<string> lines)
    {
        var sum = 0;
        foreach (var line in lines)
            sum += new Part1Sover(line).GetResult();
        return sum.ToString();
    }
    public string Part2(IEnumerable<string> lines)
    {
        foreach (var line in lines)
            Console.WriteLine(new Part2Sover(line).Solve(0, 0));
        return"";
    }
}

file class Part1Sover
{
    private int lights = 0;
    private int nonOptimalResult;
    private int[] inputMasks;

    public Part1Sover(string line)
    {
        var parts = line.Split(' ').Select(p => new String(p.Skip(1).SkipLast(1).ToArray())).ToArray();
        for (var i = 0; i < parts[0].Length; i++)
            lights |= parts[0][i] == '#' ? 1 << i : 0;
        var masks = new (int Mask, int Steps)?[1 << parts[0].Length];
        var initMasks = new List<int>();

        foreach (var pattern in parts.Skip(1).SkipLast(1))
        {
            var mask = 0;
            var bits = pattern.Split(",").Select(int.Parse).ToList();
            foreach (var bit in bits)
                mask |= 1 << bit;
            masks[mask] = (mask, 1);
            initMasks.Add(mask);
        }

        while (masks[lights] == null)
        {
            var notNullMasks = masks.Where(m => m != null);
            foreach (var maskA in notNullMasks)
            foreach (var maskB in initMasks)
            {
                var newMask = maskA.Value.Mask ^ maskB;
                var newLen = maskA.Value.Steps + 1;

                if (masks[newMask] == null)
                    masks[newMask] = (newMask, newLen);
            }
        }

        nonOptimalResult = masks[lights].Value.Steps;
        inputMasks = initMasks.ToArray();
    }

    public int GetResult()
        => nonOptimalResult < 3 ? nonOptimalResult : SearchShortestPath(lights, 0, inputMasks, 0, nonOptimalResult);

    private int SearchShortestPath(int pattern, int state, int[] availableMasks, int depth, int stopLimit)
    {
        if (state == pattern) return depth;
        if (depth == stopLimit) return int.MaxValue;
        if (!availableMasks.Any()) return int.MaxValue;

        int shortest = int.MaxValue;
        for (var i = 0; i < availableMasks.Length; i++)
        {
            var newState = state ^ availableMasks[i];
            if (pattern == newState) return depth + 1;
            var newLen = SearchShortestPath(pattern, newState,
                availableMasks.Where(m => m != availableMasks[i]).ToArray(), depth + 1, stopLimit);
            if (newLen < shortest)
            {
                if (newLen == depth + 1) return newLen;
                shortest = newLen;
            }
        }

        return shortest;
    }
}

file class Part2Sover
{
    private IList<int> buttons;
    private int joltage;
    private int joltageLen;
    public Part2Sover(string line)
    {
        var parts = line.Split(' ').Skip(1)
            .Select(p => new String(p.Skip(1).SkipLast(1).ToArray()))
            .Select(s => s.Split(',').Select(int.Parse).ToArray())
            .ToArray();
        var joltageArr = parts.Last();
        joltageLen =  joltageArr.Length;
        joltage = arrToInt(joltageArr);;
        buttons = parts.SkipLast(1).Select(arrToInt).ToList();
    }

    private int arrToInt(int[] input)
    {
        var result = 0;
        var mul = 1;
        for (var i = 0; i < input.Length; i++)
        {
            result += input[i] * mul;
            mul *= 10;
        }
        return result;
    }

    public int? Solve(int state, int clicks)
    {
        if(isTooMuch(state)) return null;
        if (state == joltage) return clicks;
        var min =  int.MaxValue;
        foreach (var buton in buttons)
        {
            var local = Solve(state + buton, clicks+1);
            if ((local ?? int.MaxValue) < min)
                min = local.Value;
        }
        return min != int.MaxValue ? min : null;
    }

    private bool isTooMuch(int state)
    {
        var initState = state;
        var target = joltage;
        while (target != 0)
        {
            if (target % 10 < state % 10)
            {
                //Console.WriteLine($"{initState} => {joltage}");
                return true;
            }

            target /= 10;
            state /= 10;
        }

        return state != 0;
    }
}