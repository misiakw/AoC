using System.Collections;
using System.Text;
using AoCBase2;
using ImageMagick;

namespace AoC2024.Days;

public class Day4: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day4>(4)
        .Callback(1, (d, t) => d.Part1(t.GetLinesAsync().ToEnumerable().ToArray()))
        //.Callback(2, (d, t) => d.Part2(t.GetLinesAsync()))
        .Test("clean")
            //.Part(2).Skip()
        .Test("example").Skip()
            //.Part(1).Skip()
        .Test("input").Skip()
//               .Part(1).Correct(174336360)
//               .Part(2).Correct(88802350)
        .Run();

    public string Part1(string[] input)
    {
        foreach (var line in input)
            Console.WriteLine(line+$" {SearchinString(line)}");
        Console.WriteLine(" -- 45 -- ");
        foreach (var line in Flip45(input))
            Console.WriteLine(line+$" {SearchinString(line)}");
        Console.WriteLine(" -- 135 -- ");
        foreach (var line in Flip135(input))
            Console.WriteLine(line+$" {SearchinString(line)}");
        
        
        
        
        return null;
    }

    private long SearchinString(string line)
    {
        string word;
        int pos = 0;
        int count = 0;

        while (pos < line.Length)
        {
            word = line[pos] == 'X' ? "XMAS" : line[pos] == 'S' ? "SAMX" : null;
            pos++;
            if (string.IsNullOrEmpty(word))
                continue;
            for (int i = 0; i < 3 && pos+i < line.Length; i++)
            {
                if (word[i + 1] != line[pos + i])
                    break;
                if(i==2)
                    count++;
            }
        }
        return count;
    }

    private IEnumerable<string> Flip90(string[] input)
    { 
        var sb = new StringBuilder();
        for (var x = 0; x < input[0].Length; x++)
        {
            sb.Clear();
            for (var y = 0; y < input.Length; y++)
                sb.Append(input[y][x]);
            yield return sb.ToString();
        }
    }
    private IEnumerable<string> Flip45(string[] input)
    {
        var sb = new StringBuilder();
        for (var x = 0; x < input[0].Length; x++)
        {
            var y = input.Length-1;
            sb.Clear();
            for (var step = 0; x - step >= 0 && y - step >= 0; step++)
                sb.Append(input[y - step][x - step]);
            yield return sb.ToString();
        }
         //tylko połowa wyszukania
    }

    private IEnumerable<string> Flip135(string[] input)
    {
        var sb = new StringBuilder();
        for (var x = 0; x < input[0].Length; x++)
        {
            var y = x;
            sb.Clear();
            for (var step = 0; step <= x; step++)
                sb.Append(input[y - step][step]);
            yield return sb.ToString();
        }
        //ToTylkoPołowa wyszukania
    }
}