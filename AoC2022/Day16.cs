using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                .Part1(2059)
                .Part2(2790);
        }


        private IDictionary<string, int> cache = new Dictionary<string, int>();
        private int amountOfNodes = 0;

        public override int Part1(IComparableInput<int> input)
        {
            var nodes = ReadNodes(input);
            amountOfNodes = nodes.Count();
            return GetOptimalFlow(30, nodes["AA"], 0, nodes);
        }

        public override int Part2(IComparableInput<int> input)
        {
            var nodes = ReadNodes(input);
            var allOptions = (1 << nodes.Count) - 1;

            var max = 0;
            for (var i = 0; i< (allOptions+1)/2; i++)
            {
                var me = GetOptimalFlow(26,  nodes["AA"], i, nodes);
                var elefant = GetOptimalFlow(26, nodes["AA"], ~i, nodes);
                if (me + elefant > max)
                {
                    max = me + elefant;
                    Console.WriteLine(max);
                }
            }
            return max;
        }

        private IDictionary<string, Node> ReadNodes(IComparableInput<int> input)
        {
            IDictionary<string, Node> result = new Dictionary<string, Node>();
            var task = input.ReadLines();
            task.Wait();

            foreach (var line in task.Result)
            {
                var node = new Node(line);
                result.Add(node.Name, node);
            }
            result = CompressGraph(ExtendPaths(result));

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

        private IDictionary<string, Node> ExtendPaths(IDictionary<string, Node> nodes)
        {
            var amountOfNodes = nodes.Count();
            while (nodes.Values.Any(v => v.Paths.Count < amountOfNodes - 1))
            {
                var startNode = nodes.Values.First(v => v.Paths.Count < amountOfNodes - 1);
                var keysToProcess = startNode.Paths.Keys.ToArray();
                foreach (var midNodeKey in keysToProcess)
                    foreach (var endNodeKey in nodes[midNodeKey].Paths.Keys)
                        if (startNode.Name != endNodeKey && !startNode.Paths.ContainsKey(endNodeKey))
                        {
                            var dist = nodes[midNodeKey].Paths[endNodeKey] + startNode.Paths[midNodeKey];
                            startNode.Paths.Add(endNodeKey, dist);
                            nodes[endNodeKey].Paths.Add(startNode.Name, dist);
                        }
            }
            return nodes;
        }

        private int GetOptimalFlow(int time,  Node node, int openedValves, IDictionary<string, Node> nodes)
        {
            var state = $"{time}|{node.Name}|{openedValves}";
            if (cache.ContainsKey(state))
                return cache[state];

            var max = 0;
            if ((openedValves & node.Flag) == 0)
                max = GetOptimalFlow(time - 1, node, openedValves | node.Flag, nodes) + (time - 1) * node.Flow;

            foreach(var next in node.Paths.Where(kv => kv.Value < time).Select(k => nodes[k.Key]).Where(n => (openedValves & n.Flag) == 0))
            {
                var newFlow = GetOptimalFlow(time - node.Paths[next.Name], next, openedValves, nodes);
                if (newFlow > max)
                    max = newFlow;
            }

            cache.Add(state, max);
            return max;
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
