using System.Collections.Concurrent;
using System.Text;

namespace AoC.Common
{
    public class Array2D<T>
    {
        public Array2D(T? def = default(T)){
            defaultValue = def;
        }

        protected ConcurrentDictionary<string, T?> _data = new ConcurrentDictionary<string, T?>();
        protected long _minX = long.MaxValue;
        protected long _maxX = long.MinValue;
        protected long _minY = long.MaxValue;
        protected long _maxY = long.MinValue;
        private readonly T? defaultValue;
        public long Width
        {
            get
            {
                return _data.Any() ? _maxX - _minX : 0;
            }
        }
        public long Height
        {
            get
            {
                return _data.Any() ? _maxY - _minY : 0;
            }
        }
        /// <summary>returns span of X and Y (in this order)</summary>
        public Tuple<long, long>[] Bounds
        {
            get
            {
                return _data.Any() 
                ? new Tuple<long, long>[2]
                {
                    Tuple.Create(_minX, _maxX),
                    Tuple.Create(_minY, _maxY),
                }
                : new Tuple<long, long>[2]
                {
                    Tuple.Create(0L, 0L),
                    Tuple.Create(0L, 0L),
                };
            }
        }

        public T? this[Point p] => this[p.X, p.Y];

        public T? this[long x, long y]
        {
            get
            {
                string key = $"{x},{y}";
                return _data.ContainsKey(key)
                    ? _data[key]
                    : defaultValue;
            }
            set
            {
                string key = $"{x},{y}";
                if (_data.ContainsKey(key))
                {
                    _data[key] = value;
                }
                else
                {
                    _data.TryAdd(key, value);
                    if (_minX > x) _minX = x;
                    if (_maxX < x) _maxX = x;
                    if (_minY > y) _minY = y;
                    if (_maxY < y) _maxY = y;
                }
            }
        }

       public string Draw(Func<T?, string> drawing, string split = "")
        {
            if (!_data.Any()) return "[Empty]";
            
            var sb = new StringBuilder();
            for(var y = _minY; y<=_maxY; y++)
            {
                IList<string> tmp = new List<string>();
                for (var x = _minX; x <= _maxX; x++)
                    tmp.Add(drawing(this[x, y]));
                sb.AppendLine(string.Join(split, tmp));
            }
            return sb.ToString();
        }
    }
}
