using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCBase2
{
    internal class Callback<T>
    {
        internal Func<T, Task<string>> callback;
        internal bool needSetup = false;
    }
    public class DayState<T>
    {
        internal TestState selectedTest;
        internal string stateFile;
        internal DayStateDTO dto;
        internal Callback<T>[] callback = new Callback<T>[2];
        internal Func<string, string, T> setupFunc = null;

        internal DayState() { }
        internal DayState(string path, int dayNum)
        {
            stateFile = path;
            dto = new DayStateDTO() { dayNum = dayNum};
            Save();
        }
        internal void Save()
        {
            var contents = JsonSerializer.Serialize(this.dto, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllLines(stateFile, new List<string>() { contents });
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

    public class TestState: TestStateDTO
    {
        internal TestResultDTO selectedResult;
    }
    public class TestStateDTO
    {
        public string name { get; set; } = string.Empty;
        public string testFile { get; set; } = string.Empty;
        public TestResultDTO[] result { get; set; } = new TestResultDTO[2];

    }

    public class TestResultDTO
    {
        public bool run { get; set; } = false;
        public string correct { get; set; } = null;
        public IList<string> incorrect { get; set; } = new List<string>();
        public bool isDebug { get; set; } = false;
    }
}
