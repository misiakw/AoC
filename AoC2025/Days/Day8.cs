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
            .Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example", "Inputs/Day8/example.txt") //.Part(1)//.Part(2)
            .Test("input", "Inputs/Day8/input.txt") //.Part(1)//.Part(2)
            .Run();
    }

    public string Part1(IEnumerable<string> input, int circuts)
    {
        var data = ReadInput(input);

        for (int i = 0; i < circuts; i++)
        {
            var toMerge =data.order.Dequeue();
            toMerge.b1.Group.Merge(toMerge.b2.Group, data.groups);
        }

        var g = data.groups.OrderByDescending(g => g.Size).Take(4).Select(g => g.Size).ToArray();

        return (g[0] * g[1] * g[2]).ToString();
    }
    public string Part2(IEnumerable<string> input)
    {
        var data = ReadInput(input);

        (Box b1, Box b2) last = data.order.Peek();
        for (var i=0 ; data.groups.Count != 1 ; i++)
        {
            var toMerge = data.order.Dequeue();
            if (toMerge.b1.Group != toMerge.b2.Group)
            {
                last = (toMerge.b1, toMerge.b2);
                toMerge.b1.Group.Merge(toMerge.b2.Group, data.groups);
            }
        }
        return (last.b1.X * last.b2.X).ToString();
    }

    private (Queue<(Box b1, Box b2)> order, IList<Group> groups) ReadInput(IEnumerable<string> input)
    {
        IList<(ulong dist, Box b1, Box b2)> distances = new List<(ulong dist, Box b1, Box b2)>();
        IList<Box> boxes = new List<Box>();
        int groupId = 1;
        foreach (var line in input)
        {
            var box = new Box(line, groupId++);
            foreach (var otherBox in boxes)
                distances.Add((box.AddNodeToVertices(otherBox), box, otherBox));
            boxes.Add(box);
        }

        var order =  new Queue<(Box b1, Box b2)>(distances.OrderBy(b => b.dist).Select(b => (b.b1, b.b2)).ToList());
        var groups = boxes.Select(b => b.Group).OrderBy(g => g.GroupId).ToList();
        return (order, groups);
    }
}

file class Box
{
    public readonly ulong X;
    public readonly ulong Y;
    public readonly ulong Z;
    public Group Group;
    private IList<(ulong, Box)> _verticeWeight = new List<(ulong, Box)> ();

    public Box(string line, int groupId)
    {
        var pos = line.Split(',').Select(ulong.Parse).ToArray();
        X = pos[0];
        Y = pos[1];
        Z = pos[2];
        Group = new Group(this, groupId);
    }
    private ulong GetDistance(Box box)
    {
        var dx = box.X < X ? X - box.X : box.X - X;
        var dy = box.Y < Y ? Y - box.Y : box.Y - Y;
        var dz = box.Z < Z ? Z - box.Z : box.Z - Z;
        return dx*dx + dy*dy + dz*dz;
    }

    public override string ToString()
        => $"[{X},{Y},{Z}][{Group.GroupId}]";

    public ulong AddNodeToVertices(Box box)
    {
        var distance = GetDistance(box);
        _verticeWeight.Add((distance, box));
        box._verticeWeight.Add((distance, this));
        return distance;
    }
}

file class Group
{
    public int GroupId { get; init; }
    public readonly IList<Box> Members = new List<Box>();
    public int Size = 1;
    public Group(Box founder, int groupId)
    {
        GroupId = groupId;
        Members.Add(founder);
    }

    public void Merge(Group other, IList<Group> groups)
    {
        if(this == other) return;
        if (other.Size > Size)
        {
            other.Merge(this, groups);
            return;
        }

        //Console.WriteLine($"Merge {other.GroupId} => {GroupId}");

        groups.Remove(other);
        foreach (var member in other.Members)
        {
            member.Group = this;
            Members.Add(member);
            Size++;
        }
    }

    public override string ToString()
        => $"G[{GroupId}]({Size})";
}