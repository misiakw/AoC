using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Day12
{
    [BasePath("Day12")]
    [TestFile(File = "example.txt", Name = "Example")]
    [TestFile(File = "larger.txt", Name = "Larger")]
    [TestFile(File = "largest.txt", Name = "Largest")]
    [TestFile(File = "Input.txt", Name = "Input")]
    public class Day12 : DayBase
    {
        private CaveFactory factory = new CaveFactory();
        public Day12(string filePath) : base(filePath)
        {
            foreach (var line in LineInput)
            {
                var names = line.Trim().Split("-");
                var A = factory.GetCave(names[0]);
                var B = factory.GetCave(names[1]);
                A.AddCave(B);
                B.AddCave(A);
            }
        }

        [ExpectedResult(TestName = "Example", Result = "10")]
        [ExpectedResult(TestName = "Larger", Result = "19")]
        [ExpectedResult(TestName = "Largest", Result = "226")]
        [ExpectedResult(TestName = "Input", Result = "4659")]
        public override string Part1(string testName)
        {
            BaseCave.TestNum = 1;
            var start = factory.GetCave("start");
            var paths = start.GetPaths(new List<BaseCave>());
            return paths.Count.ToString();
        }

        [ExpectedResult(TestName = "Example", Result = "36")]
        [ExpectedResult(TestName = "Larger", Result = "103")]
        [ExpectedResult(TestName = "Largest", Result = "3509")]
        [ExpectedResult(TestName = "Input", Result = "148962")]
        public override string Part2(string testName)
        {
            BaseCave.TestNum = 2;
            var start = factory.GetCave("start");
            var paths = start.GetPaths(new List<BaseCave>());
            return paths.Count.ToString();
        }

        private class CaveFactory {
            private readonly IDictionary<string, BaseCave> Caves = new Dictionary<string, BaseCave>();
            public BaseCave GetCave(string name)
            {
                if (!Caves.ContainsKey(name))
                    Caves.Add(name, name.Equals(name.ToLower())
                        ? new SmallCave(name)
                        : new BigCave(name));
                return Caves[name];
            }
        }
        private abstract class BaseCave
        {
            public readonly string Name;
            public static int TestNum = 1;
            private readonly IList<BaseCave> Connected = new List<BaseCave>();
            public BaseCave(string name)
            {
                Name = name;
            }
            public void AddCave(BaseCave cave)
            {
                if (!Connected.Contains(cave))
                    Connected.Add(cave);
            }
            public IList<IList<BaseCave>> GetPaths(IList<BaseCave> path)
            {
                path.Add(this);
                var result = new List<IList<BaseCave>>();
                foreach (var cave in Connected.Where(c => c.CanGoInto(path)))
                {
                    var newPath = new List<BaseCave>(path);
                    if(cave.Name == "end")
                    {
                        newPath.Add(cave);
                        result.Add(newPath);
                    }
                    else
                    {
                        var pathsDeeper = cave.GetPaths(newPath);
                        foreach (var deper in pathsDeeper.ToList())
                            result.Add(deper.ToList());
                    }
                }

                return result;
            }
            public abstract bool CanGoInto(IList<BaseCave> path);

            public override string ToString()
            {
                return Name;
            }
        }
        private class SmallCave : BaseCave
        {
            public SmallCave(string name) : base(name) { }

            public override bool CanGoInto(IList<BaseCave> path)
            {
                if (Name == "start" || Name == "end" || TestNum == 1) return !path.Contains(this);

                if (!path.Contains(this)) //nie ma takiego, spokojnie dodaj
                    return true;

                //interesuja nas grupy malych jaskin
                var groups = path.Where(p => p is SmallCave).GroupBy(p => p);

                //jesli jest grupa rozmiaru dwa, to znaczy ze teraz zrobilbym druga, a nie moge
                return !groups.Any(g => g.Count() == 2);
            }
        }
        private class BigCave : BaseCave
        {
            public BigCave(string name) : base(name) { }
            public override bool CanGoInto(IList<BaseCave> path) { return true; }
        }
    }
}
