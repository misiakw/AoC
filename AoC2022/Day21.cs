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
            //Input("test")
            //.RunPart(2)
            Input("example1")
                .RunPart(1, 152L)
                .RunPart(2, 301L)
            .Input("output")
                .RunPart(1, 152479825094094L)
                .RunPart(2, 3360561285172L); // too low
        }

        public override object Part1(Input input)
        {
            var herd = new Herd();
            foreach (var line in input.Lines)
                herd.Introduce(line);

            var result = herd.Solve("root", false);
            if (result == null)
                throw new InvalidDataException();
            return result.Value;
        }

        public override object Part2(Input input)
        {
            var herd = new Herd();
            foreach (var line in input.Lines)
                herd.Introduce(line);

            Tuple<decimal, decimal> number;
            decimal value;

            var l = herd.Solve(herd.Root.Left, true);
            var r = herd.Solve(herd.Root.Right, true);
            if (l == null)
            {
                if (r == null) throw new InvalidDataException();
                number = herd.GetNumTupple(herd.Root.Left);
                value = r.Value;
            }
            else
            {
                number = herd.GetNumTupple(herd.Root.Right);
                value = l.Value;
            }
            return (long)((value - number.Item2) / number.Item1);
        }

        protected class Herd
        {
            protected IDictionary<string, Monkey> _herd = new Dictionary<string, Monkey>();

            public Monkey Root => _herd["root"];
            public void Introduce(string input)
            {
                var parts = input.Split(":", StringSplitOptions.TrimEntries);
                var operation = parts[1].Split(" ", StringSplitOptions.TrimEntries);

                if (operation.Length == 1)
                {
                    var val = long.Parse(operation[0]);
                    _herd.Add(parts[0], new Monkey()
                    {
                        Name = parts[0],
                        Value = val
                    });
                }
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
                        Name = parts[0],
                        Left = operation[0],
                        Right = operation[2],
                        Action = action,
                        operation = operation[1][0]
                    });
                }
            }

            public long? Solve(string name, bool modHumn)
            {
                if (modHumn && name == "humn")
                    return null;
                var monkey = _herd[name];
                if (!monkey.Value.HasValue)
                {
                    var l = Solve(monkey.Left, modHumn);
                    var r = Solve(monkey.Right, modHumn);
                    if (l == null || r == null || monkey.Action == null)
                        return null;
                    monkey.Value = monkey.Action.Invoke(l.Value, r.Value);
                }
                return monkey.Value.Value;
            }

            // a*humn+b
            public Tuple<decimal, decimal> GetNumTupple(string name)
            {
                if (name == "humn")
                {
                    decimal a = 1;
                    decimal b = 0;
                    return Tuple.Create(a, b);
                }
                var monkey = _herd[name];
                var lv = Solve(monkey.Left, true);
                var rv = Solve(monkey.Right, true);

                if (lv == null)
                {
                    if (rv == null) throw new InvalidDataException();
                    var t = GetNumTupple(monkey.Left);
                    var val = (decimal)rv.Value;
                    switch (monkey.operation)
                    {
                        case '+':
                            return Tuple.Create(t.Item1, t.Item2 + val);
                        case '-':
                            return Tuple.Create(t.Item1, t.Item2 - val);
                        case '*':
                            return Tuple.Create(t.Item1 * val, t.Item2 * val);
                        case '/':
                            return Tuple.Create(t.Item1 / val, t.Item2 / val);
                    }
                }
                else
                {
                    var t = GetNumTupple(monkey.Right);
                    var val = (decimal)lv.Value;
                    switch (monkey.operation)
                    {
                        case '+':
                            return Tuple.Create(t.Item1, t.Item2 + val);
                        case '-':
                            return Tuple.Create(-1 * t.Item1, val - t.Item2);
                        case '*':
                            return Tuple.Create(t.Item1 * val, t.Item2 * val);
                        case '/':
                            throw new Exception("idunno... probably not needed :P");
                    }
                }

                throw new Exception("oops, sould not reach this point...");
            }
        }

        protected class Monkey
        {
            public string Name = string.Empty;
            public long? Value;
            public string Left = string.Empty;
            public string Right = string.Empty;
            public Func<long, long, long>? Action;
            public char operation = '#';
        }
    }
}
