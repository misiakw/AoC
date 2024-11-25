using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Common.Maps
{
    public class StaticMap<T> : IMap<T>
    {
        protected T[,] _data;

        public StaticMap(long width, long height, T def = default)
        {
            _data = new T[width, height];
            Width = width;
            Height = height;
            if(!def?.Equals(default(T)) ?? false)
            {
                for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                        _data[x, y] = def;
            }
        }
        public T? this[long x, long y] { 
            get => _data[x, y];
            set => _data[x, y] = value; }

        public long Width { get; protected set; }

        public long Height { get; protected set; }

        public Range rangeX => new Range(0, Width);

        public Range rangeY => new Range(0, Height);

        public string Draw(Func<T?, string> drawing, string split = "")
        {
            var sb = new StringBuilder();
            for (var y = 0; y < Height; y++)
            {
                IList<string> tmp = new List<string>();
                for (var x = 0; x < Width; x++)
                    tmp.Add(drawing(this[x, y]));
                sb.AppendLine(string.Join(split, tmp));
            }
            return sb.ToString();
        }
    }
}
