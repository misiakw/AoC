using AoC_2021.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AoC_2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var testType = typeof(Day2.Day2);

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ((BasePathAttribute)testType.GetCustomAttribute(typeof(BasePathAttribute))).Path);

            foreach(TestFileAttribute testFile in testType.GetCustomAttributes(typeof(TestFileAttribute)))
            {
                Console.WriteLine($"--- {testFile.Name} ---");

                IDay day = (IDay)Activator.CreateInstance(testType, Path.Combine(path, testFile.File));

                TryRun("Part1", day.Part1, testFile.Name);
                TryRun("Part2", day.Part2, testFile.Name);
            } 
        }

        private static void TryRun(string label, Func<string> action, string testName)
        {
            var expected = action.Method.GetCustomAttributes<ExpectedResultAttribute>().FirstOrDefault(a => a.TestName.Equals(testName, StringComparison.InvariantCultureIgnoreCase));

            try
            {
                if (expected == null)
                {
                    Console.WriteLine($"\t{label}: [[ {action()} ]]");
                }
                else
                {
                    var result = action();
                    var success = result.Equals(expected.Result, StringComparison.InvariantCultureIgnoreCase);
                    Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.Write($"\t{label}:");
                    Console.WriteLine( success
                        ? $" [[ {result} ]]"
                        : $" {result} != {expected.Result}");
                    Console.ResetColor();
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\tFAILED:");
                Console.ResetColor();
                Console.WriteLine($" {label}");
            }
        }
    }
}
