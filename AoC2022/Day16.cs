using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day16 : DayBase
    {
        public Day16() : base(16)
        {
            Input("example1")
            //.RunPart(1, 1651)
            //.RunPart(2, 1707)
            .Input("output")
                //.RunPart(1, 2059)
                .RunPart(2);
        }

        public override object Part1(Input input)
        {
            var AA = ReadInput(input);
            input.Cache = AA;
            var result = Process(AA, 30, 0, "AA");
            return result;
        }

        public override object Part2(Input input)
        {
            var AA = ReadInput(input);
            var valves = AA.DistanceTo.Keys.ToList();
            valves.Add(AA);
            Console.WriteLine("input prepared, start");
            var result = ProcessPart2("AA", 0, 0, "AA", 0, 0, "AA", "", valves.ToDictionary(k => k.Name));
            return result;
        }
        private Valve ReadInput(Input input)
        {
            var valves = new Dictionary<string, Valve>();
            var paths = new Dictionary<string, string[]>();
            var regex = new Regex(@"Valve ([A-Z]{2}) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? ([A-Z, ]*)");

            foreach (var line in input.Lines)
            {
                var match = regex.Match(line);
                var valve = new Valve(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
                valves.Add(valve.Name, valve);
                paths.Add(valve.Name, match.Groups[3].Value.Split(", ").ToArray());

                foreach (var dest in paths[valve.Name])
                    if (valves.ContainsKey(dest))
                        valve.Leads.Add(valves[dest]);

                foreach (var src in paths.Where(p => p.Value.Contains(valve.Name)).Select(p => p.Key))
                    valves[src].Leads.Add(valve);
            }

            foreach (var valve in valves.Values)
            {
                foreach (var dst in valves.Values.Where(v => v.Flow > 0 && v != valve))
                    valve.DistanceTo.Add(dst, valve.PathTo(dst, 0, string.Empty));
            }

            return valves["AA"];
        }

        private int Process(Valve current, int stepsToGo, int value, string opened)
        {
            if (stepsToGo < 0)
            {
                return value; //no time left
            }

            var split = opened.Split("=>").Select(s => s.Split("|").First()).ToArray();
            var potentials = current.DistanceTo.Where(kv => kv.Value < stepsToGo)
                .Select(kv => kv.Key).Where(v => split.All(s => s != v.Name)).ToList();
            var flowSet = current.DistanceTo.Keys.Where(v => split.Contains(v.Name)).ToList();
            var flow = flowSet.Select(v => v.Flow).Sum() + current.Flow;

            if (!potentials.Any())
            {
                return value + (stepsToGo * flow); //no change, in future calculate 
            }

            var result = int.MinValue;
            foreach (var next in potentials)
            {
                var steps = current.DistanceTo[next] + 1;
                var output = steps > stepsToGo
                    ? Process(next, 0, value + (flow * stepsToGo), opened)
                    : Process(next, stepsToGo - steps, value + (flow * steps), opened + $"=>{next.Name}");
                if (output > result) result = output;
            }

            return result;
        }
        private int ProcessPart2(
            string me, int meTick, int meFlow, string ele, int eleTick, int eleFlow,
            string usedArr, string path,
            IDictionary<string, Valve> valves)
        {
            if (meTick >= 30 && eleTick >= 30)
                return -1;//

            var available = valves.Where(kv => !usedArr.Split(",").Contains(kv.Key))
                .Select(kv => kv.Value).ToList();

            if (meTick <= eleTick)
            {
                var meVal = valves[me];
                var toUse = meVal.DistanceTo
                    .Where(kv => kv.Value < (30 - meTick - 1) && available.Contains(kv.Key));

                if (!toUse.Any())
                {
                    return ProcessPart2(me, 30, meFlow, ele, eleTick, eleFlow, usedArr,
                        path + $"=>{me}|m|FIN", valves);
                }

                var result = new List<int>();
                foreach (var tu in toUse)
                {
                    var newTick = meTick + meVal.DistanceTo[tu.Key] + 1;
                    var newFlow = meFlow + tu.Key.Flow;
                    result.Add(ProcessPart2(tu.Key.Name, newTick, newFlow, ele, eleTick, eleFlow,
                        usedArr + $"=>{tu.Key.Name}", path + $"{tu.Key.Name}|m|{newTick}", valves));
                }
                return result.Max();
            }
            else
            {
                var eleVal = valves[ele];
                var toUse = eleVal.DistanceTo
                    .Where(kv => kv.Value < (30 - eleTick - 1) && available.Contains(kv.Key));

                if (!toUse.Any())
                {
                    return ProcessPart2(me, meTick, meFlow, ele, 30, eleFlow, usedArr,
                        path + $"=>{me}|e|FIN", valves);
                }

                var result = new List<int>();
                foreach (var tu in toUse)
                {
                    var newTick = eleTick + eleVal.DistanceTo[tu.Key] + 1;
                    var newFlow = eleFlow + tu.Key.Flow;
                    result.Add(ProcessPart2(me, meTick, meFlow, tu.Key.Name, newTick, newFlow,
                        usedArr + $"=>{tu.Key.Name}", path + $"{tu.Key.Name}|e|{newTick}", valves));
                }
                return result.Max();
            }
        }

        /*if(tick == 0)
            return i++;

        var meList = meInp.Split("=>").Select(s=>s.Split("|").ToArray());
        var mePos = int.Parse(meList.Last()[0]);
        var meVal = mePos == tick ? valves[meList.Last()[1]]: null;
        var visited = meList.Select(v => v[1]).ToList();

        var eleList = eleInp.Split("=>").Select(s=>s.Split("|").ToArray());
        var elePos = int.Parse(eleList.Last()[0]);
        var eleVal = elePos == tick ? valves[eleList.Last()[1]]: null;
        visited.AddRange(eleList.Select(v => v[1]));

        var available = valves.Where(kv => !visited.Contains(kv.Key)).Select(kv => kv.Value).ToList();

        if(tick == mePos){
            var set = meVal.DistanceTo
                .Where(kv => kv.Value < mePos && available.Contains(kv.Key));
            if (!set.Any()){
                return i++;
            }
            var result = new List<int>();
            foreach(var valve in set){
                var nextPos = mePos - valve.Value - 1;
                result.Add(MeAndEleList(meInp+$"=>{nextPos}|{valve.Key.Name}", 
                    eleInp, nextPos > elePos ? elePos : nextPos,
                    valves, path+$"=>{valve.Key.Name}|m|{nextPos}"));
            }
            return result.Max();
        }else{
            var set = eleVal.DistanceTo
                .Where(kv => kv.Value < elePos && available.Contains(kv.Key));
            if (!set.Any()){
                return i++;
            }
            var result = new List<int>();
            foreach(var valve in set){
                var nextPos = elePos - valve.Value - 1;
                result.Add(MeAndEleList(meInp, eleInp+$"=>{nextPos}|{valve.Key.Name}",
                    nextPos > mePos ? mePos : nextPos,
                    valves, path+$"=>{valve.Key.Name}|m|{nextPos}"));
            }
            return result.Max();
        }*/
    }

    internal class Valve
    {
        public readonly string Name;
        public readonly int Flow;
        public IList<Valve> Leads = new List<Valve>();
        public bool Opened = false;

        public IDictionary<Valve, int> DistanceTo = new Dictionary<Valve, int>();

        public Valve(string name, int flow)
        {
            Name = name;
            Flow = flow;
        }
        public override string ToString() => $"Valve[{Name}, {Flow}]";

        public int PathTo(Valve dest, int steps, string visited)
        {
            if (this == dest) return steps;

            var min = int.MaxValue;

            var visitedStr = visited.Split(",").ToList();

            foreach (var lead in Leads.Where(l => !visitedStr.Any(v => l.Name == v)))
            {
                var path = lead.PathTo(dest, steps + 1, visited != string.Empty ? visited + $",{lead.Name}" : lead.Name);
                if (path < min) min = path;
            }

            return min;
        }
    }
}
