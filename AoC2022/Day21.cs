using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day21 : DayBase
    {
        public Day21() : base(21)
        {
            Input("example1")
                .RunPart(1, 152L)
                .RunPart(2, 301L)
            .Input("output")
                .RunPart(1, 152479825094094L);
        }

        public override object Part1(Input input)
        {
            var herd = new Herd();

            foreach (var line in input.Lines)
                herd.Introduce(line);

            return herd.Solve("root");
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        protected class Herd
        {
            protected IDictionary<string, Monkey> _herd = new Dictionary<string, Monkey>();

            public void Introduce(string input)
            {
                var parts = input.Split(":", StringSplitOptions.TrimEntries);
                var operation = parts[1].Split(" ", StringSplitOptions.TrimEntries);

                if (operation.Length == 1)
                    _herd.Add(parts[0], new Monkey() { Value = long.Parse(operation[0]) });
                else
                {
                    Func<long, long, long>? action = null;
                    switch (operation[1][0])
                    {
                        case '+':
                            action = (l, r) => l + r;
                            break;
                        case '-':
                            action = (l, r) => l - r;
                            break;
                        case '*':
                            action = (l, r) => l * r;
                            break;
                        case '/':
                            action = (l, r) => l / r;
                            break;
                    }
                    _herd.Add(parts[0], new Monkey()
                    {
                        Left = operation[0],
                        Right = operation[2],
                        Action = action
                    });
                }
            }

            public long Solve(string name)
            {
                var monkey = _herd[name];
                if (!monkey.Value.HasValue)
                {
                    var l = Solve(monkey.Left);
                    var r = Solve(monkey.Right);
                    var result = monkey.Action.Invoke(l, r);
                    monkey.Value = result;
                }
                return monkey.Value.Value;
            }


        }

        protected class Monkey
        {
            public long? Value;
            public string Left = string.Empty;
            public string Right = string.Empty;
            public Func<long, long, long>? Action;
        }
    }
}
