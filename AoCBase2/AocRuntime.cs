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

        public static void Run<T>(this DayState<T> state) => Run<T>(state, null, null);
        public static void Run<T>(this DayState<T> state, byte part) => Run<T>(state, part, null);
        public static void Run<T>(this DayState<T> state, string testName) => Run<T>(state, null, testName);
        public static void Run<T>(this DayState<T> state, byte? part = null, string testName = null)
        {
            state.Save(); //persist config
            var testsToRun = string.IsNullOrEmpty(testName)
                ? state.dto.tests
                : state.dto.tests.Where(t => t.name == testName);

            var results = new List<(string, (string, bool?, string), (string, bool?, string))>();
            var watch = new Stopwatch();

            foreach (var test in testsToRun.Where(t => t.run))
            {
                (string, (string, bool ?, string), (string, bool?, string)) result = (test.name, ("n/a", null, null), ("n/a", null, null));
                T day = state.setupFunc.Invoke(test.name, test.testFile.PathToRelativeToSolution());
                string[] outputs = { null, null };
                bool?[] corrects = {null, null };
                string[] durations = { null, null };
                for(int t=0; t< outputs.Length; t++)
                {
                    var shouldRun = test.result[t] != null && test.result[t].run;
                    if ((state.callback[t] == null) //no callback
                        || (part.HasValue && part.Value != t) //skipped pasrt as param
                        || (test.result[t] != null && !test.result[t].run)) //skipped test execution
                        continue;
                    watch.Start();
                    var task = state.callback[t].needSetup
                        ? state.callback[t].callback.Invoke(state.setupFunc.Invoke(test.name, test.testFile.PathToRelativeToSolution()), test)
                        : state.callback[t].callback.Invoke(day, test);
                    task.Wait();
                    outputs[t] = task.Result;
                    watch.Stop();

                    if (task.IsFaulted)
                        throw task.Exception;

                    durations[t] = watch.ToString();
                    if (test.result[t] == null)
                        test.result[t] = new TestResult();
                    corrects[t] = test.result[t].ProcessResult(outputs[t], $"{test.name} Part{(t + 1)}");
                    if (state.isDirty)
                        state.Save();
                }

                results.Add((test.name, (outputs[0] ?? "n/a", corrects[0], durations[0]), (outputs[1] ?? "n/a", corrects[1], durations[1])));
            }

            //display outputs
            Console.WriteLine("\n"+"name".PadRight(15) + " | " + "answer part 1".PadRight(15) + " | "+"durarion".PadRight(16)+" | " + "answer part 2".PadRight(15) + " | "+"durarion".PadRight(16));
            Console.WriteLine("".PadRight(16, '-') + "+" + "".PadRight(17, '-') + "+" + "".PadRight(18, '-') + "+" + "".PadRight(17, '-') + "+" + "".PadRight(18, '-'));
            foreach (var result  in results)
            {
                Console.Write(result.Item1.PadRight(15) + " | ");
                if (result.Item2.Item2.HasValue)
                    Console.ForegroundColor = result.Item2.Item2.Value ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write((result.Item2.Item1).PadRight(15));
                Console.ResetColor();
                Console.Write(" | "+ result.Item2.Item3.PadRight(16) + " | ");
                if (result.Item3.Item2.HasValue)
                    Console.ForegroundColor = result.Item3.Item2.Value ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write((result.Item3.Item1).PadRight(15));
                Console.ResetColor();
                Console.WriteLine(" | " + result.Item3.Item3.PadRight(16));
            } 
        }

        public static DayState<T> Test<T>(this DayState<T> state, string name, string filePath)
        {
            EnsureFileExist(filePath.PathToRelativeToSolution());
            TestState selectedTest = state.dto.tests.FirstOrDefault(t => t.name == name);
            if (selectedTest == null)
            {
                selectedTest = new TestState()
                {
                    name = name,
                    testFile = filePath
                };
                state.dto.tests.Add(selectedTest);
            }
            if (state.selectedTest != null)
                state.selectedTest.selectedResult = null;
            state.selectedTest = selectedTest;
            state.selectedTest.selectedResult = null;
            return state;
        }
        public static DayState<T> Part<T>(this DayState<T> state, byte part)
        {
            EnsureProperTaskPart(part);
            if (state.selectedTest == null) throw new InvalidDataException("Test not selected");
            if (state.selectedTest.result[part - 1] == null)
                state.selectedTest.result[part - 1] = new TestResult();
            state.selectedTest.selectedResult = state.selectedTest.result[part - 1];
            return state;
        }
        public static DayState<T> Correct<T>(this DayState<T> state, string result)
        {
            if (state.selectedTest == null) throw new InvalidDataException("Test not selected");
            if (state.selectedTest.selectedResult == null) throw new InvalidDataException("Result not selected");
            if (state.selectedTest.selectedResult.correct != result)
            {
                state.selectedTest.selectedResult.correct = result;
                state.selectedTest.selectedResult.incorrect.Clear();
            }
            return state;
        }
        public static DayState<T> Correct<T>(this DayState<T> state, long result) => state.Correct(result.ToString());
        public static DayState<T> Skip<T>(this DayState<T> state)
        {
            if (state.selectedTest == null) throw new InvalidDataException("Test not selected");
            if (state.selectedTest.selectedResult == null) 
                state.selectedTest.run = false;
            else
                state.selectedTest.selectedResult.run = false;
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
            if (dto?.correct != null)
                return dto.correct == result;
            if (dto.incorrect.Contains(result.ToString()))
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
