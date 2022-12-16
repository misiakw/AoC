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
            .Input("output");
        }

        public override object Part1(Input input)
        {
            var AA = ReadInput(input);
            return Process(AA, new Valve[0], 0, 1, input.Lines.Count());
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

            return valves["AA"];
        }

        private int Process(Valve current, Valve[] opened, int alreadyFlown, int time, int numOfValves){
            if (time == 31) return alreadyFlown;

            time++;
            alreadyFlown += opened.Sum(v => v.Flow);

            var openedList = new List<Valve>(opened);
            if(!opened.Contains(current)){ // open valve
                openedList.Add(current);
                if (time == 31) return alreadyFlown;
            }

            if(openedList.Count() == numOfValves){
                while(time < 31){
                    time++;
                    alreadyFlown += opened.Sum(v => v.Flow);
                }
                return alreadyFlown;
            }

            var result = int.MinValue; //move to next
            foreach(var next in current.Leads){
                var thisResult = Process(next, openedList.ToArray(), alreadyFlown, time, numOfValves);
                if (thisResult > result)
                    result = thisResult;
            }

            return result;
        }

        internal class Valve{
            public readonly string Name;
            public readonly int Flow;
            public IList<Valve> Leads = new List<Valve>();

            public Valve(string name, int flow){
                Name = name;
                Flow = flow;
            }
            public override string ToString() => $"Valve[{Name}, {Flow}]";
        }
    }
}
