using AoC_2021.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AoC_2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var prms = new Params(args);
            var dayNumber = ((BasePathAttribute)(prms.DayType).GetCustomAttribute(typeof(BasePathAttribute)))?.Path ?? prms.DayType.Name;
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dayNumber);

            Console.WriteLine($"----- {dayNumber} -----");
            var dayTests1 = new List<Tuple<IDay, TestFileAttribute>>();
            var dayTests2 = new List<Tuple<IDay, TestFileAttribute>>();
            foreach (TestFileAttribute testFile in prms.DayType.GetCustomAttributes(typeof(TestFileAttribute)))
            {
                IDay day = (IDay)Activator.CreateInstance(prms.DayType, Path.Combine(path, testFile.File));
                if (testFile.TestToProceed == TestCase.All || testFile.TestToProceed == TestCase.Part1)
                    dayTests1.Add(Tuple.Create(day, testFile));
                if (testFile.TestToProceed == TestCase.All || testFile.TestToProceed == TestCase.Part2)
                    dayTests2.Add(Tuple.Create(day, testFile));
            }

            Console.WriteLine("--- Part1 ---");
            foreach (var tuple in dayTests1)
            {
                TryRun(tuple.Item2.Name, tuple.Item1.Part1, tuple.Item2.Name);
            }
            Console.WriteLine("--- Part2 ---");
            foreach (var tuple in dayTests2)
            {
                TryRun(tuple.Item2.Name, tuple.Item1.Part2, tuple.Item2.Name);
            }
        }

        private static void TryRun(string label, Func<string, string> action, string testName)
        {
            var expected = action.Method.GetCustomAttributes<ExpectedResultAttribute>().FirstOrDefault(a => a.TestName.Equals(testName, StringComparison.InvariantCultureIgnoreCase));

            try
            {
                var start = DateTime.Now;
                var result = action(testName);
                var stop = DateTime.Now;
                var span = new TimeSpan(stop.Ticks - start.Ticks);

                if (string.IsNullOrEmpty(expected?.Result))
                {
                    Console.Write($"\t");
                    var numResult = 0L;
                    if (expected != null && long.TryParse(result, out numResult))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (numResult >= (expected?.TooHigh ?? long.MaxValue))
                            { Console.Write($"Too high: "); }
                        else if (numResult <= (expected?.TooLow ?? long.MinValue))
                            { Console.Write($"Too Low: "); }
                    }

                    Console.WriteLine($"{label}: [[ {result} ]] running: {span.Minutes}:{span.Seconds}.{span.Milliseconds}");
                    Console.ResetColor();
                }
                else
                {
                    var success = result.Equals(expected.Result, StringComparison.InvariantCultureIgnoreCase);
                    Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.Write($"\t{label}:");
                    Console.Write(success
                        ? $" [[ {result} ]]"
                        : $" {result} != {expected.Result}");
                    Console.WriteLine($" running: {span.Minutes}:{span.Seconds}.{span.Milliseconds}");
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\tFAILED:");
                Console.ResetColor();
                Console.WriteLine($" {label}");
                if (e is not NotImplementedException)
                {
                    Console.WriteLine(e);
                }
            }
            finally
            {
                Console.ResetColor();
            }
        }


        private class Params
        {
            private int? _dayOfMonth;
            private Type _dayType = null;
            public Type DayType
            {
                get
                {
                    if (this._dayType != null)
                        return _dayType;
                    var day = this._dayOfMonth ?? DateTime.Now.Day;
                    Type result = null;
                    do
                    {
                        result = Type.GetType($"AoC_2021.Day{day}.Day{day}");
                        if (result == null)
                            day--;

                    } while (day > 0 && result == null);
                    this._dayType = result;
                    return result;
                }
            }

            public Params(string[] args)
            {
                this._dayOfMonth = args.GetKey("day").ToNullable<int>();
            }
        }
    }

    public static class Extensions
    {
        public static string GetKey(this string[] args, string key)
        {
            return args.Where(a => a.StartsWith($"-{key}")).Select(a => a.Trim().Split("=")[1]).FirstOrDefault();
        }

        public static Nullable<T> ToNullable<T>(this string value) where T : struct, IConvertible
        {
            return value != null
                ? (T?)Convert.ChangeType(value, typeof(T))
                : (T?)null;
        }
    }
}
