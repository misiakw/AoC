using System.Linq;
using AoC.Common;
using AoC.Common.Maps.StaticMap;
using AoCBase2;

namespace AoC2024.Days;

public class Day8: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day8>(8, t => new Day8(t.GetLines().ToArray()))
        .Callback(1, (d, t) => d.Part1()).Skip()
        .Callback(2, (d, t) => d.Part2())
        .Test("debug", true).Skip()
        .Test("example").Skip()
        .Test("input")//.Skip()
        //.Part(1).Correct(249)
        //.Part(2).Correct(169122112716571)
        .Run();

    private class Cell
    {
        public char Freq;
        public bool isAntinode = false;
        public (int x, int y) pos;
        public override string ToString() => $"[{Freq} {isAntinode} ({pos.x}, {pos.y})]";
    }

    private StaticMap<Cell> map;
    private IDictionary<char, IList<Cell>> frkSet = new Dictionary<char, IList<Cell>>();
    public Day8(string[] input)
    {
        map = new StaticMap<Cell>(input[0].Length, input.Length);
        for(var y=0; y<map.Height; y++)
            for (var x = 0; x < map.Width; x++)
                if (input[y][x] != '.')
                {
                    var cell = new Cell()
                    {
                        Freq = input[y][x],
                        pos = (x, y)
                    };
                    if (!frkSet.ContainsKey(cell.Freq))
                        frkSet.Add(cell.Freq, new List<Cell>());
                    frkSet[cell.Freq].Add(cell);
                    map[x, y] = cell;
                }
                else
                    map[x, y] = new Cell() { Freq = '.' };

        var keysToDelete = frkSet.Keys.Where(k => frkSet[k].Count <= 1);
        foreach (var key in keysToDelete)
            frkSet.Remove(key);
    }

    public string Part1()
    {
        foreach (var frequency in frkSet.Keys)
        {
            var positions = frkSet[frequency].ToArray();
            for (var i = 1; i < positions.Length; i++)
            {
                var antinodes = GetAntinodes(positions[i - 1], positions.Skip(i).ToArray());
                foreach (var ant in antinodes)
                    if(ant.x >= 0 && ant.x < map.Width && ant.y >=0 && ant.y < map.Height)
                        map[ant.x, ant.y].isAntinode = true;
            }
        }
        return map.Count(c => c.isAntinode).ToString();
    }

    public string Part2()
    {
        foreach (var frequency in frkSet.Keys)
        {
            var positions = frkSet[frequency].ToArray();
            for (var i = 1; i < positions.Length; i++)
            {
                var antinodes = GetAntinodes2(positions[i - 1], positions.Skip(i).ToArray());

                try
                {
                    foreach (var ant in antinodes)
                        map[ant.x, ant.y].isAntinode = true;
                }
                catch (Exception e)
                {
                    var a = 5;
                }
            }
        }
        Console.WriteLine(map.Draw(c => c.isAntinode ? "#" :$"{c.Freq}"));
        return map.Count(c => c.isAntinode).ToString();
    }
    
    
    private IEnumerable<(int x, int y)> GetAntinodes(Cell start, IList<Cell> ends)
    {
        foreach (var cell in ends)
        {
            (int dx, int dy) dif = (start.pos.x - cell.pos.x, start.pos.y - cell.pos.y);
            if(start.pos.x != cell.pos.x-dif.dx && start.pos.y != cell.pos.y-dif.dy)
                yield return (cell.pos.x-dif.dx, cell.pos.y-dif.dy);
            if(start.pos.x != cell.pos.x+dif.dx && start.pos.y != cell.pos.y+dif.dy)
            yield return (cell.pos.x+dif.dx, cell.pos.y+dif.dy);
            
            if(cell.pos.x != start.pos.x-dif.dx && cell.pos.y != start.pos.y-dif.dy)
            yield return (start.pos.x-dif.dx, start.pos.y-dif.dy);
            if(cell.pos.x != start.pos.x+dif.dx && cell.pos.y != start.pos.y+dif.dy)
            yield return (start.pos.x+dif.dx, start.pos.y+dif.dy);
        }
    }
    
    private IEnumerable<(int x, int y)> GetAntinodes2(Cell start, IList<Cell> ends)
    {
        foreach (var cell in ends)
        {
            var step = GetStep(start, cell);
            
            Console.WriteLine(step);
            
            if(step.x < 0) //revert negative step
                step = (step.x * -1, step.y*-1);
            
            
            var cy = start.pos.y;
            for (var x = start.pos.x; (x < map.Width && x >= 0); x += step.x)
            {
                if (cy >= 0 && cy <= map.Height)
                    yield return (x, cy);
                cy += step.y;
            }

            cy = start.pos.y - step.y;
            for (var x = start.pos.x- step.x; (x < map.Width && x >= 0); x -= step.x)
            {
                if (cy >= 0 && cy < map.Height)
                    yield return (x, cy);
                cy -= step.y;
            }
        }
    }
    
    private (int x, int y) GetStep(Cell a, Cell b){ 
        double ix, iy;
        if (a.pos.x < b.pos.x)
        {
            ix = b.pos.x - a.pos.x;
            iy = b.pos.y - a.pos.y;
        }
        else
        {
            ix = a.pos.x - b.pos.x;
            iy = a.pos.y - b.pos.y;
        }

        //todo: tu wpada niepotrzebne zaokraglenie :!
        //NWD jako posrednia
        for (var x = 1; x <= ix; x++)
        {
            var y = iy / (ix / (double)x);
            if (y == (int)y)
                return (x, (int)y);
        }
        return ((int)ix, (int)iy);
        
        
        /*double sy = (double)iy / ix;
        iy = sy;
        ix = 1;
        while (iy != (int)iy)
        {
            ix += 1;
            iy += sy; //todo: tu wpada niepotrzebne zaomroglenie :!
        }
        return ((int)ix, (int)iy);*/
    }
}