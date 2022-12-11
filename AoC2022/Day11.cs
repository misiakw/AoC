using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AoC.Base;
using Monkey = AoC2022.Days.Day11.Monkey;

namespace AoC2022
{
    public class Day11 : Day<Monkey>
    {
        public Day11() : base(11, false)
        {
            Input("example1")
                .RunPart(1, 10605L)
                .RunPart(2, 2713310158L)
            .Input("output")
                .RunPart(1, 58056L)
                .RunPart(2); //1577219568 too low
        }

        public override Monkey Parse(string val) => new Monkey(val);

        public override object Part1(IList<Monkey> data, Input input)
        {
            Monkey.Truncate(); //cleanup temp before next test
            var monkeys = data.OrderBy(m => m.Number).ToList();
            input.Cache = monkeys;

            for(var i=0; i<20; i++)
                foreach(var monkey in monkeys)
                    monkey.Inspect(3);

            var result = monkeys.OrderByDescending(m => m.Ctr).Take(2).Select(m => m.Ctr).ToArray();

            return (long)result[0]*(long)result[1];
        }

        public override object Part2(IList<Monkey> data, Input input)
        {
            var monkeys = ((IList<Monkey>)input.Cache).OrderBy(m => m.Number).ToList();
            foreach (var monkey in monkeys)
                monkey.Ctr = 0;

            for(var i=0; i<10000; i++)
                foreach(var monkey in monkeys)
                    monkey.Inspect(1);

            var result = monkeys.OrderByDescending(m => m.Ctr).Take(2).Select(m => m.Ctr).ToArray();


            return result[0]*result[1];
        }

        public override IList<string> Split(string val) => val.Split("\n\n").ToList();
    }
}
namespace AoC2022.Days.Day11{
    public class Monkey{
        private static IDictionary<int, Monkey> Herd = new Dictionary<int, Monkey>();
        public readonly int Number;
        public Queue<ModNum> ToInspect = new Queue<ModNum>();
        private InspectorFactor inspector;
        private long dividor = 1;
        public int ifTrueDest, ifFalseDest;
        public Monkey ifTrue, ifFalse;
        public int Ctr = 0;

        public Monkey(string description){
            var lines = description.Split("\n");

            Number = int.Parse(lines[0].Substring(7, lines[0].Length-8));

            var objs = lines[1].Substring(18, lines[1].Length-18)
                .Split(", ").Select(s => long.Parse(s));
            foreach(var obj in objs)
                ToInspect.Enqueue(new ModNum(obj));

            inspector = new InspectorFactor(lines[2].Split("=", StringSplitOptions.TrimEntries)[1]);

            dividor = long.Parse(lines[3].Replace("Test: divisible by", "").Trim());

            ifTrueDest = int.Parse(lines[4].Replace("If true: throw to monkey", "").Trim());
            ifFalseDest = int.Parse(lines[5].Replace("If false: throw to monkey", "").Trim());

            foreach (var monkey in Herd.Values) monkey.Introduce(this);
            Herd.Add(Number, this);
        }

        public void Introduce(Monkey newbie){
            if(newbie.ifTrueDest == Number) newbie.ifTrue = this;
            if(newbie.ifFalseDest == Number) newbie.ifFalse = this;

            if(ifTrueDest == newbie.Number) ifTrue = newbie;
            if(ifFalseDest == newbie.Number) ifFalse = newbie;
        }

        public void Inspect(int worryReducer){
            if (ToInspect.Count() == 0)
                return;

            while(ToInspect.Count > 0){
                var lvl = ToInspect.Dequeue();
                lvl = inspector.Calculate(lvl)/worryReducer;

                var nextMonkey = (lvl%dividor == 0) ? ifTrue : ifFalse;
                nextMonkey.ToInspect.Enqueue(lvl);
                Ctr++;
            }
        }

        public static void Truncate(){
            Herd = new Dictionary<int, Monkey>();
        }
        public override string ToString() => $"Monkey[{Number}: Ctr={Ctr}]";

        private class InspectorFactor{
            private long? b;
            private Func<ModNum, ModNum> operatorFunc;

            public InspectorFactor(string input){
                var parts = input.Split(" ");
                if(parts[0] == parts[2]){
                    operatorFunc = (a) => a.Sqrt();
                }
                else{
                    if (parts[2] != "old") 
                        b = long.Parse(parts[2]);
                    switch (parts[1][0]){
                        case '*':
                            operatorFunc = (a) => a * (b ?? 1);
                            break;
                        case '+':
                            operatorFunc = (a) => a + (b ?? 0);
                            break;
                        default:
                            throw new InvalidOperationException($"Unknown operator ${parts[1]}");
                    }
                }
            }

            public ModNum Calculate(ModNum old) => 
                operatorFunc.Invoke(old);
        }
    }

    public class ModNum{
        public ModNum(long rest){
            Rest = rest;
        }
        public long Rest;
        private IDictionary<long, int> Multiplications = new Dictionary<long, int>();
        public static ModNum operator *(ModNum a, long b){
            if (a.Multiplications.Any()){
                a.Multiplications.PushAdd(b);
                a.Rest *= b;
            }else{
                a.Multiplications.Add(a.Rest, 1);
                a.Multiplications.PushAdd(b);
                a.Rest = 0;
            }
            return a;
        }

        public static ModNum operator +(ModNum a, long val){
            a.Rest += val;
            return a;
        }
        public static ModNum operator /(ModNum a, long div) => div == 1 ? a : new ModNum(a.ToLong() / div)

        public static long operator %(ModNum a, long mod){
            return a.ToLong()%mod;

            var mul = a.Multiplications.Any() ? 1L: 0L;

            if(!a.Multiplications.Any() || a.Multiplications.Keys.Any(k => k%mod == 0) 
                || a.Multiplications.Values.Any(v => v%mod == 0))
                return a.Rest % mod;

            foreach(var kv in a.Multiplications){
                var tmp = (kv.Key%mod)*kv.Value;
                mul *= tmp;
            }
            return (mul + a.Rest)%mod;
        }

        public long ToLong(){
            if(Multiplications.Any()){
                var result = 1L;
                foreach (var kv in Multiplications)
                    result *= (kv.Key * kv.Value);
                return result + Rest;
            }
            else 
                return Rest;
        }
        public ModNum Sqrt(){
            return new ModNum(this.ToLong()*this.ToLong());
            foreach(var k in Multiplications.Keys)
                Multiplications[k] += Multiplications[k];
            Rest = Rest * Rest;
            return this;
        }
    }

    public static class Ext{
        public static IDictionary<long, int> PushAdd(this IDictionary<long, int> dict, long val){
            if(dict.ContainsKey(val))
                dict[val]++;
            else
                dict.Add(val, 1);
            return dict;
        }
    }
}
