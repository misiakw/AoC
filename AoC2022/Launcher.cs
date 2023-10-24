using AoC.Base.TestInputs;
using System;
using System.Linq;
using System.Reflection;

namespace AoC.Base
{
    public static class Launcher
    {
        private static void Execute<R, T>(this IDay<R, T> day) where T : TestInputBase<R>
        {
            var tests = day.GetTests();

            Console.WriteLine($"Running {day.GetType().Name}. Total Tests: {tests.Length}");

            foreach(var test in tests.Where(t => t.RunPart1))
            {
                var result = day.Part1(test);
                var outcome = test.GetOutcome(1, result);

                Console.WriteLine($"Test {test.Name} Result: {result} Outcome: {outcome}");
            }
        }
    }
}
