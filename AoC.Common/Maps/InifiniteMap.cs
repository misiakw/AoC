using System;
using System.Collections.Generic;

namespace AoC.Common.Maps
{
    public class InifiniteMap<T> : Array2D<T>, IMap<T>
    {

        public InifiniteMap(T? def = default(T)): base(def)
        {
        }
        public T? this[long x, long y] {
            get => base[x, y];
            set {
                if(value == null || value.Equals(default(T)))
                {
                    _data.Remove(key(x, y));
                    return;
                }
                base[x, y] = value;
            }
        }

        public long[,] Bounds
        {
            get
            {
                var b = base.Bounds;
                return new long[2, 2]
                {
                    {b[0].Item1, b[0].Item2 }, {b[1].Item1, b[1].Item2}
                };
            }
        }


        public IEnumerable<T> Where(Func<T, bool>selector){
            for(var x = _minX; x<=_maxX; x++)
                for(var y = _minY; y<=_maxY; y++)
                    if(selector(this[x,y]))
                        yield return this[x, y];
        }

        public string Draw(Func<T?, string> drawing, string split = "")
        {
            throw new NotImplementedException();
        }

        public Range rangeX => new Range(_minX, _maxX);

        public Range rangeY => new Range(_minY, _maxY);

        object? IMap.this[long x, long y] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /*public class MapItem<K>
        {
            public K Data;
            public long X;
            public long Y;
            public MapItem(K data, long x, long y)
            {
                this.Data = data;
                this.X = x;
                this.Y = y;
            }
        }*/
    }
}
