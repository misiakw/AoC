using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day6
{
    [BasePath("Day6")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day6 : DayBase
    {

        public Day6(string filePath) : base(filePath)
        {
            Fish.FeedDict(256);
        }

        [ExpectedResult(TestName = "Example", Result = "5934")]
        [ExpectedResult(TestName = "Input", Result = "358214")]
        public override string Part1()
        {
            var days = 80;
            
            var groups = this.RawInput.Trim().Split(",").Select(s =>int.Parse(s)).GroupBy(l => l);
            var size = 0L;

            foreach(var group in groups)
            {
                var fish = Fish.GetFish(group.Key, days);
                size += fish.FamilySize * group.Count();
            }
            return size.ToString();

        }

        [ExpectedResult(TestName = "Example", Result = "26984457539")]
        [ExpectedResult(TestName = "Input", Result = "1622533344325")]
        public override string Part2()
        {
            var days = 256;

            var groups = this.RawInput.Trim().Split(",").Select(s => int.Parse(s)).GroupBy(l => l);
            var size = 0L;

            foreach (var group in groups)
            {
                var fish = Fish.GetFish(group.Key, days);
                size += fish.FamilySize * group.Count();
            }
            return size.ToString();
        }

        private class Fish
        {
            public static IDictionary<int, List<int>> BREED_DICT = new Dictionary<int, List<int>>();
            public static IDictionary<int, Fish> FISH_DICT = new Dictionary<int, Fish>();
            public readonly IList<Fish> Family = new List<Fish>();

            public static Fish GetFish(int state, int daysToBreed)
            {
                daysToBreed = daysToBreed - state;

                if (daysToBreed < 0)
                    return new Fish(0, daysToBreed);

                if (!FISH_DICT.ContainsKey(daysToBreed))
                    FISH_DICT.Add(daysToBreed, new Fish(0, daysToBreed));

                return FISH_DICT[daysToBreed];
            }

            private Fish(int state, int daysToBreed)
            {
                daysToBreed = daysToBreed - state;

                if (daysToBreed < 0)
                    return;

                var breeds = BREED_DICT[daysToBreed];

                foreach (var date in breeds)
                {
                    Family.Add(Fish.GetFish(8, date));
                }
            }

            private long _familySize = -1;
            public long FamilySize
            {
                get
                {
                    if (_familySize < 0)
                        _familySize =  Family.Select(f => f.FamilySize).Sum() + 1;
                    return _familySize;
                }
            }

            public static void FeedDict(int size)
            {
                var start = BREED_DICT.Any()
                ? BREED_DICT.Keys.Max() + 1
                : 0;
                for (var i = start; i <= size; i++)
                {
                    BREED_DICT.Add(i, CalculateBreed(i).ToList());
                }
            }
            private static  IEnumerable<int> CalculateBreed(int days)
            {
                var state = 0;
                while (days > 0)
                {
                    days -= 1;
                    state -= 1;
                    if (state < 0)
                    {
                        yield return days;
                        state = 6;
                    }
                }
            }
        }
    }
}
