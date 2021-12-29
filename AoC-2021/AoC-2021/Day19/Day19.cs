using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day19
{
    [BasePath("Day19")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day19 : DayBase
    {
        private IReadOnlyDictionary<string, IList<Beacon>> Input;
        public Day19(string filePath) : base(filePath)
        {
            var input = LineInput.GetEnumerator();
            var dict = new Dictionary<string, IList<Beacon>>();

            while (input.MoveNext())
            {
                var key = input.Current.Replace("scanner", "").Replace("---", "").Trim();
                string line;
                var beacons = new List<Beacon>();

                while(input.MoveNext() && (line = input.Current.Trim()) != string.Empty)
                    beacons.Add(new Beacon(line.Split(",").Select(l => long.Parse(l)).ToArray()));

                foreach(var beacon in beacons)
                    foreach (var other in beacons.Where(b => b != beacon))
                        beacon.AddRelated(other);

                dict.Add(key, beacons);
            }
            Input = dict;
        }

        [ExpectedResult(TestName = "Example", Result = "79")]
        //[ExpectedResult(TestName = "Input", Result = 571032)]
        public override string Part1(string testName)
        {
            var commons = Input["0"].ToList();

            throw new NotImplementedException();
        }

        public override string Part2(string testName)
        {
            throw new NotImplementedException();
        }

        private class Beacon
        {
            public long[] Pos;
            private IDictionary<long, IList<Beacon>> NearOnes = new Dictionary<long, IList<Beacon>>();
            public Beacon(long[] pos)
            {
                Pos = pos;
            }

            public IEnumerable<long> dif(Beacon beacon)
            {
                for (var i = 0; i < 3; i++)
                    yield return Pos[i] - beacon.Pos[i];
            }

            public void AddRelated(Beacon beacon)
            {
                var key = dif(beacon).Select(d => Math.Abs(d)).Sum();
                if (!NearOnes.ContainsKey(key)) NearOnes.Add(key, new List<Beacon>());
                NearOnes[key].Add(beacon);
            }
        }
    }
}
