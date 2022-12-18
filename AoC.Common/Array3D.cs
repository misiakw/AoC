using System;
using System.Collections.Generic;

namespace AoC.Common
{
    public class Array3D<T>
    {
        public Array3D(T def = default(T))
        {
            defaultValue = def;
        }

        protected IDictionary<string, T> _data = new Dictionary<string, T>();
        public IList<T> ToList() => _data.Select(kv=> kv.Value).ToList();
        protected long _minX = long.MaxValue;
        protected long _maxX = long.MinValue;
        protected long _minY = long.MaxValue;
        protected long _maxY = long.MinValue;
        protected long _minZ  = long.MaxValue;
        protected long _maxZ = long.MinValue;
        private readonly T defaultValue;
        public long RangeX
        {
            get
            {
                return _maxX - _minX;
            }
        }
        public long RangeY
        {
            get
            {
                return _maxY - _minY;
            }
        }
        public long RangeZ
        {
            get
            {
                return _maxZ - _minZ;
            }
        }
        /// <summary>returns span of X and Y (in this order)</summary>
        public Tuple<long, long>[] Bounds
        {
            get
            {
                return new Tuple<long, long>[3]
                {
                    Tuple.Create(_minX, _maxX),
                    Tuple.Create(_minY, _maxY),
                    Tuple.Create(_minZ, _maxZ)
                };
            }
        }

        public T this[long[] arr] => this[arr[0], arr[1], arr[2]]; 
        public T this[long x, long y, long z]
        {
            get
            {
                var key = $"{x},{y},{z}";
                return _data.ContainsKey(key)
                    ? _data[key]
                    : defaultValue;
            }
            set
            {
                var key = $"{x},{y},{z}";
                if (_data.ContainsKey(key))
                {
                    _data[key] = value;
                }
                else
                {
                    _data.Add(key, value);
                    if (_minX > x) _minX = x;
                    if (_maxX < x) _maxX = x;
                    if (_minY > y) _minY = y;
                    if (_maxY < y) _maxY = y;
                    if (_minZ > z) _minZ = z;
                    if (_maxZ < z) _maxZ = z;
                }
            }
        }
    }
}
