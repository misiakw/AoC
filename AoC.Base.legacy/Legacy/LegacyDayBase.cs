using AoC.Base.TestInputs;
using AoCBase2;
using System;
using System.Collections.Generic;
using System.Linq;
using TestType = AoC.Base.LegacyInput.TestType;

namespace AoC.Base
{
    public abstract partial class LegacyDayBase
    {
        private readonly int dayNum;
        protected IList<LegacyInput> tests = new List<LegacyInput>();

        public LegacyDayBase(int dayNum)
        {
            this.dayNum = dayNum;
        }
        public abstract object Part1(LegacyInput input);
        public abstract object Part2(LegacyInput input);

        public virtual void Execute(){           
            ProcessTest(0, Part1);
            ProcessTest(1, Part2);
        }

        private void ProcessTest(byte testNum, Func<LegacyInput, object> testFunc)
        {
            Console.WriteLine($"==== Part {testNum + 1} ====");
            foreach (var test in tests.Where(t => t.Tests[testNum] != TestType.Skip))
            {
                try
                {

                    Console.WriteLine($"\t{test.Name}");

                    var start = DateTime.Now;
                    var resultObj = testFunc(test);
                    var stop = DateTime.Now;
                    var span = new TimeSpan(stop.Ticks - start.Ticks);
                    var timeStr = $"{span.Hours}:{span.Minutes}:{span.Seconds}.{span.Milliseconds}";

                    if (test.Tests[testNum] == TestType.Silent)
                        continue;

                    var desiredResult = test.Result[testNum];

                    if (resultObj != null)
                    {
                        if (desiredResult?.Item1 == null)
                        {
                            Console.WriteLine($"Result[ {resultObj} ] running: {timeStr}");
                        }
                        else if (resultObj.GetType() != desiredResult.Item2)
                        {
                            Console.WriteLine($"Result[ {resultObj} ] Expected[ {desiredResult.Item1} ] running: {timeStr}");
                        }
                        else
                        {
                            if (resultObj.GetType().GetInterface("IComparable") != null)
                            {
                                VerifyComparable((IComparable)resultObj, desiredResult.Item1, timeStr);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exeption: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    continue;
                }
            }
        }

        private void VerifyComparable(IComparable compResult, object desired, string timeStr)
        {
            switch (compResult.CompareTo(desired))
            {
                case -1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ {compResult} ] too low Expected[ { desired} ] running: {timeStr}");
                    Console.ResetColor();
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ {compResult} ] too High Expected[ { desired} ] running: {timeStr}");
                    Console.ResetColor();
                    break;
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Correct[ {compResult} ] running: {timeStr}");
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Invalid compare Result[ {compResult} ] <> Expected[ { desired} ] running: {timeStr}");
                    Console.ResetColor();
                    break;
            }
        }

        public DayState<AbstractDay<object, LegacyInput>> GetDayState()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class LegacyDay<T> : LegacyDayBase
    {
        protected LegacyDay(int dayNum) : base(dayNum)
        {
        }

        public abstract object Part1(IList<T> data, LegacyInput input);
        public abstract object Part2(IList<T> data, LegacyInput input);
        public abstract IList<string> Split(string val);
        public abstract T Parse(string val);
        public override object Part1(LegacyInput input) => Part1((IList<T>)input.Prepare<T>(Split, Parse), input);

        public override object Part2(LegacyInput input) => Part2((IList<T>)input.Prepare<T>(Split, Parse), input);
    }
}
