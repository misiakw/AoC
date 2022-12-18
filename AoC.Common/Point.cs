namespace AoC.Common
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

        public void Move(long dx, long dy){
            X += dx;
            Y += dy;
        }
        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }
        public override string ToString()
        {
            return $"Point[{X},{Y}]";
        }
    }
}
