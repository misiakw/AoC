using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2021.Common
{
    public class Vector3D: IEquatable<Vector3D>
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
        //public Vector3D Transpose(long x, long y, long z) => new Vector3D(x + X, y + Y, z + Z, Rotation);
        public override string ToString() => $"Vec[{X},{Y}, {Z}]";

        public bool Equals(Vector3D other) => X == other.X && Y == other.Y && Z == other.Z;

        public long NYLength => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
    }

    public class Rotation3D: IEquatable<Rotation3D>
    {
        public readonly int X, Y, Z;
        public Rotation3D(int x, int y, int z)
        {
            X = x % 360;
            Y = y % 360;
            Z = z % 360;
        }
        public override string ToString() => $"Rot[{X},{Y},{Z}]";

        public static Rotation3D Parse(string input)
        {
            var pos = input.Replace("Rot[", "").Replace("]", "").Split(",").Select(s => int.Parse(s)).ToArray();
            return new Rotation3D(pos[0], pos[1], pos[2]);
        }

        public bool Equals(Rotation3D other) => X == other.X && Y == other.Y && Z == other.Z;
    }

    public enum Axis
    {
        X, Y, Z
    }
}
