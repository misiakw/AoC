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
        protected long _minX, _maxX = 0;
        protected long _minY, _maxY = 0;
        protected long _minZ, _maxZ = 0;
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
