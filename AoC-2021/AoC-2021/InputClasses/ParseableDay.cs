using System.Collections.Generic;
using System.Linq;

namespace AoC_2021.InputClasses
{
    public abstract class ParseableDay<T>: DayBase, IDay
    {
        protected IReadOnlyList<T> Input { private set; get; }
        public ParseableDay(string path): base(path)
        {
            this.Input = this.LineInput.Select(l => Parse(l)).ToList();
        }

        public abstract T Parse(string input);
    }
}
