using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Base;
using AoC.Base.TestInputs;
using AoC.Common;


namespace AoC2023.Days
{
    public class Day20 : AbstractDay<long, IComparableInput<long>>
    {
        public override void PrepateTests(InputBuilder<long, IComparableInput<long>> builder)
        {
            builder.New("example1", "./Inputs/Day20/example1.txt")
               .Part1(21);
            //.Part2(82000210);
            builder.New("output", "./Inputs/Day20/output.txt")
                .Part1(6935);
                //.Part2(790194712336);
        }
        public override long Part1(IComparableInput<long> input)
        {
            return 0;
        }

        public override long Part2(IComparableInput<long> input)
        {
            return 0;
        }

        private interface IModule{
            public void Initialize(IDictionary<string, IModule> map);
            public void Input(bool signal, string sender);
            public Queue<bool> Output {get;}
        }

        private abstract class BaseModule: IModule{
            protected string Name;
            public IDictionary<string, IModule> Targets = new Dictionary<string, IModule>();
            public BaseModule(string cmd){
                Name = cmd.Split("->")[0].Trim();
                foreach(var dst in cmd.Split("->")[1].Split(",").Select(s=> s.Trim()))
                    Targets.Add(dst, null);
            }

            public Queue<bool> Output {get; protected set;} = new Queue<bool>();
            public void Initialize(IDictionary<string, IModule> map){
                foreach(var k in Targets.Keys)
                    Targets[k] = map[k];
            }
            public abstract void Input(bool signal, string sender);
        }

        private class FlipFlop : BaseModule, IModule
        {
            private bool State  = false;

            public FlipFlop(string cmd): base(cmd){}
            public override void Input(bool signal, string sender){
                if(signal == false){
                    State = !State;
                    Output.Enqueue(State);
                }
            }
        }
        private class ConjunctionModule: BaseModule, IModule{
            private IDictionary<string, bool> States = new Dictionary<string, Boolean>();
            public ConjunctionModule(string cmd): base(cmd){}

            public override void Input(bool signal, string sender)
            {
                if(!States.ContainsKey(sender))
                    States.Add(sender, false);
                States[sender] = signal;
                Output.Enqueue(!States.Values.All(v => v==true));
            }
        }
    }
}
