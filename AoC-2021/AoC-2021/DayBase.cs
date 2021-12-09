using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AoC_2021
{
    public abstract class DayBase: IDay
    {
        protected string RawInput { private set; get; }
        protected IReadOnlyList<string> LineInput { private set; get; }
        protected readonly string InputDir = string.Empty;

        public DayBase(string filePath)
        {
            this.InputDir = Path.GetDirectoryName(filePath);
            this.RawInput = File.ReadAllText(filePath);
            LineInput = this.RawInput.Split("\n").Select(l => l.Trim()).ToList();
        }

        public abstract string Part1(string testName);

        public abstract string Part2(string testName);
    }
}
