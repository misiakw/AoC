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
        public bool RunPart1 = false;
        public bool RunPart2 = false;
        public bool SkipBoth = false;
        protected Result[] _results = new Result[2];
        protected TestInputBase(string filePath, string name)
        {
            _filePath = filePath;
            Name = name;

            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? string.Empty);

            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();

            InputDir = Path.GetDirectoryName(_filePath)?.ToString() ?? string.Empty;
        }

        public async Task<string> GetRaw()
            => await File.ReadAllTextAsync(_filePath);

        public async Task<IEnumerable<string>> ReadLines()
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

        protected TestInputBase<Result> SetResult(byte testNum, Result result)
        {
            if (testNum < 1 || testNum > 2)
                throw new InvalidDataException("Parts to run should be 1 or 2");
            _results[testNum-1] = result;
            return this;
        }

        public TestInputBase<Result> Part1() => Part1(default);
        public TestInputBase<Result> Part1(Result result){
            RunPart1 = true;
            return SetResult(1, result);
        }
        public TestInputBase<Result> Part2() => Part2(default);
        public TestInputBase<Result> Part2(Result result){
            RunPart2 = true;
            return SetResult(2, result);
        }
        public TestInputBase<Result> Off()
        {
            SkipBoth = true;
            return this;
        }
    }
}
