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
                .RunPart(1, 1651)
            .Input("output")
                .RunPart(1); //1709 to low, 2839 too high //2000, 1822 Not Right
                //WhyNot 1869 
        }

        public override object Part1(Input input)
        {
            var AA = ReadInput(input);

            var toOpen = AA.DistanceTo.Keys.ToList();

            var result =  Process(AA, 30, 0, "AA");
            Console.WriteLine(result.Item2);
            return result.Item1;

        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }
        private Valve ReadInput(Input input){
            var valves = new Dictionary<string, Valve>();
            var paths = new Dictionary<string, string[]>();
            var regex = new Regex(@"Valve ([A-Z]{2}) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? ([A-Z, ]*)");

            foreach(var line in input.Lines){
                var match = regex.Match(line);
                var valve = new Valve(match.Groups[1].Value, int.Parse(match.Groups[2].Value));
                valves.Add(valve.Name, valve);
                paths.Add(valve.Name, match.Groups[3].Value.Split(", ").ToArray());

                foreach(var dest in paths[valve.Name])
                    if (valves.ContainsKey(dest))
                        valve.Leads.Add(valves[dest]);

                foreach(var src in paths.Where(p => p.Value.Contains(valve.Name)).Select(p => p.Key))
                    valves[src].Leads.Add(valve);
            }

            foreach(var valve in valves.Values){
                foreach(var dst in valves.Values.Where(v => v.Flow > 0 && v!= valve))
                    valve.DistanceTo.Add(dst, valve.PathTo(dst, 0, string.Empty));
            }

            return valves["AA"];
        }

        private Tuple<int, string> Process(Valve current, int stepsToGo, int value, string opened)
        {
            if(stepsToGo <0){
                return Tuple.Create(value, opened); //no time left
            }

            var split = opened.Split("=>").Select(s => s.Split("|").First()).ToArray();
            var potentials = current.DistanceTo.Keys.Where(v => split.All(s => s != v.Name)).ToList();
            var flowSet = current.DistanceTo.Keys.Where(v => split.Contains(v.Name)).ToList();
            var flow = flowSet.Select(v=> v.Flow).Sum() + current.Flow;

            if (!potentials.Any()){
                opened += $"|{stepsToGo}*{flow}*{value}*{stepsToGo*flow}*{value+(stepsToGo*flow)}";
                return Tuple.Create(value+(stepsToGo*flow), opened); //no change, in future calculate 
            }

            var result = Tuple.Create(int.MinValue, "default");
            foreach(var next in potentials){
                    var steps = current.DistanceTo[next] + 1;
                    if (steps >= stepsToGo){
                        steps = stepsToGo;
                        return Tuple.Create(value+(flow * steps), opened);
                    }
                    var output = Process(next, stepsToGo-steps, value+(flow * steps), opened+$"|{steps}*{flow}*{value}*{flow * steps}=>{next.Name}");
                    if(output.Item1 > result.Item1) result = output;
                }

            return result;
        }

        internal class Valve{
            public readonly string Name;
            public readonly int Flow;
            public IList<Valve> Leads = new List<Valve>();
            public bool Opened = false;

            public IDictionary<Valve, int> DistanceTo = new Dictionary<Valve, int>();

            public Valve(string name, int flow){
                Name = name;
                Flow = flow;
            }
            public override string ToString() => $"Valve[{Name}, {Flow}]";

            public int PathTo(Valve dest, int steps, string visited){
                if (this == dest) return steps;

                var min = int.MaxValue;

                var visitedStr = visited.Split(",").ToList();

                foreach(var lead in Leads.Where(l => !visitedStr.Any(v => l.Name == v))){
                    var path = lead.PathTo(dest, steps+1, visited != string.Empty? visited+$",{lead.Name}": lead.Name);
                    if (path < min) min = path;
                }

                return min;
            }
        }
    }
}
