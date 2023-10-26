using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Base;
using AoC.Base.TestInputs;

namespace AoC2022
{
    public class Day16 : AbstractDay<int, IComparableInput<int>>
    {
        public override void PrepateTests(InputBuilder<int, IComparableInput<int>> builder)
        {
            builder.New("example1", "./Inputs/Day16/example1.txt")
                .Part1(1651)
                .Part2(1707);
            builder.New("output", "./Inputs/Day16/output.txt")
                .Part1(2059);
            //.RunPart(2);
        }

        //private IDictionary<string, Node> nodes;
        private int amountOfNodes;

        public override int Part1(IComparableInput<int> input)
        {
            var nodes = ReadNodes(input);
            amountOfNodes = nodes.Count();
            return GetOptimalFlow(30, 0, nodes["AA"], 0);
        }

        public override int Part2(IComparableInput<int> input)
        {
            return 5;
        }

        private IDictionary<string, Node> ReadNodes(IComparableInput<int> input)
        {
            IDictionary<string, Node> result = new Dictionary<string, Node>();
          //  var paths = new Dictionary<string, int>();
            var task = input.ReadLines();
            task.Wait();

            foreach (var line in task.Result)
            {
                var node = new Node(line);
                foreach(var x in node.Paths.Keys)
                {
                    var key = string.Join("|", ((new List<string>() { node.Name, x }).OrderBy(x => x).ToArray()));
                    //if (!paths.ContainsKey(key))
                    //    paths.Add(key, 1);
                }
                result.Add(node.Name, node);
            }
            result = CompressGraph(result);

            //dodaj mapę innych ścierzek

            var path = GetShortestPath(result["AA"], result["HH"], result.Count(), result);

            int i = 0;
            foreach (var n in result.Values)
                n.Flag = 1 << i++;

            return result;
        }

        private IDictionary<string, Node> CompressGraph(IDictionary<string, Node> result)
        {
            foreach (var kv in result)
            {
                if (kv.Key == "AA" || kv.Value.Flow > 0)
                    continue;

                foreach (var n in kv.Value.Paths)
                    result[n.Key].Paths.Remove(kv.Key);

                foreach (var l in kv.Value.Paths)
                    foreach (var r in kv.Value.Paths.Where(x => x.Key != l.Key))
                        if (!result[r.Key].Paths.ContainsKey(l.Key))
                        {
                            result[r.Key].Paths.Add(l.Key, l.Value + 1);
                            result[l.Key].Paths.Add(r.Key, l.Value + 1);
                        }
                        else if (result[r.Key].Paths[l.Key] > l.Value + r.Value)
                        {
                            result[r.Key].Paths[l.Key] = l.Value + r.Value;
                            result[l.Key].Paths[r.Key] = l.Value + r.Value;
                        }

                result.Remove(kv.Key);
            }
            return result;
        }

        private int GetShortestPath(Node start, Node end, int leftToGo, IDictionary<string, Node> nodes)
        {
            //jesli osoagnales cel zwroc 0
            if (start == end)
                return 0;
            //jesli skonczyl sie czas zwroc -1
            if (leftToGo <= 0)
                return -1;
            var min = int.MaxValue;
            //rusz sie po wszystkich, wez pod uwage najmniejsza droge wieksza niż 0
            foreach (var n in start.Paths)
            {
                var node = nodes[n.Key];
                var tmp = GetShortestPath(node, end, leftToGo - n.Value, nodes);
                if (tmp > 0 && tmp + n.Value < min)
                    min = tmp + n.Value;
            }
            return min < int.MaxValue ? min : -1;
        }

       /* private Node ExtendPaths(Node node, IDictionary<string, Node> nodes)
        {

        } */

        private int GetOptimalFlow(int time, int flow,  Node node, int openedValves)
        {
            var min = int.MaxValue;
            //jesli wszystkie są otwarte - nic więcej nie zrobisz, zwróc flow
            if (openedValves == (1 << amountOfNodes) - 1)
                return flow;
            //jeśli zawór jest zamknięty zdejmij minutę na otwarcie, przelicz jego flow i odpal ponownie
            if ((openedValves & node.Flag) == 0)
            {
                var newFlow = GetOptimalFlow(time - 1, flow + (time - 1) * node.Flow, node, openedValves | node.Flag);
                if (newFlow < min)
                    min = newFlow;
            }
            //jeśli nie masz sąsiada do któego dojdziesz, zakończ z akrualnym flow
            //w innym przypadku leć po wszystkich sąsiadach do których 

            return min;
        }

        internal class Node
        {
            public readonly string Name;
            public readonly int Flow;
            public int Flag;
            public IDictionary<string, int> Paths = new Dictionary<string, int>();
            
            public Node(string line)
            {
                var regex = new Regex(@"Valve ([A-Z]+) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? ([A-Z, ]*)");
                var match = regex.Match(line);
                Name = match.Groups[1].Value;
                Flow = int.Parse(match.Groups[2].Value);
                foreach(var n in match.Groups[3].Value.Split(","))
                {
                    Paths.Add(n.Trim(), 1);
                }
            }
        }
    }
}
