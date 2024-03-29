﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2016.Common
{
    public class Array2D<T>
    {
        public Array2D(T def = default(T)){
            defaultValue = def;
        }

        protected IDictionary<string, T> _data = new Dictionary<string, T>();
        protected long _minX, _maxX = 0;
        protected long _minY, _maxY = 0;
        private readonly T defaultValue;
        public long Width
        {
            get
            {
                return _maxX - _minX;
            }
        }
        public long Height
        {
            get
            {
                return _maxY - _minY;
            }
        }
        /// <summary>returns span of X and Y (in this order)</summary>
        public Tuple<long, long>[] Bounds
        {
            get
            {
                return new Tuple<long, long>[2]
                {
                    Tuple.Create(_minX, _maxX),
                    Tuple.Create(_minY, _maxY),
                };
            }
        }

        public T this[long x, long y]
        {
            get
            {
                var key = $"{x},{y}";
                return _data.ContainsKey(key)
                    ? _data[key]
                    : defaultValue;
            }
            set
            {
                var key = $"{x},{y}";
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
                }
            }
        }

       public string Draw(Func<T, string> drawing, string split = "")
        {
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
