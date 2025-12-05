using AoC.Common.Abstractions;
using System;
using System.Collections.Generic;

namespace AoC.Common.Maps
{
    public class InifiniteMap<T>(T? def) : IMap<T> //Array2D<T>
    {
        private IDictionary<(long x, long y), T> _data = new Dictionary<(long x, long y), T>();
        private ValueScope scopeX = new ValueScope { min = 0, max = 0 };
        private ValueScope scopeY = new ValueScope { min = 0, max = 0 };


        public T? this[long x, long y] {
            get => _data.ContainsKey((x, y))
                ? _data[(x, y)] : def;
            set {
                if (_data.ContainsKey((x, y)))
                    if (value == null || value.Equals(default(T)))
                    {
                        //ToDo - remove and cleanup ranges
                    }
                    else
                        _data[(x, y)] = value;
                else if (value != null || !value.Equals(default(T)))
                {
                    _data.Add((x, y), value);
                    scopeX.Add(x);
                    scopeY.Add(y);
                }
            }
        }

        public long Width => scopeX.max - scopeX.min;

        public long Height => scopeY.max - scopeY.min;

        public string Draw(Func<T?, string> drawing, string split = "")
        {
            throw new NotImplementedException();
        }

        private class ValueScope
        {
            public required long min;
            public required long max;
            
            public void Add(long value)
            {
                if(min > value) min = value;
                if(max < value) max = value;
            }
        }
    }
}
