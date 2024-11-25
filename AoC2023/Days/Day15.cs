using System.Collections.Generic;
using System.Linq;
using AoC.LegacyBase;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day15 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day15/example1.txt")
                    .Part(1).Correct(1320)
                    .Part(2).Correct(145)
                .Test("output", "./Inputs/Day15/output.txt")
                    .Part(1).Correct(516804)
                    .Part(2).Correct(231844);
        }
        public override string Part1(TestState input)
        {
            var steps = ReadSteps(input);
            return steps.Sum(s => GetHash(s.Cmd)).ToString();
        }

        public override string Part2(TestState input)
        {
            var steps = ReadSteps(input);
            var boxes = new Dictionary<int, IList<CmdStruct>>();
            for (var i = 0; i < 256; i++)
                boxes.Add(i, new List<CmdStruct>());

            foreach(var step in steps)
            {
                var box = boxes[GetHash(step.Name)];
                if (step.Action == '-')
                {
                    if (box.Any(s => s.Name == step.Name))
                        box.Remove(box.First(s => s.Name == step.Name));
                }
                else
                {
                    if(!box.Any(s => s.Name == step.Name))
                    {
                        box.Add(new CmdStruct(step.Cmd));
                    }
                    else
                    {
                        var len = box.First(s => s.Name == step.Name);
                        len.Vaue = step.Vaue;
                    }
                }
            }

            var score = 0;
            for (var i = 0; i < 256; i++)
                for (var j = 0; j < boxes[i].Count; j++)
                    score += (i + 1) * (j + 1) * boxes[i][j].Vaue;

            return score.ToString();
        }

        private int GetHash(string input)
        {
            var result = 0;
            foreach(var ch in input)
            {
                result += ch;
                result *= 17;
                result %= 256;
            }
            return result;
        }

        private IList<CmdStruct> ReadSteps(TestState input)
            => input.GetLines().ToArray()[0].Split(',').Select(s =>new CmdStruct(s.Trim())).ToList();

        private class CmdStruct
        {
            public string Name;
            public char Action;
            public int Vaue;
            public string Cmd;
            public CmdStruct(string input)
            {
                Action = ' ';
                Name = string.Empty;
                var tmp = string.Empty;
                foreach (var ch in input)
                {
                    if (ch != '-' && ch != '=')
                        tmp = $"{tmp}{ch}";
                    else
                    {
                        Action = ch;
                        Name = tmp;
                        tmp = string.Empty;
                    }
                }
                Vaue = !string.IsNullOrEmpty(tmp) ? int.Parse(tmp) : -1;
                Cmd = input;
            }
        }
    }
}
