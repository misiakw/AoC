using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AoC2024.Days
{
    public class Day23 : IDay
    {
        public static void RunAoC() => AocRuntime.Day<Day23>(23, t => new Day23(t.GetLines()))
                .Callback(1, (d, t) => d.Part1())
                //.Callback(2, (d, t) => d.Part2())
                .Test("example")
                .Test("input")
                //.Part(1).Correct(1149)
                //.Part(2).Correct()
                .Run();

        public Day23(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var names = line.Split('-').Select(l => l.Trim()).ToArray();

                var nodeA = GetNode(names[0]);
                var nodeB = GetNode(names[1]);

                nodeA.AddNeighbour(nodeB);
                nodeB.AddNeighbour(nodeA);
            }
        }

        public string Part1()
        {
            var dict = new Dictionary<string, string[]>();

            foreach (var firstKey in Nodes.Keys.Where(k => k.StartsWith("t")))
            {
                var first = Nodes[firstKey];
                foreach (var second in first.Neighbours)
                    foreach (var thirth in first.Neighbours.Where(n => n != second))
                        if (second.Neighbours.Contains(thirth))
                        {
                            var names = new string[] { first.Name, second.Name, thirth.Name };
                            var key = string.Join(",", names.Order());
                            if (!dict.ContainsKey(key))
                                dict.Add(key, names);
                        }
            }

            return dict.Count().ToString();
        }

        private IDictionary<string, Node> Nodes = new Dictionary<string, Node>();
        private Node GetNode(string name)
        {
            if (!Nodes.ContainsKey(name))
                Nodes.Add(name, new Node(name));
            return Nodes[name];
        }


        private class Node
        {
            public void AddNeighbour(Node node)
            {
                if (!Neighbours.Contains(node))
                    Neighbours.Add(node);
            }
            
            public Node(string name) { Name = name; }
            public readonly string Name;
            public IList<Node> Neighbours = new List<Node>();
        }
    }
}
