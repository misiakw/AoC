using AoC.Common;
using AoC.Common.Maps.StaticMap;
using AoCBase2;

namespace AoC2024.Days;

public class Day16: IDay
{
    public static void RunAoC()=> AocRuntime.Day<Day16>(16, t => new Day16(t.GetMap()))
        .Callback(1, (d, t) => d.Part1())
        //.Callback(2, (d, t) => d.Part2())
        .Test("example1")
        .Test("example2").Skip()
        .Test("input").Skip()
        //.Part(1).Correct(88416)
        //.Part(2).Correct()
        .Run();

    private readonly IMap<char> _map;
    private readonly StaticMap<long> _heatmap;
    private readonly Point _start;
    private readonly Point _finish;
    public Day16(IMap<char> map)
    {
        _map = map;
        for (var y = 0; y < map.Height; y++)
        for (var x = 0; x < map.Width; x++)
        {
            if(_map[x,y] == 'S')
                _start = new Point(x,y);
            if(_map[x,y] == 'E')
                _finish = new Point(x,y);
        }

        _heatmap = new StaticMap<long>(_map.Width, map.Height, long.MaxValue);
    }

    public string Part1()
    {
        GetPath((_start.X, _start.Y), 'E', 0);
        
        return _heatmap[_finish.X, _finish.Y].ToString();
    }

    private void GetPath((long x, long y) pos, char direction, long currScore)
    {
        if (_heatmap[pos.x, pos.y] < currScore) return;
            _heatmap[pos.x, pos.y] = currScore;
            
        if (pos.x == _finish.X && pos.y == _finish.Y)
            return;

        if (CanGoTo(pos.x, pos.y - 1))
            GetPath((pos.x, pos.y - 1), 'S',
                currScore + Rotate(direction, 'S') + 1);

        if (CanGoTo(pos.x, pos.y + 1))
            GetPath((pos.x, pos.y + 1), 'N',
                currScore + Rotate(direction, 'N') + 1);

        if (CanGoTo(pos.x - 1, pos.y))
            GetPath((pos.x - 1, pos.y), 'W',
                currScore + Rotate(direction, 'W') + 1);

        if (CanGoTo(pos.x + 1, pos.y))
            GetPath((pos.x + 1, pos.y), 'E',
                currScore + Rotate(direction, 'E') + 1);
    }

    private bool CanGoTo(long x, long y)
    {
        if (x < 0 || y < 0 || x > _map.Width - 1 || y > _map.Height - 1)
            return false;
        return _map[x,y] != '#';
    }
    
    private int Rotate(char now, char next)
    {
        if (now == next) return 0;
        return now switch
        {
            'N' => next is 'E' or 'W' ? 1000 : 2000,
            'E' => next is 'N' or 'S' ? 1000 : 2000,
            'S' => next is 'E' or 'W' ? 1000 : 2000,
            'W' => next is 'N' or 'S' ? 1000 : 2000,
        };
    }
}