using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AoC_2021
{
    public abstract class DayBase: IDay
    {
        protected string RawInput { private set; get; }
        protected IList<string> LineInput { private set; get; }

        public DayBase(string filePath)
        {
            this.RawInput = File.ReadAllText(filePath);
            LineInput = this.RawInput.Split("\n").Select(l => l.Trim()).ToList();
        }

        public abstract string Part1();

        public abstract string Part2();
    }
}
