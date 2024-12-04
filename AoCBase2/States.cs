using System;
using System.Collections.Generic;
using System.IO;using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AoCBase2
{
    internal class Callback<T>
    {
        internal Func<T, TestState, Task<string>> callback;
        internal bool needSetup = false;
        internal bool run = true;
    }
    public class DayState<T>
    {
        internal bool isDirty => dto.tests.Any(t => t.isDirty);
        internal string stateFile;
        internal DayStateDTO dto;
        internal Callback<T>[] callback = new Callback<T>[2];
        internal Func<string, string, T> setupFunc = null;
        internal object context = null;
        public IList<TestState> DebugTests { get; set; } = new List<TestState>();
        public IEnumerable<TestState> Tests
        {
            get =>  DebugTests.Concat(dto.tests);
        }

        internal DayState() { }
        internal DayState(string path, int dayNum)
        {
            stateFile = path;
            dto = new DayStateDTO() { dayNum = dayNum };
            Save();
        }
        internal void Save()
        {
            var contents = JsonSerializer.Serialize(this.dto, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllLines(stateFile, new List<string>() { contents });
            foreach (var test in dto.tests)
            {
                test.isDirtyInternal = false;
                foreach (var result in test.result.Where(r => r != null))
                    result.isDirty = false;
            }
        }
        internal static DayState<T> GetState(int dayNum)
        {
            var filename = Path.Combine("data", $"day{dayNum}.json");
            return File.Exists(filename)
                ? DayState<T>.Load(filename)
                : new DayState<T>(filename, dayNum);
        }
        internal static DayState<T> Load(string path)
        {
            var contents = File.ReadAllText(path);
            var dto = JsonSerializer.Deserialize<DayStateDTO>(contents);
            return new DayState<T>()
            {
                dto = dto,
                stateFile = path
            };
        }
    }
    public class DayStateDTO
    {
        public int dayNum { get; set; }
        public IList<TestState> tests { get; set; } = new List<TestState>();
    }

    public class TestState : TestStateDTO
    {
        internal bool debug = false;
        internal bool run = true;
        internal bool isDirtyInternal = false;
        internal bool isDirty => isDirtyInternal || result.Any(r => r?.isDirty ?? false);
    }
    public class TestStateDTO
    {
        public string name { get; set; } = string.Empty;
        public string testFile { get; set; } = string.Empty;
        public TestResult[] result { get; set; } = new TestResult[2];
    }
    public class TestResult : TestResultDTO
    {
        internal bool isDirty = false;
        internal bool run { get; set; } = true;
        internal TestState test {  get; set; }
    }

    public class TestResultDTO
    {
        public string correct { get; set; } = null;
        public IList<string> incorrect { get; set; } = new List<string>();
    }
        }
