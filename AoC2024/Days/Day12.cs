using AoC.Common;
using AoC.Common.Maps.StaticMap;
using AoCBase2;

namespace AoC2024.Days;

public class Day12: IDay
{
    public static void RunAoC() => AocRuntime.Day<Day12>(12, t => new Day12(t.GetMap()))
        .Callback(1, (d, t) => d.Part1())
        //.Callback(2, (d, t) => d.Part2())
        .Test("smallexample")
        .Test("complexexample")
        .Test("bigexample")
        .Test("input").Skip(2)
        //.Part(1).Correct(222461)
        //.Part(2).Correct()
        .Run();

    private class Plot
    {
        public char crop;
        public int group;
        public int fenceSize;
        public int x, y;
        public override string ToString() => $"[{crop}:{group} {fenceSize}]";
    }

    private StaticMap<Plot> map;
    public Day12(IMap<char> input)
    {
        map = new StaticMap<Plot>(input.Width, input.Height);
        for(var y = 0; y < input.Height; y++)
            for (var x = 0; x < input.Width; x++)
                map[x, y] = new Plot(){ crop = input[x, y], x = x, y = y };

        map[0,0].fenceSize = 4 - getNeighbours(0,0).Where(p => p.crop == map[0,0].crop).Count();
        floodRegionWithGroupId(0, 0, 1, map[0,0].crop);
        var nextGroupId = 2;
        
        for(var y = 0; y < input.Height; y++)
            for (var x = 0; x < input.Width; x++)
            {
                if (x == 0 && y == 0) continue;
                var neighbours = getNeighbours(x, y).Where(p=> p!= null && p.crop == map[x,y].crop);
                map[x, y].fenceSize = 4-neighbours.Count();
                if(map[x, y].group == 0) floodRegionWithGroupId(x, y, nextGroupId++, map[x, y].crop);
            }
    }

    public string Part1()
    {
        return map.GroupBy(p => p.group)
            .Sum(g => g.Count() * g.Sum(p => p.fenceSize))
            .ToString();
    }
    
    //part2
    //wez dowolny klocek z każdej grupy, zasuwaj do góry aż trafisz na siane (0 fence size to brak sciany
    // leć wzdloz sciany, licznik scian = 0. zrob wskaznik przesuwania sie i lec az do najblizszego rogu
    // fence size == 1 - niby porosta, ale uwazaj, bo moze być rog na zewnatrz
    // biez pod uwage z ktorej strony jest sciana, licznik moze byc bledy!!!
    // licznik 2 - albo rog, albo waskie przejscie - sprawdz to za pomoca sasiadow
    // licznik 3 - na 100% "zawrotka, mozna zmienic kierunek, licznik scian+1
    // licznik 4 - samotne pole, licznik = 4, obsluguj kolejna grupe.
    
    //alternatywnie - a co gdyby tak przypasowywac macierz 2x2 lub 3x3???

    private void floodRegionWithGroupId(int x, int y, int id, char crop)
    {
        if (map[x, y].group != 0) return;
        map[x, y].group = id;
        var nextGroupId = id + 1;
        var neighbours = getNeighbours(x, y).Where(p => p.group == 0);
        foreach(var neighbour in neighbours.Where(p => p.crop == crop))
            floodRegionWithGroupId(neighbour.x, neighbour.y, id, neighbour.crop);
    }
    
    private IEnumerable<Plot> getNeighbours(long x, long y)
    {
        if (y > 0) yield return map[x, y - 1];
        if(x < map.Width-1) yield return map[x+1, y];
        if(y < map.Height - 1) yield return map[x, y + 1];
        if (x > 0) yield return map[x - 1, y];
    }
}