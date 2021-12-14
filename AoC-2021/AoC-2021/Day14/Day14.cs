using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day14
{
    [BasePath("Day14")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day14 : DayBase
    {
        public Day14(string filePath) : base(filePath)
        {
            foreach(var line in LineInput.Skip(2))
            {
                var parts = line.Trim().Split("->");
                Insertions.Add(parts[0].Trim(), parts[1].Trim());
            }
        }

        private IDictionary<string, string> Insertions = new Dictionary<string, string>();

        [ExpectedResult(TestName = "Example", Result = "1588")]
        [ExpectedResult(TestName = "Input", Result = "3259")]
        public override string Part1(string testName)
        {
            int steps = 10;
            var polymer = LineInput[0];
            while (steps-->0)
            {
                var newPolymer = $"{polymer[0]}";
                for (var i = 0; i < polymer.Length - 1; i++)
                {
                    var pair = polymer.Substring(i, 2);
                    newPolymer += $"{Insertions[pair]}{pair[1]}";
                }
                polymer = newPolymer;
            }

            var groups = polymer.GroupBy(c => c).OrderBy(g => g.Count()).ToList();

            return (groups.Last().Count() - groups.First().Count()).ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "2188189693529")]
        //[ExpectedResult(TestName = "Input", Result = "729")]
        public override string Part2(string testName)
        {
            int steps = 40;
            var polymer = LineInput[0];
            while (steps-- > 0)
            {
                var newPolymer = $"{polymer[0]}";
                for (var i = 0; i < polymer.Length - 1; i++)
                {
                    var pair = polymer.Substring(i, 2);
                    newPolymer += $"{Insertions[pair]}{pair[1]}";
                }
                polymer = newPolymer;
            }

            var groups = polymer.GroupBy(c => c).OrderBy(g => g.Count()).ToList();

            return (groups.Last().Count() - groups.First().Count()).ToString();
        }
    }
}
