using System;

namespace AoC.Common.Abstractions
{
    public interface IMap
    {
        long Width { get; }
        long Height { get; }
        //(long min, long max) rangeX { get; }
        //(long min, long max) rangeY { get; }
    }

    public interface IMap<T> : IMap
    {
        T? this[long x, long y] { get; set; }
        string Draw(Func<T?, string> drawing, string split = "");
        //IEnumerable<T> Where(Func<T, bool>selector);
        //IEnumerable<T> Where(Func<T, bool>selector);
    }
}
