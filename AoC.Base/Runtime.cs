using AoC.Base.TestInputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Base
{
    public interface IRuntime
    {
        void Execute();
    }

    public class Runtime<Result, Tests>: IRuntime where Tests : TestInputs.TestInputBase<Result>
    {
        private readonly Type dayType;
        private readonly Tests[] tests;

        public Runtime(IDay<Result, Tests> day)
        {
            dayType = day.GetType();
            tests = day.GetTests();
        }

        public void Execute()
        {
            foreach(var test in tests)
            {
                Console.WriteLine($"Process Test {test.Name}");
                IDay<Result, Tests> dayObj = (IDay<Result, Tests>)Activator.CreateInstance(dayType);

                if (test.RunPart1)
                    ProcessTest(1, dayObj.Part1, test);
                if (test.RunPart2)
                    ProcessTest(2, dayObj.Part2, test);
            }
        }

        public void ProcessTest(byte num, Func<Tests, Result> testFunc, Tests test)
        {
            Console.WriteLine($"Part {num}");
            var result = testFunc(test);
            var outcome = test.GetOutcome(num - 1, result);
            switch (outcome.Item1)
            {
                case Outcome.TooLow:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"[ { result } ] too low Expected[ { outcome.Item2 } ]");
                    break;
                case Outcome.TooHigh:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"[ { result } ] too high Expected[ { outcome.Item2 } ]");
                    break;
                    break;
                case Outcome.Correct:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"Correct[ { result } ]");
                    break;
                case Outcome.Invalid:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Invalid compare Result[ { result } ] <> Expected[ { outcome.Item2} ] ");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Invalid compare Result[ { result } ]");
                    break;
            }
            Console.WriteLine($" running: ##:##.##.##");
                    Console.ResetColor();


        }
    }
}
