using AoCBase2;

namespace AoC2024.Days;

public class Day9: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day9>(9, t => new Day9(t.GetLines().First()))
        .Callback(1, (d, t) => d.Part1())
        //.Callback(2, (d, t) => d.Part2())
        .Test("example")
        .Test("input")
        //.Part(1).Correct(6432869891895)
        //.Part(2).Correct()
        .Run();

    private long[] disk;
    public Day9(string diskspace)
    {
        disk = diskspace.ToCharArray().Select(c => (long)(c - '0')).ToArray();
    }

    public string Part1()
    {
        var memory = GetMemoryStruct(disk).ToArray();
        memory = moveBlocks(memory);

        var sum = 0l;
        for(var i=0; memory[i].HasValue; i++)
            sum += i*(memory[i] ?? 0);
        
        //Console.WriteLine(string.Join("", memory.Select(s => s.HasValue ? (char)(s + '0') : '.')));
        return sum.ToString();
    }

    private IEnumerable<long?> GetMemoryStruct(long[] fileStruct)
    {
        for (short i = 0; i < fileStruct.Length; i++)
            for(var l = fileStruct[i]; l>0; l--)
                if (i % 2 == 0)
                    yield return (long?)(i / 2);
                else
                    yield return null;
    }

    private long?[] moveBlocks(long?[] memory)
    {
        var emptyCursor = disk[0];
        var endCursor = memory.Length;
        while (!memory[--endCursor].HasValue) ;

        while (endCursor > emptyCursor)
        {
            memory[emptyCursor] = memory[endCursor];
            memory[endCursor] = null;
            while (!memory[--endCursor].HasValue) ;
            while(memory[++emptyCursor].HasValue) ;
        }

        return memory;
    }
}