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
        foreach (var line in lines)
        {
            (var lights, var masks) = ReadInput(line);
            if(masks[lights].Steps < 3)
                Console.WriteLine($"initially to get {lights} you need no more than {masks[lights].Steps}");
            else
                Console.WriteLine($"initially to get {lights} you need no more than {masks[lights].Steps} AND THAT MAY BE IMPROVED...");
        }
        Console.WriteLine("---");
        return "";
    }

    private (int target, IList<MaskSet> masks) ReadInput(string line)
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
        
        return (lights, masks);
    }
}

file class MaskSet
{
    public required int Mask;
    public required int Steps;
}