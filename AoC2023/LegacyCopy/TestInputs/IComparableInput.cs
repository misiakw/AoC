using System;

namespace AoC.Base.TestInputs
{
    public class IComparableInput<TResult> : TestInputBase<TResult> where TResult : IComparable
    {
        public IComparableInput(string name, string filePath) : base(filePath, name)
        { }

        public override Tuple<Outcome, TResult> GetOutcome(int test, TResult result)
        {
            var desiredResult = _results[test];

            if (desiredResult == null)
                return Tuple.Create(Outcome.Unknown, desiredResult);

            switch ((result).CompareTo(desiredResult))
            {
                case -1: return Tuple.Create(Outcome.TooLow, desiredResult);
                case 0: return Tuple.Create(Outcome.Correct, desiredResult);
                case 1: return Tuple.Create(Outcome.TooHigh, desiredResult);
            }
            return Tuple.Create(Outcome.Unknown, desiredResult);
        }
    }
}
