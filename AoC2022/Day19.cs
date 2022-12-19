using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AoC.Base;

namespace AoC2022
{
    public class Day19 : DayBase
    {
        public Day19() : base(19)
        {
            Input("example1")
                .RunPart(1, 33)
            .Input("output");
        }

        public override object Part1(Input input)
        {
            var blueprints = ReadInput(input).ToArray();
            throw new NotImplementedException();
        }

        public override object Part2(Input input)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Blueprint> ReadInput(Input input){
            foreach(var line in input.Lines)
                yield return new Blueprint(line);
        }

        private class Blueprint{
            public readonly int Number;
            public IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> PriceList;
            public Blueprint(string recepie){
                var parts = recepie.Split(":", StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim()).ToArray();
                Number = int.Parse(parts[0].Replace("Blueprint ", ""));

                var prices = new Dictionary<string, IReadOnlyDictionary<string, int>>();

                var regex = new Regex(@"Each ([a-z]*)? robot costs ((([0-9]*) ([a-z]*))+)");
                foreach(var single in parts[1].Trim().Split(".", StringSplitOptions.RemoveEmptyEntries)){
                    var match = regex.Match(single);
                    var costs = new Dictionary<string, int>();
                    foreach(var cost in match.Groups[2].Value.Split(" and ").Select(c => c.Split( )))
                        costs.Add(cost[1], int.Parse(cost[0]));
                    prices.Add(match.Groups[1].Value, costs.AsReadOnly());
                }
                PriceList = prices.AsReadOnly();
            }
        }
    }

    
}
