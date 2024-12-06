using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AoC.Common.Maps.StaticMap
{
    public class StaticMap : IMap, IEnumerable
    {
        protected object[,] _data;
        public StaticMap(long width, long height)
        {
            _data = new object[width, height];
            Width = width;
            Height = height;
        }
        public object? this[long x, long y]
        {
            get => _data[x, y];
            set => _data[x, y] = value;
        }

        public long Width { get; protected set; }

        public long Height { get; protected set; }

        public Range rangeX => new Range(0, Width);

        public Range rangeY => new Range(0, Height);

        public IEnumerator GetEnumerator()
            => new StaticMapEnumerator(this);
    }
    public class StaticMap<T> : StaticMap, IMap<T>, IEnumerable<T>
    {
        public StaticMap(long width, long height, T def = default): base(width, height)
        {
            if (!def?.Equals(default(T)) ?? false)
            {
                for (var y = 0; y < height; y++)
                    for (var x = 0; x < width; x++)
                        _data[x, y] = def;
            }
        }
        public T? this[long x, long y]
        {
            get => (T)_data[x, y];
            set => _data[x, y] = value;
        }

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

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => new StaticMapEnumerator<T>(this);
    }
    public class StaticMapEnumerator : IEnumerator
    {
        protected StaticMap _map;
        private (long, long) pos = (0, 0);
        internal StaticMapEnumerator(StaticMap map)
        {
            _map = map;
        }
        public object Current => _map[pos.Item1, pos.Item2] ?? default(object);

        public bool MoveNext()
        {
            if (pos.Item1 < _map.Width-1)
                pos.Item1++;
            else if (pos.Item2 < _map.Height-1)
                pos = (0, pos.Item2 + 1);
            else return false;
            return true;
        }

        public void Reset()
        {
            pos = (0, 0);
        }
    }
    public class StaticMapEnumerator<T> : StaticMapEnumerator, IEnumerator<T>
    {
        internal StaticMapEnumerator(StaticMap<T> map) : base(map)
        {
        }

        T IEnumerator<T>.Current => (T)this.Current;

        public void Dispose()
        {
        }
    }
}
