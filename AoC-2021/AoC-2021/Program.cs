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
            var prms = new Params(args);
            var dayNumber = ((BasePathAttribute)(prms.DayType).GetCustomAttribute(typeof(BasePathAttribute))).Path;
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dayNumber);

            Console.WriteLine($"----- {dayNumber} -----");
            foreach (TestFileAttribute testFile in prms.DayType.GetCustomAttributes(typeof(TestFileAttribute)))
            {
                Console.WriteLine($"--- {testFile.Name} ---");

                IDay day = (IDay)Activator.CreateInstance(prms.DayType, Path.Combine(path, testFile.File));

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
                    Console.WriteLine(success
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

        public static Nullable<T> ToNullable<T> (this string value) where T: struct, IConvertible
        {
            return value != null
                ? (T?) Convert.ChangeType(value, typeof(T))
                : (T?) null;
        }
    }
}
