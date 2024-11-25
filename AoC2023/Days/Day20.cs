using System;
using System.Collections.Generic;
using System.Linq;
using AoC.LegacyBase;
using AoCBase2;

namespace AoC2023.Days
{
    public class Day20 : LegacyAbstractDay
    {
        public override void PrepateTests(DayState<LegacyAbstractDay> dayState)
        {
            dayState.Test("example1", "./Inputs/Day20/example1.txt")
               .Part(1).Correct(21);
            //.Part(2).Correct(82000210);
            //builder.New("output", "./Inputs/Day20/output.txt")
            //    .Part(1).Correct(6935);
            //    .Part(2).Correct(790194712336);
        }
        public override string Part1(TestState input)
        {
            var broadcasts = GetBroadcastTargets(input);
            return 0.ToString();
        }

        public override string Part2(TestState input)
        {
            return 0.ToString();
        }

        private IList<IModule> GetBroadcastTargets(TestState input)
        {
            var result = new List<IModule>();
            var modules = new Dictionary<string, IModule>();
            string[] broadcasts;
            foreach(var line in input.GetLines())
            {
                var tiles = line.Split("->").Select(s => s.Trim()).ToArray();
                if (tiles[0] == "broadcaster")
                    broadcasts = tiles[1].Split(",").Select(s => s.Trim()).ToArray();
                else
                    modules.Add(tiles[0].Substring(1), tiles[0][0] switch
                    {
                        '%'=> new FlipFlop(line),
                        '&'=> new ConjunctionModule(line),
                        _ => (IModule) null
                    });
                var a = 5;
            }
            return result;
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
