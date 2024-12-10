using AoC.Common.Maps.StaticMap;
using AoCBase2;

namespace AoC2024.Days;

public class Day10: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day10>(10, t => new Day10(t.GetLines().ToArray()))
        .Callback(1, (d, t) => d.Part1())
        .Callback(2, (d, t) => d.Part2())
        .Test("example").Skip(2)
        .Test("largeexample")
        .Test("input")
        //.Part(1).Correct(760)
        //.Part(2).Correct(1764)
        .Run();

    private readonly StaticMap<int> map;
    private IList<(int x, int y)> startingPoints = new List<(int x, int y)>();
    public Day10(string[] lines)
    {
        map = new StaticMap<int>(lines[0].Length, lines.Length);
        for (var y=0; y<map.Height; y++)
            for (var x = 0; x < map.Width; x++)
            {
                map[x, y] = lines[y][x]-'0';
                if(lines[y][x] == '0')
                    startingPoints.Add((x, y));
            }
    }

    public string Part1()
    {
        var sum = 0;
        foreach (var point in startingPoints)
        {
            var paths = Ascend(point.x, point.y, "");
            sum += paths.DistinctBy(p => $"{p.x},{p.y}").Count();
        }

        return sum.ToString();
    }
    public string Part2()
    {
        var sum = 0;
        foreach (var point in startingPoints)
        {
            var paths = Ascend(point.x, point.y, "");
            sum += paths.Count();
        }

        return sum.ToString();
    }

    private IEnumerable<(int x, int y, string path)> Ascend(int sx, int sy, string path)
    {
        path = $"{path}->[{sx},{sy}]";
        if(map[sx, sy] == 9)
            yield return (sx, sy, path);
        else
        {
            var height = map[sx, sy];
            foreach (var point in new (int x, int y)[] { (sx - 1, sy), (sx + 1, sy), (sx, sy - 1), (sx, sy + 1) })
            {
                if (point.x < 0 || point.x >= map.Width || point.y < 0 || point.y >= map.Height) continue;
                if (map[point.x, point.y] - height != 1) continue;
                foreach (var top in Ascend(point.x, point.y, path))
                    yield return top;
            }
        }
    }
}