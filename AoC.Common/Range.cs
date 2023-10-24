using System;

namespace AoC.Common
{
    public class Range: IComparable<Range>
    {
        public long Min { get; protected set;}
        public long Max { get; protected set;}

        public Range(long a, long b){
            if (a<=b){
                Min = a;
                Max = b;
            }else{
                Min = b;
                Max = a;
            }
        }

        public bool Overlap(Range other){
                if (other.Min <= Min && other.Max >= Min)
                    return true;
                if (Min <= other.Min && Max >= other.Min)
                    return true;
                return false;
            }

            public static Range operator +(Range a, Range b){
                a.Min = a.Min <= b.Min? a.Min: b.Min;
                a.Max = a.Max >= b.Max? a.Max: b.Max;
                return a;
            }

        public int CompareTo(Range? other) =>  Min.CompareTo(other?.Min ?? long.MinValue);
        public override string ToString() => $"Range[{Min}|{Max}]";
        public override bool Equals(object? obj) => ToString().Equals(obj?.ToString());
        public override int GetHashCode() => base.GetHashCode();
    }
}