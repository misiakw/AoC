using AoC.Common.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Common
{
    public interface IMap
    {
        object? this[long x, long y] { get; set; }
        long Width { get; }
        long Height { get; }
        Range rangeX { get; }
        Range rangeY { get; }
    }
    public interface IMap<T>: IMap
    {
        T? this[long x, long y] { get; set; }
        string Draw(Func<T?, string> drawing, string split = "");
        //IEnumerable<T> Where(Func<T, bool>selector);
        //IEnumerable<T> Where(Func<T, bool>selector);
    }
    public class Map<T>: InifiniteMap<T>, IMap<T>{

        //ToDo: Exception at condtructor so that I won't use it on future 

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