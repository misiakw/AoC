using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoCBase2
{
    public static class AocRuntime
    {
        public static DayState<T> Day<T>(int dayNum, Func<string, string, T> setupFunc)
        {
            var filename = Path.Combine("data", $"day{dayNum}.json").PathToRelativeToSolution();

            DayState<T> result = null;
            if (File.Exists(filename))
                result = DayState<T>.Load(filename);
            else
            {
                EnsureFileExist(filename);
                result = new DayState<T>(filename, dayNum);
            }
            result.setupFunc = setupFunc;
            return result;
        }
        public static DayState<T> Day<T>(int dayNum) where T: new()
            => Day<T>(dayNum, (name, input) => new T());

        public static void Run<T>(this DayState<T> state)
        {
            state.context = null;
            state.Save(); //persist config

            var watch = new Stopwatch();
            var tab = new PrintTable(new List<string>() { "name", "Part 1", "duration", "Part 2", "duration" });

            foreach (var test in state.Tests)
            {
                var row = tab.Row(test.debug ? ConsoleColor.Magenta : ConsoleColor.White).Cell(test.name);
                if (!test.run)
                {
                    row.Cell("").Cell("").Cell("").Cell("");
                    continue;
                }
                T day = state.setupFunc.Invoke(test.name, test.testFile.PathToRelativeToSolution());
                for(int t=0; t< 2; t++)
                {
                    if ((state.callback[t] == null || !state.callback[t].run) //skip callback
                        || (test.result[t] != null && !test.result[t].run)) //skipped test execution
                    {
                        row.Cell("").Cell("");
                        continue;
                    }

                    string output = string.Empty;
                    long[] timings = new long[test.result[t]?.repeats ?? 1];

                    for (var i = 0; i < timings.Length; i++)
                    {
                        watch.Start();
                        var task = state.callback[t].needSetup
                            ? state.callback[t].callback.Invoke(state.setupFunc.Invoke(test.name, test.testFile.PathToRelativeToSolution()), test)
                            : state.callback[t].callback.Invoke(day, test);
                        task.Wait();
                        output = task.Result;
                        watch.Stop();
                        timings[i] = watch.ElapsedTicks;

                        if (task.IsFaulted)
                            throw task.Exception;
                    }

                    var timespan = new TimeSpan(timings.Sum()/timings.Length);

                    var testResult = test.result[t] != null
                        ? test.result[t]
                        : new TestResult() { test = test, isDirty = true };
                    if (test.debug)
                    {
                        Console.WriteLine($"file '{test.name}' Part{(t + 1)} result: {output}");
                        row.Cell(output).Cell(timespan.ToString());
                    }
                    else
                    {
                        var correct = testResult.ProcessResult(output, $"file '{test.name}' Part{(t + 1)}");
                        if (correct.HasValue && testResult.isDirty)
                            test.result[t] = testResult;
                        if (state.isDirty)
                            state.Save();
                        row.Cell(output, correct.HasValue ? correct.Value ? ConsoleColor.Green : ConsoleColor.Red : ConsoleColor.White)
                        .Cell(timings.Length > 1 ? $"avg {watch}": watch.ToString());
                    }
                }
            }
            tab.PrintConsole();
        }

        public static DayState<T> Test<T>(this DayState<T> state, string name, bool debug = false) =>
            state.Test(name, $"Inputs/Day{state.dto.dayNum}/{name}.txt", debug);
        public static DayState<T> Test<T>(this DayState<T> state, string name, string filePath, bool debug = false)
        {
            EnsureFileExist(filePath.PathToRelativeToSolution());
            TestState selectedTest = !debug            
                ? state.dto.tests.FirstOrDefault(t => t.name == name)
                : state.DebugTests.FirstOrDefault(t => t.name == name);
            if (selectedTest == null)
            {
                selectedTest = new TestState()
                {
                    name = name,
                    testFile = filePath,
                    debug = debug
                };
                (debug? state.DebugTests: state.dto.tests).Add(selectedTest);
            }
            else
                selectedTest.debug = debug;
            foreach(var result in selectedTest.result.Where(r => r != null))
                result.test = selectedTest;
            state.context = selectedTest;
            return state;
        }
        public static DayState<T> Part<T>(this DayState<T> state, byte part)
        {
            EnsureProperTaskPart(part);
            var selectedTest = state.context as TestState;
            if (selectedTest == null) throw new InvalidDataException("Test not selected");
            state.context = selectedTest.result[part - 1];
            return state;
        }
        public static DayState<T> Correct<T>(this DayState<T> state, string result)
        {
            var selectedResult = state.context as TestResult;
            if(selectedResult == null) throw new InvalidDataException("Result not selected");
            if (selectedResult.correct != result && result != null)
            {
                selectedResult.correct = result;
                selectedResult.incorrect.Clear();
            }
            return state;
        }
        public static DayState<T> Correct<T>(this DayState<T> state, long result) => state.Correct(result.ToString());
        public static DayState<T> Skip<T>(this DayState<T> state)
        {
            if (state.context is TestResult result) result.run = false;
            else if (state.context is TestState test) test.run = false;
            else if(state.context is Callback<T> callback) callback.run = false;
            else throw new InvalidDataException("Invalid Skip Context");
            return state;
        }
        public static DayState<T> Performance<T>(this DayState<T> state, int repeats = 5)
        {
            if (state.context is TestResult result) result.repeats = repeats;
            else throw new InvalidDataException("Invalid Performance Context");
            return state;
        }
        public static DayState<T> Drop<T>(this DayState<T> state)
        {

            if (state.context is TestResult result)
            {
                state.context = result.test;
                for (var i = 0; i < 2; i++)
                    if (result.test.result[i] == result) result.test.result[i] = null;
            }
            else if (state.context is TestState test)
            {
                state.context = null;
                state.dto.tests.Remove(test);
            }
            else throw new InvalidDataException("Invalid Drop Context");
            return state;
        }
        public static DayState<T> Callback<T>(this DayState<T> state, byte part, Func<T, TestState, Task<string>> callback, bool needSetup = false)
        {
            EnsureProperTaskPart(part);
            state.callback[part - 1] = new Callback<T>()
            {
                callback = callback,
                needSetup = needSetup
            };

            state.context = state.callback[part - 1];
            return state;
        }
        public static DayState<T> Callback<T>(this DayState<T> state, byte part, Func<T, TestState, string> callback, bool needSetup = false)
            => state.Callback(part, new Func<T, TestState, Task<string>>((x, test) => {
                var t = new Task<string>(() => callback(x, test));
                t.Start();
                return t; 
            }), needSetup);

        private static void EnsureProperTaskPart(byte part)
        {
            if (part != 1 && part != 2)
                throw new InvalidDataException($"could not process part {part} - valid values are 1 or 2");
        }

        private static bool? ProcessResult(this TestResult dto, string result, string label)
        {
            if (dto == null || result == null)
                return null;
            if (dto?.correct != null)
                return dto.correct == result;
            if (dto.incorrect != null && dto.incorrect.Contains(result.ToString()))
                return false;

            Console.WriteLine($"{label} result: {result}");
            var correct = readYesNo();

            if (!correct.HasValue) //skip
                return null;


            dto.isDirty = true;
            if (correct.Value)
            {
                dto.correct = result;
                dto.incorrect.Clear();
            }
            else
                dto.incorrect.Add(result.ToString());
            return correct.Value;
        }


        private static bool? readYesNo()
        {
            char choice;
            do
            {
                Console.Write("\rIs output correct (y/n/s):  "); //double to owerwrite previous insert
                Console.Write("\rIs output correct (y/n/s): ");
                choice = Console.ReadKey().KeyChar;
                choice = choice switch
                {
                    'S' => 's',
                    'Y' => 'y',
                    'N' => 'n',
                    _ => choice
                };
            } while (choice != 'y' && choice != 'n' && choice != 's');
            Console.WriteLine();
            return choice == 's' ? null : choice == 'y';
        }
        private static void EnsureFileExist(string path)
        {
            if (!File.Exists(path))
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.Create(path).Close();
            }
        }
        internal static string PathToRelativeToSolution(this string path)
        {
            var dir = Path.GetDirectoryName(path);

            while (!File.Exists(Path.Combine(dir, "Program.cs")))
            {
                dir = Directory.GetParent(dir).FullName;
            }

            return Path.Combine(dir, path);
        }
    }
}
