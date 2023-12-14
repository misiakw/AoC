﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}