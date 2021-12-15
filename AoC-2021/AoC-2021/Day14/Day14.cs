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
            var rules = new Dictionary<string, string>();
            foreach (var line in LineInput.Skip(2))
            {
                var parts = line.Trim().Split("->");
                rules.Add(parts[0].Trim(), parts[1].Trim());
            }
            foreach (var rule in rules)
            {
                var resultPair = new string[2];
                resultPair[0] = $"{rule.Key[0]}{rule.Value}";
                resultPair[1] = $"{rule.Value}{rule.Key[1]}";
                Insertions.Add(rule.Key, resultPair);
            }
        }

        private IDictionary<string, string[]> Insertions = new Dictionary<string, string[]>();

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
            IDictionary<string, long> Polymer = new Dictionary<string, long>();

            //define Polymer parts
            for (var i = 0; i < LineInput[0].Length - 1; i++)
            {
                var key = LineInput[0].Substring(i, 2);
                if (!Polymer.ContainsKey(key))
                {
                    Polymer.Add(key, 0);
                }
                Polymer[key]++;
            }

            //process polymer creation
            while (steps-- > 0)
            {
                var newPolymer = new Dictionary<string, long>();
                foreach (var cSet in Polymer)
                {
                    var amount = cSet.Value;
                    var newOnes = Insertions[cSet.Key];

                    newPolymer.TryAdd(newOnes[0], 0);
                    newPolymer.TryAdd(newOnes[1], 0);
                    newPolymer[newOnes[0]] += cSet.Value;
                    newPolymer[newOnes[1]] += cSet.Value;
                }
                Polymer = newPolymer;
            }

            //calculate polymer parts occurience
            var keyDict = new Dictionary<char, long>();
            foreach(var kv in Polymer)
            {
                keyDict.TryAdd(kv.Key[0], 0);
                keyDict.TryAdd(kv.Key[1], 0);
                keyDict[kv.Key[0]] += kv.Value;
                keyDict[kv.Key[1]] += kv.Value;
            }

            //get interestign polymer pars
            var minSet = keyDict.OrderBy(kv => kv.Value).First();
            var maxSet = keyDict.OrderBy(kv => kv.Value).Last();

            /* Right now we have amount of polymer parts duplicated, because pairs overlaps 
             * (end of one counts also as begin of other, despite it's the same part) the only 
             * ones that are odd are those from start or end (edge) of polymer this means 
             * that we need to consider them as duplicated except the one at edge of polymer
             */
            var min = (minSet.Value % 2 == 0)
                ? minSet.Value / 2
                : (minSet.Value - 1) / 2 + 1; 
            var max = (maxSet.Value % 2 == 0)
                 ? maxSet.Value / 2
                 : (maxSet.Value - 1) / 2 + 1;


            return max-min;
        }
    }
}
