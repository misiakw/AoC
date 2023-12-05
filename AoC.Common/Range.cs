using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Common
{
    public class Range : IComparable<Range>
    {
        public long Min { get; protected set; }
        public long Max { get; protected set; }

        public long Span => Max - Min;

        //ToDo: add inclusive/exclusive ends
        public Range(long a, long b)
        {
            if (a <= b)
            {
                Min = a;
                Max = b;
            }
            else
            {
                Min = b;
                Max = a;
            }
        }

        //
        // Summary:
        //     Initializes a new instance of the System.InvalidOperationException class with
        //     a specified error message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public bool Overlap(Range other)
        {
            if (other.Min <= Min && other.Max >= Min)
                return true;
            if (Min <= other.Min && Max >= other.Min)
                return true;
            return false;
        }

        public static Range operator +(Range a, Range b)
        {
            var c = a.Overlap(b);
            if (!a.Overlap(b))
                throw new InvalidOperationException("Ranges don't overlap");
            var Min = a.Min <= b.Min ? a.Min : b.Min;
            var Max = a.Max >= b.Max ? a.Max : b.Max;
            return new Range(Min, Max);
        }

        public static IEnumerable<Range> operator -(Range a, Range b)
        {
            if (!a.Overlap(b))
                throw new InvalidOperationException("Ranges don't overlap");
            if (b.Min <= a.Min && b.Max >= a.Max)
                yield return null;
            else if (b.Min > a.Min && b.Max < a.Max)
            {
                yield return new Range(a.Min, b.Min);
                yield return new Range(b.Max, a.Max);
            }
            else if (b.Min <= a.Min && b.Max < a.Max)
                yield return new Range(b.Max, a.Max);
            else
                yield return new Range(a.Min, b.Min);
        }

        public Range? ContainedBy(Range containedBy)
            => this.Overlap(containedBy)
            ? new Range(containedBy.Min >= this.Min ? containedBy.Min : this.Min,
                containedBy.Max <= this.Max ? containedBy.Max : this.Max)
            : null;

        public static IEnumerable<Range> ContainedBy(IEnumerable<Range> source, Range containedBy)
        {
            if (!source.Any(a => a.Overlap(containedBy)))
                return null;
            return InnerContainedBy(source, containedBy);
        }
        private static IEnumerable<Range> InnerContainedBy(IEnumerable<Range> source, Range containedBy)
        {
            foreach (var a in source.Where(a => a.Overlap(containedBy)))
                if(a.Overlap(containedBy))
                    yield return a.ContainedBy(containedBy);
        }

        //ToDo - CommonStripped - podajesz szeroki zakres oraz inne zakresy ktore z nim koliduja, a on dzieli je na najmniejsze unikalne male zakresy

        public int CompareTo(Range? other) => Min.CompareTo(other?.Min ?? long.MinValue);
        public override string ToString() => $"Range[{Min}|{Max}]";
        public override bool Equals(object? obj) => ToString().Equals(obj?.ToString());
        public override int GetHashCode() => base.GetHashCode();
    }
}