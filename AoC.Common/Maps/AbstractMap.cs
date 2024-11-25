using System;
using System.Collections.Generic;

namespace AoC.Common.Maps
{
    public abstract class AbstractMap<T> : IMap<T>
    {
        public abstract T? this[long x, long y] { get; set; }

        public abstract long Width { get; }

        public abstract long Height{ get; }

        public Range rangeX => throw new NotImplementedException();

        public Range rangeY => throw new NotImplementedException();

        public string Draw(Func<T?, string> drawing, string split = "")
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Where(Func<T, bool> selector)
        {
            throw new NotImplementedException();
        }
    }
}