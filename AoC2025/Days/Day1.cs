using AoC.Base;
using AoC.Base.Abstraction;
using AoC.Base.Runtime;

namespace AoC2025.Days;

[DayNum(1)]
file class Day1 : IDay
{
    public void RunAoC()
    {
        AocRuntime.Day<Day1>(1)
            .Callback(1, (d, t) => d.Part1(t.GetLines()))
            .Callback(2, (d, t) => d.Part2(t.GetLines()))
            .Test("example", "Inputs/Day1/example1.txt")
            .Test("input", "Inputs/Day1/input.txt")
            .Run();
    }

    private string Part1(IEnumerable<string> lines)
    {
        var dial = new Dial(50);
        var ctr = 0;
        foreach (var line in lines)
            if (dial.Rotate(line) == 0)
                ctr++;
        return ctr.ToString();
    }

    public string Part2(IEnumerable<string> lines)
    {
        var dial = new Dial(50);
        foreach (var line in lines)
            dial.Rotate(line);
        return dial.Zeroes.ToString();
    }
}

file class Dial
{
    public int State { get; private set; }
    public int Zeroes = 0;

    public Dial(int State)
    {
        this.State = State;
    }

    public int Rotate(string descr)
    {
        var dir = descr[0];
        var dist = int.Parse(descr.Substring(1));

        if (dir == 'L')
            for (var i = 0; i < dist; i++)
                RotL();
        else
            for (var i = 0; i < dist; i++)
                RotR();
        return State;
    }

    private void RotL()
    {
        State--;
        if (State == 0) Zeroes++;
        if (State == -1) State = 99;
    }

    private void RotR()
    {
        State = (State + 1) % 100;
        if (State == 0) Zeroes++;
    }
}