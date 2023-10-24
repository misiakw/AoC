using AoC.Base.TestInputs;

namespace AoC.Base
{
    public interface IDay<Result, Tests> where Tests: TestInputBase<Result>
    {
        public Tests[] GetTests();
        public Result Part1(Tests input);
        public Result Part2(Tests input);
        public IRuntime GetRuntime();
    }
}
