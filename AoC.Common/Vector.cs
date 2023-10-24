using System;

namespace AoC.Common
{
    public class Vector3D
    {
        public long X { get; protected set; }
        public long Y { get; protected set; }
        public long Z { get; protected set; }
        public Rotation3D Rotation { get; protected set; }

        public Vector3D(long x, long y, long z) : this(x, y, z, new Rotation3D(0, 0, 0)) { }
        public Vector3D(long x, long y, long z, Rotation3D rotation)
        {
            X = x;
            Y = y;
            Z = z;
            Rotation = rotation;
        }

        public Vector3D RotateCW(Axis axis)
        {
            long A, B;
            switch (axis)
            {
                case Axis.X:
                    A = Z;
                    B = Y;
                    break;
                case Axis.Y:
                    A = Z;
                    B = X;
                    break;
                default: //Axis.Z
                    A = X;
                    B = Y;
                    break;
            }
            var tmp = A;
            A = B;
            B = -1 * tmp;

            switch (axis)
            {
                case Axis.X:
                    return new Vector3D(X, B, A, new Rotation3D(Rotation.X + 90, Rotation.Y, Rotation.Z));
                case Axis.Y:
                    return new Vector3D(B, Y, A, new Rotation3D(Rotation.X, Rotation.Y + 90, Rotation.Z));
                default: //Axis.Z
                    return new Vector3D(A, B, Z, new Rotation3D(Rotation.X, Rotation.Y, Rotation.Z + 90));
            }
        }
        public override string ToString() => $"Vec[{X},{Y}, {Z}]";

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override bool Equals(object? obj)
        {
            return obj is Vector3D d &&
                   X == d.X &&
                   Y == d.Y &&
                   Z == d.Z;
        }
    }

    public class Rotation3D
    {
        public readonly int X, Y, Z;
        public Rotation3D(int x, int y, int z)
        {
            X = x % 360;
            Y = y % 360;
            Z = z % 360;
        }
        public override string ToString() => $"Rot[{X},{Y},{Z}]";

        public override bool Equals(object? obj)
        {
            return obj is Rotation3D d &&
                   X == d.X &&
                   Y == d.Y &&
                   Z == d.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    public enum Axis
    {
        X, Y, Z
    }
}
