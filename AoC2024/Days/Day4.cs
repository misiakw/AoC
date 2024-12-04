using System.Collections;
using System.Text;
using AoCBase2;
using ImageMagick;

namespace AoC2024.Days;

public class Day4: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day4>(4)
        .Callback(1, (d, t) => d.Part1(t.GetLines().ToArray()))
        .Callback(2, (d, t) => d.Part2(t.GetLines().ToArray()))
        .Test("clean")
            .Part(2).Skip()
        .Test("example")
        .Test("input")
            //.Part(1).Correct(2358)
            //.Part(2).Correct(1737)
        .Run();

    public string Part1(string[] input)
    {
        var sum = 0l;
        foreach (var line in input.Concat(Flip45(input)).Concat(Flip90(input)).Concat(Flip135(input)))
            sum += SearchinString(line, "XMAS");
        return sum.ToString();
    }

    public string Part2(string[] input)
    {
        var width = input[0].Length;
        var height = input.Length;
        var count = 0;

        for (var x = 0; x <= width - 3; x++)
            for (var y = 0; y <= height - 3; y++)
                if(GotPattern(input.Skip(y).Take(3).Select(s => s.Substring(x, 3)).ToArray()))
                    count++;
        return count.ToString();
    }

    private long SearchinString(string input, string pattern)
    {
        string word;
        int pos = 0;
        int count = 0;

        while (pos < input.Length)
        {
            word = input[pos] == 'X' ? "XMAS" : input[pos] == 'S' ? "SAMX" : null;
            //word = input[pos] == pattern[0] ? pattern : input[pos] == pattern[pattern.Length - 1] ? string.Concat(pattern.Reverse()) : null;
            pos++;
            if (string.IsNullOrEmpty(word))
                continue;
            for (int i = 0; i < 3 && pos+i < input.Length; i++)
            {
                if (word[i + 1] != input[pos + i])
                    break;
                if(i==2)
                    count++;
            }
        }
        return count;
    }

    private bool GotPattern(string[] window)
    {
        if (window[1][1] != 'A') return false;
        var allowed = new string[]{ "MMSS", "SMMS", "SSMM", "MSSM" };
        return allowed.Contains($"{window[0][0]}{window[0][2]}{window[2][2]}{window[2][0]}");
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
        var width = input[0].Length;
        var height = input.Length;
        var sb = new StringBuilder();

        for(var y=height-1; y>=0; y--)
        {
            sb.Clear();
            for(var step=0; y+step < height && step < width; step++)
                sb.Append(input[y+step][step]);
            yield return sb.ToString();
        }
        for(var x=1; x<width; x++)
        {
            sb.Clear();
            for (var step = 0; step < height && x+step < width; step++)
                sb.Append(input[step][x+step]);
            yield return sb.ToString();
        }
    }

    private IEnumerable<string> Flip135(string[] input)
    {
        var width = input[0].Length;
        var height = input.Length;
        var sb = new StringBuilder();

        for(var x=0; x<width; x++)
        {
            sb.Clear();
            for(var step=0;step < height && x-step >= 0; step++)
                sb.Append(input[step][x-step]);
            yield return sb.ToString() ;
        }
        for(var y=1; y < height; y++)
        {
            sb.Clear();
            for(var step=0; y+step<height && step<width ;step++)
                sb.Append(input[y+step][width-1-step]);
            yield return sb.ToString();
        }
    }
}