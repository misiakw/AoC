using System;

namespace AoC.Common.Maps
{
    public abstract class AbstractMap<T> : IMap<T>
    {
        public abstract T? this[long x, long y] { get; set; }
        object? IMap.this[long x, long y] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public abstract long Width { get; }

        public abstract long Height{ get; }

        public Range rangeX => throw new NotImplementedException();

        public Range rangeY => throw new NotImplementedException();

        public abstract string Draw(Func<T?, string> drawing, string split = "");
    }
}