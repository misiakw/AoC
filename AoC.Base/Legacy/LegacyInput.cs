using AoC.Base.TestInputs;
using System;
using System.Collections.Generic;
using System.Linq;
namespace AoC.Base
{
    public class LegacyInput : TestInputBase<object>
    {
        public LegacyInput(string FilePath, string name) : base(FilePath, name)
        { }

        public string Raw => GetRaw().Result.Replace("\r", "");
        public new string[] Lines {
            get
            {
                var task = ReadLines();
                task.Wait();
               return task.Result.ToArray();
            }
        }
        public readonly TestType[] Tests = new TestType[2];
        public readonly Tuple<object, Type>[] Result = new Tuple<object, Type>[2];
        public readonly IList<object>[] Invalid = new IList<object>[2] { new List<object>(), new List<object>() };
        private IList<object?> _storedInput = new List<object?>();
        public object[,] FailedResults = new object[2, 2];
        public object? Cache = null;

        public IList<string> Split(Func<string, IList<string>> splitFunc)
        {
            return splitFunc(Raw.Trim()).Select(s => s.Trim()).ToList();
        }
        public IList<T> Prepare<T>(Func<string, IList<string>> splitFunc, Func<string, T> transformFunc, bool force = false)
        {
            if (!_storedInput.Any() || force == true)
            {
                _storedInput = new List<object?>();
                foreach (var line in Split(splitFunc))
                    if (!string.IsNullOrEmpty(line))
                        _storedInput.Add(transformFunc(line));
            }
            return (IList<T>)_storedInput.Select(o => (T?)o).ToList();
        }

        public override Tuple<Outcome, object> GetOutcome(int testNum, object result)
        {
            var desiredResult = Result[testNum];

            if (desiredResult?.Item1 == null)
                return Tuple.Create(Outcome.Unknown, desiredResult.Item1);

            if (result.GetType() != desiredResult.Item2)
                return Tuple.Create(Outcome.Invalid, desiredResult.Item1);

            if (result.GetType().GetInterface("IComparable") != null)
            {
                switch (((IComparable)result).CompareTo(desiredResult.Item1))
                {
                    case -1: return Tuple.Create(Outcome.TooLow, desiredResult.Item1);
                    case  0: return Tuple.Create(Outcome.Correct, desiredResult.Item1);
                    case  1: return Tuple.Create(Outcome.TooHigh, desiredResult.Item1);
                }
            }
            return Tuple.Create(Outcome.Unknown, desiredResult.Item1);
        }

        public enum TestType
        {
            Skip = 0,
            Verbal = 1,
            Silent = 2
        }
    }
}
