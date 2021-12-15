using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Common
{
    public class Point
    {
        public Point(long X, long Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Point (string[] pos)
        {
            this.X = long.Parse(pos[0].Trim());
            this.Y = long.Parse(pos[1].Trim());
        }
        public long X { get; protected set; }
        public long Y { get; protected set; }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }
    }
}
