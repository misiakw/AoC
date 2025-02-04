using AdventOfCode.Attributes;
using AdventOfCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day6
{
    [Day("Day6")]
    [Input("test", typeof(Day6TestInput))]
    [Input("task2", typeof(Day6Task2Input))]
    [Input("aoc", typeof(Day6AoCInput))]
    public class Day6 : IDay
    {
        private Dictionary<string, Satelite> _planets = new Dictionary<string, Satelite>();
        public string Task1(IInput input)
        {
            foreach (var line in input.Input.Split("\n").Select(l => l.Trim()))
            {
                var sateliteNames = line.Split(')');
                var center = GetSatelite(sateliteNames[0]);
                var satelite = GetSatelite(sateliteNames[1]);

                center.Satelites.Add(satelite);
                satelite.Center = center;
            }

            return _planets.Values.Select(p => p.Orbits).Sum().ToString();
        }

        public string Task2(IInput input)
        {
            var location = GetSatelite("YOU");
            var target = GetSatelite("SAN");

            var steps = 0;

            //go to common root 
            while (!location.Below.Contains(target))
            {
                steps++;
                location = location.Center;
            }

            //go down
            while (!location.Satelites.Contains(target))
            {
                steps++;
                location = location.Satelites.First(s => s.Below.Contains(target));
            }

            return (steps - 1).ToString();
        }

        private Satelite GetSatelite(string name)
        {
            if (!_planets.ContainsKey(name))
            {
                _planets.Add(name, new Satelite(name));
            }
            return _planets[name];
        }

        private class Satelite
        {
            public readonly string Name;
            public readonly IList<Satelite> Satelites;
            public Satelite Center;
            private List<Satelite> _below;

            public Satelite(string name)
            {
                Name = name;
                Satelites = new List<Satelite>();
            }

            public List<Satelite> Below
            {
                get
                {
                    if (_below == null)
                    {
                        _below = new List<Satelite>(Satelites);
                        foreach (var s in Satelites)
                            _below.AddRange(s.Below);
                    }
                    return _below;
                }
            }

            public long Orbits => Center == null ? 0 : Center.Orbits + 1;

            public override string ToString()
            {
                return $"{Name}({Orbits})";
            }
        }
    }
}
