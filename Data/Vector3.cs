using System;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Represents the location of an object in 3D space (float).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vector3 : IEquatable<Vector3>
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;


        public Vector3(float value) { X = Y = Z = value; }
        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }
        public Vector3(double x, double y, double z) { X = (float) x; Y = (float) y; Z = (float) z; }
        public Vector3(Vector3 v) { X = v.X; Y = v.Y; Z = v.Z; }


        /// <summary>
        /// Converts this Vector3 to a string.
        /// </summary>
        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";

        #region Math

        public static Vector3 Floor(Vector3 value) => new Vector3(Math.Floor(value.X), Math.Floor(value.Y), Math.Floor(value.Z));
        public Vector3 Floor() => Floor(this);

        public static Vector3 Ceiling(Vector3 value) => new Vector3(Math.Ceiling(value.X), Math.Ceiling(value.Y), Math.Ceiling(value.Z));
        public Vector3 Ceiling() => Ceiling(this);


        private static float Square(float num) => num * num;
        public static float DistanceTo(Vector3 a, Vector3 b) => a.DistanceTo(b);
        public float DistanceTo(Vector3 other) => (float) Math.Sqrt(Square(other.X - X) + Square(other.Y - Y) + Square(other.Z - Z));

        /// <summary>
        /// Finds the distance of this vector from Vector3.Zero
        /// </summary>
        public float Distance() => DistanceTo(Zero);

        public static Vector3 Min(Vector3 a, Vector3 b) => new Vector3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        public Vector3 Min(Vector3 other) => new Vector3(Math.Min(X, other.X), Math.Min(Y, other.Y), Math.Min(Z, other.Z));

        public static Vector3 Max(Vector3 a, Vector3 b) => new Vector3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        public Vector3 Max(Vector3 other) => new Vector3(Math.Max(X, other.X), Math.Max(Y, other.Y), Math.Max(Z, other.Z));

        public static Vector3 Delta(Vector3 a, Vector3 b) => a - b;
        public Vector3 Delta(Vector3 other) => this - other;

        public static Vector3 Normalize(Vector3 value)
        {
            var factor = 1f / DistanceTo(value, Zero);
            return value * factor;
        }
        public Vector3 Normalize() => Normalize(this);

        #endregion

        #region Operators

        public static Vector3 operator -(Vector3 a) => new Vector3(-a.X, -a.Y, -a.Z);
        public static Vector3 operator ++(Vector3 a) => new Vector3(a.X, a.Y, a.Z) + One;
        public static Vector3 operator --(Vector3 a) => new Vector3(a.X, a.Y, a.Z) - One;

        public static bool operator !=(Vector3 a, Vector3 b) => !a.Equals(b);
        public static bool operator ==(Vector3 a, Vector3 b) => a.Equals(b);
        public static bool operator >(Vector3 a, Vector3 b) => a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        public static bool operator <(Vector3 a, Vector3 b) => a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        public static bool operator >=(Vector3 a, Vector3 b) => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
        public static bool operator <=(Vector3 a, Vector3 b) => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator *(Vector3 a, Vector3 b) => new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Vector3 operator /(Vector3 a, Vector3 b) => new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Vector3 operator %(Vector3 a, Vector3 b) => new Vector3(a.X % b.X, a.Y % b.Y, a.Z % b.Z);

        public static Vector3 operator +(Vector3 a, float b) => new Vector3(a.X + b, a.Y + b, a.Z + b);
        public static Vector3 operator -(Vector3 a, float b) => new Vector3(a.X - b, a.Y - b, a.Z - b);
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.X * b, a.Y * b, a.Z * b);
        public static Vector3 operator /(Vector3 a, float b) => new Vector3(a.X / b, a.Y / b, a.Z / b);
        public static Vector3 operator %(Vector3 a, float b) => new Vector3(a.X % b, a.Y % b, a.Z % b);

        public static Vector3 operator +(float a, Vector3 b) => new Vector3(a + b.X, a + b.Y, a + b.Z);
        public static Vector3 operator -(float a, Vector3 b) => new Vector3(a - b.X, a - b.Y, a - b.Z);
        public static Vector3 operator *(float a, Vector3 b) => new Vector3(a * b.X, a * b.Y, a * b.Z);
        public static Vector3 operator /(float a, Vector3 b) => new Vector3(a / b.X, a / b.Y, a / b.Z);
        public static Vector3 operator %(float a, Vector3 b) => new Vector3(a % b.X, a % b.Y, a % b.Z);

        #endregion

        #region Constants

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Backwards = new Vector3(0, 0, -1);
        public static readonly Vector3 Forwards = new Vector3(0, 0, 1);

        public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            if (obj is float)
                return Equals((float) obj);

            if (obj is Vector3)
                return Equals((Vector3) obj);

            return false;
        }
        public bool Equals(float other) => other.Equals(X) && other.Equals(Y) && other.Equals(Z);
        public bool Equals(Vector3 other) => other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
    }
}
