using AoCBase2;

namespace AoC2024.Days;

public class Day9: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day9>(9, t => new Day9(t.GetLines().First()))
        .Callback(1, (d, t) => d.Part1())
        .Callback(2, (d, t) => d.Part2())
        .Test("example")
        .Test("input")
        //.Part(1).Correct(6432869891895)
        //.Part(2).Correct(6467290479134)
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
        for (var i = 0; memory[i].HasValue; i++)
            sum += i * (memory[i] ?? 0);

        return sum.ToString();
    }

    public string Part2()
    {
        var memory = moveFiles(GetMemoryStruct(disk).ToArray());

        var sum = 0l;
        for (var i = 0; i<memory.Length; i++)
            sum += i * (memory[i] ?? 0);

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

    private long?[] moveFiles(long?[] memory)
    {
        int lastFilePos = memory.Length - 1;
        int lastFileLen = 1;
        long lastFileNum = memory.Last().Value;
        while (lastFilePos > 0)
        {
            while (memory[lastFilePos - 1] == lastFileNum)
            {
                lastFilePos--;
                lastFileLen++;
                if(lastFilePos == 0)
                    return memory;
            }
            for (var gapStart = 0; gapStart < lastFilePos; gapStart++)
            {
                if (memory[gapStart].HasValue) continue;
                var gapSize = 0;
                for (; !memory[gapStart + gapSize].HasValue; gapSize++) ;

                if (gapSize >= lastFileLen)
                {
                    //move file to space
                    for (var i = 0; i < lastFileLen; i++)
                    {
                        memory[gapStart + i] = memory[lastFilePos + i];
                        memory[lastFilePos + i] = null;
                    }
                    break;
                }
                gapStart += gapSize;
            }
            lastFilePos--;
            //skip empty
            var empty = 0;
            while (!memory[lastFilePos - empty].HasValue) empty++;
            lastFilePos -= empty;
            lastFileNum = memory[lastFilePos].Value;
            lastFileLen = 1;
        }
        return memory;
    }
}