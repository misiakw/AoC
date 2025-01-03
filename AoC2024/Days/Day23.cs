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
                .Callback(2, (d, t) => d.Part2())
                .Test("example")
                .Test("input")
                //.Part(1).Correct(1149)
                //.Part(2).Correct("as,co,do,kh,km,mc,np,nt,un,uq,wc,wz,yo")
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

            foreach (var firstKey in Nodes.Keys)
            {
                var first = Nodes[firstKey];
                foreach (var second in first.Neighbours)
                    foreach (var thirth in first.Neighbours.Where(n => n != second))
                        if (second.Neighbours.Contains(thirth))
                        {
                            var names = new string[] { first.Name, second.Name, thirth.Name };
                            var key = string.Join(",", names.Order());
                            if (!dict.ContainsKey(key))
                            {
                                dict.Add(key, names);
                            }
                        }
            }
            return dict.Where(d => d.Value.Any(t => t.StartsWith("t"))).Count().ToString();
        }

        public string Part2()
        {
            if (Nodes.Count == 0)
                Part1();

            var cliques = new List<Clique>();
            foreach (var start in Nodes.Values.OrderBy(n => n.Neighbours.Count()))
            {
                var clique = new Clique();
                clique.Nodes.Add(start);

                foreach (var node in Nodes.Values)
                {
                    if (node == start) continue;
                    if (clique.Nodes.All(node.Neighbours.Contains))
                        clique.Nodes.Add(node);
                }
                cliques.Add(clique);
            }


            return cliques.OrderByDescending(c => c.Nodes.Count()).First().ToString();
        }

        private IDictionary<int, int> NodeCount = new Dictionary<int, int>();
        private IDictionary<string, Node> Nodes = new Dictionary<string, Node>();
        private Node GetNode(string name)
        {
            if (!Nodes.ContainsKey(name))
            {
                Nodes.Add(name, new Node(name));
            }
            return Nodes[name];
        }


        private class Node
        {
            public void AddNeighbour(Node node)
            {
                if (!Neighbours.Contains(node))
                    Neighbours.Add(node);
            }
            public IList<Clique> Cliques = new List<Clique>();
            public Node(string name) { Name = name; }
            public readonly string Name;
            public IList<Node> Neighbours = new List<Node>();
            public override string ToString() => Name;
        }

        private class Clique
        {
            public IList<Node> Nodes = new List<Node>();
            public override string ToString() => string.Join(",", Nodes.Select(n => n.Name).Order());
        }
    }
}
