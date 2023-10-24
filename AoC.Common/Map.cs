using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Common
{
    public class Map<T>{
        protected IDictionary<string, MapItem<T>> _data = new Dictionary<string, MapItem<T>>();
        protected long _minX = long.MaxValue;
        protected long _maxX = long.MinValue;
        protected long _minY = long.MaxValue;
        protected long _maxY = long.MinValue;
        private readonly T? defaultValue;

        public Map(T? def = default(T)){
            defaultValue = def;
        }

        public long Width => _data.Any() ? _maxX - _minX : 0;
        public long Height => _data.Any() ? _maxY - _minY : 0;

         public long[,] Bounds =>
            _data.Any() 
            ? new long[2,2] {
                {_minX, _maxX}, {_minY, _maxY}
            }
            : new long[2,2]{
                {0, 0}, {0, 0}
            };

        public T? this[long x, long y]
        {
            get
            {
                string key = $"{x}|{y}";
                return _data.ContainsKey(key)
                    ? _data[key].Data
                    : defaultValue;
            }
            set
            {
                string key = $"{x}|{y}";
                if(value == null || (_data.ContainsKey(key) && _data[key].Data.Equals(value))){
                    _data.Remove(key);
                    return;
                }

                _data.Add(key, new MapItem<T>(value, x, y));
                if (_minX > x) _minX = x;
                if (_maxX < x) _maxX = x;
                if (_minY > y) _minY = y;
                if (_maxY < y) _maxY = y;
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

        public class MapItem<K>{
            public K Data;
            public long X;
            public long Y;
            public MapItem(K data, long x, long y){
                this.Data = data;
                this.X = x;
                this.Y = y;
            }
        }
    }
}