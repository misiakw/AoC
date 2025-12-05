using System;
using System.Collections.Generic;
using System.Linq;
using AoCBase2;

namespace AoC.Base.TestInputs
{
    public class InputBuilder<TResult, TInput> where TInput : TestInputBase<TResult>
    {
        private readonly DayState<AbstractDay<TResult, TInput>> state;
        public InputBuilder(DayState<AbstractDay<TResult, TInput>> state)
        {
            this.state = state;
        }

        public DayState<AbstractDay<TResult, TInput>> New(string name, string path)
            => state.Test(name, path);
    }
    public static class LegacyWrapper
    {
        public static DayState<T> Part1<T>(this DayState<T> state, long result)
            => state.Part(1).Correct(result.ToString());
        public static DayState<T> Part1<T>(this DayState<T> state)
            => state.Part(1);
        public static DayState<T> Part2<T>(this DayState<T> state, long result)
            => state.Part(2).Correct(result.ToString());
        public static DayState<T> Part2<T>(this DayState<T> state)
            => state.Part(2);
    }
}
