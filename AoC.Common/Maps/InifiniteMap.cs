using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                string key = $"{x}|{y}";
                if (value == null || (_data.ContainsKey(key) && _data[key].Equals(value)))
                {
                    _data.Remove(key);
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

        public class MapItem<K>
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
        }


    }
}
