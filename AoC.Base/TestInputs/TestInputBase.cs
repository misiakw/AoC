using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AoC.Base.TestInputs
{
    public enum Outcome
    {
        Unknown, Correct, Invalid, TooLow, TooHigh
    }
    public abstract class TestInputBase<Result>
    {
        protected readonly string _filePath;
        public readonly string Name;
        public readonly string InputDir;
        public bool RunPart1 = true;
        public bool RunPart2 = true;

        public TestInputBase(string filePath, string name)
        {
            _filePath = filePath;
            Name = name;

            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? string.Empty);

            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();

            InputDir = Path.GetDirectoryName(_filePath)?.ToString() ?? string.Empty;
        }

        protected async Task<string> GetRaw()
            => await File.ReadAllTextAsync(_filePath);

        protected async Task<IEnumerable<string>> ReadLines()
        {
            var result = new List<string>();
            using (StreamReader sr = File.OpenText(_filePath))
            {
                var line = await sr.ReadLineAsync();
                while (line != null)
                {
                    result.Add(line.Replace("\n", "").Replace("\r", ""));
                    line = await sr.ReadLineAsync();
                }
                return result;
            }
        }

        public abstract Tuple<Outcome, Result> GetOutcome(int test, Result result);
    }
}
