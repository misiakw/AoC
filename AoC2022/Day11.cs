using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
//using System.Numerics;

namespace AoC2022
{
    public class Day11 : DayBase
    {
        public Day11() : base(11)
        {
            Input("example1")
                .RunPart(1, 10605L)
                .RunPart(2, 2713310158L)
            .Input("output")
                .RunPart(1, 58056L)
                .RunPart(2, 15048718170L);
        }

        public override object Part1(Input input)
        {
            var inserts = BuildMonkeyInserts(input.Lines.Select(l => l.Trim()).ToArray())
                .ToArray();
            input.Cache = inserts;

            var monkeys = BuildMonkeys(inserts);

            for(var i=0; i< 20; i++)
                foreach(var monkey in monkeys)
                    monkey.Process(3);

            var top = monkeys.Select(m => m.Ctr).OrderByDescending(m => m).Take(2).ToArray();
            return top[0]*top[1];
        }

        public override object Part2(Input input)
        {
            var inserts = (MonkeyInsert[])(input.Cache ?? new MonkeyInsert[0]);

            var monkeys = BuildMonkeys(inserts);

            for(var i=0; i< 10000; i++)
                foreach(var monkey in monkeys)
                    monkey.Process(1);

            var top = monkeys.Select(m => m.Ctr).OrderByDescending(m => m).Take(2).ToArray();
            return top[0]*top[1];
        }

        private class ModNum{
            public static int[] modulus = new int[0];

            private long val;

            private IDictionary<int, long> Value = new Dictionary<int, long>();
            public ModNum(long value){
                foreach(var mod in modulus)
                    Value.Add(mod, value%mod);
                val = value;
            }

            public bool IsDividable(int div){
                if (Value.ContainsKey(div))
                    return Value[div]%div == 0;
                return val % div == 0;
            }

            public static ModNum operator *(ModNum a, ModNum b){
                foreach(var key in a.Value.Keys){
                    var value = a.Value[key];
                    a.Value[key] = (value%key * b.Value[key]%key)%key;
                }
                a.val *= b.val;
                return a;
            }
            public static ModNum operator *(ModNum a, int b){
                foreach(var key in a.Value.Keys){
                    var value = a.Value[key];
                    a.Value[key] = (value%key * b%key)%key;
                }
                a.val *= b;
                return a;
            }
            public static ModNum operator +(ModNum a, int b){
                foreach(var key in a.Value.Keys){
                    var value = a.Value[key];
                    a.Value[key] = (value%key + b%key)%key;
                }
                a.val += b;
                return a;
            }
            public static ModNum operator /(ModNum a, int b){
                return b == 1 ? a : new ModNum(a.val / b);
            }
        }
        private struct MonkeyInsert{
            public int Number;
            public int[] Items;
            public int divider;
            public string[] operation;
            public int[] Targets;
        }

        private IEnumerable<MonkeyInsert> BuildMonkeyInserts(string[] input){
            for(var i=0; i<input.Count(); i+=7){
                var set = new List<string>();
                for(var x = 0; x< 6 ; x++)
                    set.Add(input[i+x]);
                 yield return BuildMonkeyInsert(set.ToArray());
            }
        }

        private MonkeyInsert BuildMonkeyInsert(string[] input) => new MonkeyInsert
        {
            Number = int.Parse(input[0].Replace("Monkey ", "").Replace(":", "")),
            Items = input[1].Replace("Starting items: ", "").Split(", ", StringSplitOptions.TrimEntries).Select(e => int.Parse(e)).ToArray(),
            divider = int.Parse(input[3].Replace("Test: divisible by ", "")),
            operation = input[2].Replace("Operation: new = ", "").Split(" ", StringSplitOptions.TrimEntries).ToArray(),
            Targets = string.Join("|", input.Skip(4).ToArray())
                .Replace("true", "").Replace("false", "").Replace("If : throw to monkey ", "").Split("|")
                .Select(n => int.Parse(n)).ToArray()
        };

        private class Monkey{
            public readonly MonkeyInsert Insert;
            public Monkey? ifTrue = null;
            public Monkey? ifFalse = null;
            public Queue<ModNum> Objs = new Queue<ModNum>();
            public long Ctr = 0;
            private int a, b, c;
            public Monkey(MonkeyInsert insert){
                Insert = insert;
                foreach(var obj in Insert.Items)
                    Objs.Enqueue(new ModNum(obj));

                if(Insert.operation[1] == "*")
                    if(insert.operation[0] == insert.operation[2])
                        a = 1;
                    else
                        b = int.Parse(Insert.operation[2]);
                else
                    c = int.Parse(Insert.operation[2]);
            }

            public void Process(int divider){
                for(;Objs.Any(); Ctr++){
                    var item = Calculate(Objs.Dequeue())/divider;
                    (item.IsDividable(Insert.divider)? ifTrue: ifFalse)?
                        .Objs.Enqueue(item);
                }

            }
            private ModNum Calculate(ModNum num){
                if(a != 0)
                    return num * num;
                else if(b != 0)
                    return num * b;
                else
                    return num + c;
            }
        }

        private Monkey[] BuildMonkeys(MonkeyInsert[] inserts){
            ModNum.modulus = inserts.Select(m => m.divider).Distinct().ToArray();
            var monkeys = new List<Monkey>();

            foreach(var insert in inserts)
                monkeys.Add(new Monkey(insert));

            foreach(var monkey in monkeys){
                monkey.ifTrue = monkeys.First(m => m.Insert.Number == monkey.Insert.Targets[0]);
                monkey.ifFalse = monkeys.First(m => m.Insert.Number == monkey.Insert.Targets[1]);
            }

            return monkeys.OrderBy(m => m.Insert.Number).ToArray();
        }
    }
}