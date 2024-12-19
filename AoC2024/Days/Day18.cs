using AoC.Common.Maps.StaticMap;
using AoCBase2;

namespace AoC2024.Days;

public class Day18 : IDay
{
    public static void RunAoC() => AocRuntime.Day<Day18>(18)
        .Callback(1, (d, t) => d.Part1(t.GetLines()))
        //.Callback(2, (d, t) => d.Part2(t.GetLines()))
        .Test("example").Skip()
        .Test("input")
        //.Part(1).Correct("1,7,2,1,4,1,5,4,0")
        //.Part(2).Correct()
        .Run();

    private const int width = 71;
    private const int height = 71;
    private int bytes = 1024;
    private StaticMap<bool> map = new (width,height, true);
    private StaticMap<int> heatmap = new (width,height, int.MaxValue);
    private int currentMinimum = int.MaxValue;
    
    public string Part1(IEnumerable<string> input)
    {
        foreach (var b in input.Take(bytes).Select(i => i.Split(',').Select(int.Parse).ToArray()))
            map[b[0], b[1]] = false;
        FillHeatmap(0, 0, 0);
        
        //Console.WriteLine(map.Draw(b=> b ? ".": "#"));
        
        return heatmap[width-1, height-1].ToString();
    }

    private void FillHeatmap(int x, int y, int ctr)
    {
        if (ctr > currentMinimum) return; 
        if (heatmap[x, y] < ctr || !map[x, y]) return;
        heatmap[x, y] = ctr;
        if (x == width - 1 && y == height - 1)
        {
            currentMinimum = ctr;
            return;
        }
        if(x<width-1 && heatmap[x+1, y] > ctr+1)
            FillHeatmap(x+1, y, ctr + 1);
        if(x>0 && heatmap[x-1, y] > ctr+1)
            FillHeatmap(x-1, y, ctr + 1);
        if(y < height-1 && heatmap[x, y+1] > ctr+1)
            FillHeatmap(x, y+1, ctr + 1);
        if(y>0 && heatmap[x, y-1] > ctr+1)
            FillHeatmap(x, y-1, ctr + 1);
    }
}