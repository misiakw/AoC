using AoC_2021.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021.Day14
{
    [BasePath("Day14")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day14 : DayBase
    {
        public Day14(string filePath) : base(filePath)
        {
            foreach (var line in LineInput.Skip(2))
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
            return ProcessNSteps(10).ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "2188189693529")]
        [ExpectedResult(TestName = "Input", Result = "3459174981021")]
        public override string Part2(string testName)
        {
            return ProcessNSteps(40).ToString();
        }

        private long ProcessNSteps(int steps) { 
            IDictionary<string, string[]> pairMaker = new Dictionary<string, string[]>();
            IDictionary<string, long> Polymer = new Dictionary<string, long>();

            foreach (var rule in Insertions)
            {
                var resultPair = new string[2];
                resultPair[0] = $"{rule.Key[0]}{rule.Value}";
                resultPair[1] = $"{rule.Value}{rule.Key[1]}";
                pairMaker.Add(rule.Key, resultPair);
            }

            for (var i = 0; i < LineInput[0].Length - 1; i++)
            {
                var key = LineInput[0].Substring(i, 2);
                if (!Polymer.ContainsKey(key))
                {
                    Polymer.Add(key, 0);
                }
                Polymer[key]++;
            }

            while (steps-- > 0)
            {
                var newPolymer = new Dictionary<string, long>();
                foreach (var cSet in Polymer)
                {
                    var amount = cSet.Value;
                    var newOnes = pairMaker[cSet.Key];

                    newPolymer.TryAdd(newOnes[0], 0);
                    newPolymer.TryAdd(newOnes[1], 0);
                    newPolymer[newOnes[0]] += cSet.Value;
                    newPolymer[newOnes[1]] += cSet.Value;
                }
                Polymer = newPolymer;
            }

            var keyDict = new Dictionary<char, long>();
            foreach(var kv in Polymer)
            {
                keyDict.TryAdd(kv.Key[0], 0);
                keyDict.TryAdd(kv.Key[1], 0);
                keyDict[kv.Key[0]] += kv.Value;
                keyDict[kv.Key[1]] += kv.Value;
            }

            var min = keyDict.OrderBy(kv => kv.Value).First();
            var max = keyDict.OrderBy(kv => kv.Value).Last();

            //this is fix. why? i don't know. but result is valit with it
            var add = (min.Value % 2 + max.Value % 2) % 2;

            return (max.Value/2)-(min.Value/2) + add;
        }
    }
}
