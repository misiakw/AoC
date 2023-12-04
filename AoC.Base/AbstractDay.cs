using AoC.Base.TestInputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Base
{
    public abstract class AbstractDay<Result, Tests> : IDay<Result, Tests> where Tests : TestInputBase<Result>
    {
        public IRuntime GetRuntime() => new Runtime<Result, Tests>(this);

        public abstract void PrepateTests(InputBuilder<Result, Tests> builder);

        public Tests[] GetTests()
        {
            var builder = new InputBuilder<Result, Tests>();
            PrepateTests(builder);
            return builder.Build();
        }

        public abstract Result Part1(Tests input);

        public abstract Result Part2(Tests input);

        protected IList<string> ReadLines(Tests input)
        {
            var t = input.ReadLines();
            t.Wait();
            return t.Result.ToList();
        }
    }
}
