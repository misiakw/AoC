using System.Text;
using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;

namespace AoC2025.Days;

[DayNum(10)]
file class Day10: IDay
{
    public void RunAoC()
    {
        AocRuntime.Day<Day10>(10)
            .Callback(1, (d, t) => d.Part1(t.GetLines()))
            //.Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example", "Inputs/Day10/example.txt") //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day10/input.txt") //.Part(1)//.Part(2)
            .Run();
    }

    public string Part1(IEnumerable<string> lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            (var lights, var masks, var initialSteps) = ReadInput(line);
            if (initialSteps < 3)
                sum += initialSteps;
            else
                sum += SearchShortestPath(lights, 0, masks, 0, initialSteps);
        }
        return sum.ToString();
    }

    private (int target, int[] masks, int initialLen) ReadInput(string line)
    {
        var parts = line.Split(' ').Select(p => new String(p.Skip(1).SkipLast(1).ToArray())).ToArray();
        var lights = 0;
        for(var i=0; i < parts[0].Length; i++)
            lights |= parts[0][i] == '#' ? 1<<i : 0;
        var masks = new MaskSet[1 << parts[0].Length];
        var initMasks = new List<int>();
            
        foreach (var pattern in parts.Skip(1).SkipLast(1))
        {
            var mask = 0;
            var bits = pattern.Split(",").Select(int.Parse).ToList();
            foreach (var bit in bits)
                mask |= 1 << bit;
            masks[mask] = new MaskSet{Mask = mask, Steps = 1};
            initMasks.Add(mask);
        }

        while (masks[lights] == null)
        {
            var notNullMasks = masks.Where(m => m != null);
            foreach (var maskA in notNullMasks)
            foreach (var maskB in initMasks)
            {
                var newMask = maskA.Mask ^ maskB;
                var newLen = maskA.Steps + 1;

                if (masks[newMask] == null)
                    masks[newMask] = new MaskSet{Mask = newMask, Steps = newLen};
            }

            var a = 5;
        }
        
        return (lights, initMasks.ToArray(), masks[lights].Steps);
    }
    
    private int SearchShortestPath(int pattern, int state, int[] availableMasks, int depth, int stopLimit)
    {
        if (state == pattern) return depth;
        if (depth == stopLimit) return int.MaxValue;
        if (!availableMasks.Any()) return int.MaxValue;

        int shortest =  int.MaxValue;
        for (var i = 0; i < availableMasks.Length; i++)
        {
            var newState = state ^ availableMasks[i];
            if (pattern == newState) return depth + 1;
            var newLen = SearchShortestPath(pattern, newState, availableMasks.Where(m => m != availableMasks[i]).ToArray(), depth + 1, stopLimit);
            if (newLen < shortest)
            {
                if (newLen == depth + 1) return newLen;
                shortest = newLen;
            }
        }
        return shortest;
    }
}

file class MaskSet
{
    public required int Mask;
    public required int Steps;
}