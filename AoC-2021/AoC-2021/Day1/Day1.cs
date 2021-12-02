using AoC_2021.Attributes;
using AoC_2021.InputClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day1
{
    [BasePath("Day1")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day1 : LongDay
    {
        public Day1(string path) : base(path)
        {}

        public override string Part1()
        {
            var diffs = GetDifs(this.Input.ToList());

            return diffs.Where(k => k.Item2 < 0).Count().ToString();
        }

        public override string Part2()
        {
            var slided = new List<long>();
            for(var i=0; i+3 <= this.Input.Count; i++)
            {
                slided.Add(this.Input.Skip(i).Take(3).Sum());
            }

            var diffs = GetDifs(slided);

            return diffs.Where(k => k.Item2 < 0).Count().ToString();
        }

        private IList<Tuple<long, long>> GetDifs(IList<long> input)
        {

            var diffs = new List<Tuple<long, long>>();
            var prev = input.First();
            diffs.Add(Tuple.Create(prev, (long)0));

            foreach (var current in input.Skip(1))
            {
                diffs.Add(Tuple.Create(current, prev - current));
                prev = current;
            }

            return diffs;
        }
    }
}
