using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;

namespace AoC2025.Days;

[DayNum(8)]
file class Day8: IDay
{
    public void RunAoC()
    {
        AocRuntime.Day<Day8>(8)
            .Callback(1, (d, t) => d.Part1(t.GetLines(), t.name == "example" ? 10: 1000))
            //.Callback(2, (d, t) => d.Part2(t.GetMap()))
            .Test("example", "Inputs/Day8/example.txt") //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day8/input.txt").Skip() //.Part(1)//.Part(2)
            .Run();
    }

    public string Part1(IEnumerable<string> input, int circuts)
    {
        IList<(double dist, Box b1, Box b2)> distances = new List<(double dist, Box b1, Box b2)>();
        IList<Box> boxes = new List<Box>();
        foreach (var line in input)
        {
            var pos = line.Split(',').Select(int.Parse).ToArray();
            var box = new Box(pos[0], pos[1], pos[2]);
            foreach (var otherBox in boxes)
                distances.Add((box.GetDistance(otherBox), box, otherBox));
            boxes.Add(box);
        }

        distances = distances.OrderBy(b => b.dist).ToArray();

        return "";
    }
}

file class Box(int x, int y, int z)
{
    public int X => x;
    public int Y => y;
    public int Z => z;
    public int? Group;
    public double GetDistance(Box box)
    {
        int dx = Math.Abs(x - box.Y);
        int dy = Math.Abs(y - box.Y);
        int dz = Math.Abs(z - box.Z);
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public override string ToString()
        => $"[{x},{y},{z}][{Group}]";
}