using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2016.Common;
using Step = AoC2016.Days.Day1.Step;

namespace AoC2016
{
    public class Day3 : Day<IList<int>>
    {
        public Day3() : base(3, true)
        {
            Input("partTwo")
                .RunPart(2, 6)
            .Input("output")
                .RunPart(1, 869)
                .RunPart(2, 1544);
        }

        public override IList<int> Parse(string val) => val.Split(" ").Where(s => s != "").Select(s => int.Parse(s)).ToList();

        public override object Part1(IList<IList<int>> data, Input input)
        {
            var counter = 0;
            foreach(var unordered in data){
                var set = unordered.Order().ToArray();
                if (set[2] < set[0]+set[1])
                    counter++;
            }
            return counter;
        }

        public override object Part2(IList<IList<int>> data, Input input)
        {
            IList<IList<int>> newData = new List<IList<int>>();

            var lists = new List<int>[3];
            for(var i=0; i<3; i++)
                lists[i] = new List<int>();

            foreach (var one in data)
            {
                for(var i=0; i<3; i++)
                    lists[i].Add(one[i]);

                if (lists[0].Count() == 3)
                {
                    for(var i=0; i<3; i++)
                        newData.Add(lists[i]);
                    for(var i=0; i<3; i++)
                        lists[i] = new List<int>();

                }
            }
            return Part1(newData, input);
        }

        public override IList<string> Split(string val) => val.Split("\n").Select(s => s.Trim()).ToList();
    }
}