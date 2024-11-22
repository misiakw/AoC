using AoC.Base.TestInputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCBase2;

namespace AoC.Base
{
    public abstract class AbstractDay<Result, Tests> where Tests : TestInputBase<Result>
    {
        private Tests wrappedInput;
        public DayStateWrapper<Result, Tests> GetRuntime() => new DayStateWrapper<Result, Tests>(this);

        public abstract void PrepateTests(InputBuilder<Result, Tests> builder);
        public DayState<AbstractDay<Result, Tests>> GetDayState()
        {
            var dayNum = Int32.Parse(this.GetType().Name.Replace("Day", ""));

            var state = AocRuntime.Day(dayNum, (name, path) => {
                var day = (AbstractDay<Result, Tests>)Activator.CreateInstance(GetType());
                day.wrappedInput = (Tests)Activator.CreateInstance(typeof(Tests), new string[2] { name, path });
                return day;
            }).Callback(1, day => day.Part1(day.wrappedInput).ToString())
                .Callback(2, day => day.Part2(day.wrappedInput).ToString());

            var builder = new InputBuilder<Result, Tests>(state);
            PrepateTests(builder);
            return state;
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
    public class DayStateWrapper<Result, Tests> where Tests : TestInputs.TestInputBase<Result>
    {
        DayState<AbstractDay<Result, Tests>> dayState;
        public DayStateWrapper(AbstractDay<Result, Tests> day)
        {
            dayState = day.GetDayState();
        }
        public void Execute() => dayState.Run();

    }
}
