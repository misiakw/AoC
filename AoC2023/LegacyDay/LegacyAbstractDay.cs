using System;
using AoCBase2;

namespace AoC.LegacyBase
{
    public abstract class LegacyAbstractDay
    {
        public void ProceedAoC()
        {
            var dayNum = Int32.Parse(this.GetType().Name.Replace("Day", ""));
            var state = AocRuntime.Day(dayNum, (name, path) =>
            {
                var day = (LegacyAbstractDay)Activator.CreateInstance(GetType());
                return day;
            })
                .Callback(1, (day, test) => day.Part1(test))
                .Callback(2, (day, test) => day.Part2(test));

            PrepateTests(state);

            state.Run();
        }

        public abstract void PrepateTests(DayState<LegacyAbstractDay> dayState);

        public abstract string Part1(TestState input);

        public abstract string Part2(TestState input);
    }
}
