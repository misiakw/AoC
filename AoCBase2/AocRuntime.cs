using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoCBase2
{
    public static class AocRuntime
    {
        public static DayState<T> Day<T>(int dayNum, Func<string, string, T> setupFunc)
        {
            var filename = Path.Combine("data", $"day{dayNum}.json").PathToRelative();

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

            Console.WriteLine("name".PadRight(15) + " | " + "answer part 1".PadRight(15) + " | " + "answer part 2");
            Console.WriteLine("".PadRight(16, '-') + "+" + "".PadRight(17, '-') + "+" + "".PadRight(17, '-'));
            foreach (var test in testsToRun)
            {
                Console.Write(test.name.PadRight(15) + " | ");
                string[] result = new string[2] { "n/a", "n/a" };
                T day = state.setupFunc.Invoke(test.name, test.testFile.PathToRelative());
                for (int t = 0; t < 2; t++)
                {
                    if ((state.callback[t] == null) ||
                        (part.HasValue && part.Value != t) ||
                        (test.result[t] == null || !test.result[t].run))
                    {
                        Console.Write(result[t].PadRight(15) + " | ");
                        continue;
                    }

                    var task = state.callback[t].needSetup
                        ? state.callback[t].callback.Invoke(state.setupFunc.Invoke(test.name, test.testFile.PathToRelative()), test)
                        : state.callback[t].callback.Invoke(day, test);
                    task.Wait();
                    result[t] = task.Result;
                    if(test.result[t].ProcessResult(result[t])) //result stored data
                        state.Save();
                }
                Console.WriteLine();
            } 
        }

        public static DayState<T> Test<T>(this DayState<T> state, string name, string filePath)
        {
            EnsureFileExist(filePath.PathToRelative());
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
            state.selectedTest = selectedTest;
            return state;
        }
        public static DayState<T> Part<T>(this DayState<T> state, byte part)
        {
            EnsureProperTaskPart(part);
            if (state.selectedTest == null) throw new InvalidDataException("Test not selected");
            if (state.selectedTest.result[part - 1] == null)
                state.selectedTest.result[part - 1] = new TestResultDTO();
            state.selectedTest.selectedResult = state.selectedTest.result[part - 1];
            state.selectedTest.selectedResult.run = true;
            return state;
        }
        public static DayState<T> Correct<T>(this DayState<T> state, long result) => Correct(state, result.ToString());
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
        public static DayState<T> Debug<T>(this DayState<T> state)
        {
            if (state.selectedTest == null) throw new InvalidDataException("Test not selected");
            if (state.selectedTest.selectedResult == null) throw new InvalidDataException("Result not selected");
            state.selectedTest.selectedResult.isDebug = true;
            return state;
        }
        public static DayState<T> Callback<T>(this DayState<T> state, byte part, Func<T, TestState, Task<string>> callback, bool needSetup = false)
        {
            EnsureProperTaskPart(part);
            state.callback[part - 1] = new Callback<T>()
            {
                callback =  callback,
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
        private static bool ProcessResult(this TestResultDTO dto, string result)
        {
            if (!string.IsNullOrEmpty(dto?.correct)) // correct or invalid result
            {
                
                Console.ForegroundColor = (result == dto.correct)
                    ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write((result ?? "").PadRight(15));
                Console.ResetColor();
                Console.Write(" | ");

                return false;
            }
            if (dto.incorrect.Contains(result))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write((result ?? "").PadRight(15));
                Console.ResetColor();
                Console.Write(" | ");
                return false;
            }

            //unknown result
            Console.Write(result.PadRight(15) + " | ");
            int cursorPos = Console.CursorLeft;
            Console.Write("\nIs output correct (y/n/s): ");
            var correct = readYesNo();
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("".PadRight(30));

            if (!correct.HasValue) //skip
            {
                Console.SetCursorPosition(cursorPos, Console.CursorTop - 1);
                return false;
            }

            Console.SetCursorPosition(cursorPos - 18, Console.CursorTop - 1);
            Console.ForegroundColor = correct.Value ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(result.PadRight(15));
            Console.ResetColor();
            Console.Write(" | ");

            if (correct.Value)
            {
                dto.correct = result;
                dto.incorrect.Clear();
            }
            else
                dto.incorrect.Add(result);
            return true;
        }

        private static bool? readYesNo()
        {
            char choice = (char)Console.Read();
            while (choice != 'y' && choice != 'n' && choice != 's')
            {
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write(" ");
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                choice = (char)Console.Read();;
            }
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
        private static string PathToRelative(this string path)
            => Path.AltDirectorySeparatorChar == '/'
                ? path
                : path.MSPathToRelative();

        private static string MSPathToRelative(this string path)
            => Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", path);
    }
}
