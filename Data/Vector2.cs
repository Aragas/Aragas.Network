using System;
using System.Runtime.InteropServices;

namespace Aragas.Network.Data
{
    /// <summary>
    /// Represents the location of an object in 2D space (float).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public readonly float X;
        public readonly float Y;


        public Vector2(float value) { X = Y = value; }
        public Vector2(float x, float y) { X = x; Y = y; }
        public Vector2(double x, double y) { X = (float) x; Y = (float) y; }
        public Vector2(Vector2 v) { X = v.X; Y = v.Y; }


        /// <summary>
        /// Converts this Vector2 to a string.
        /// </summary>
        public override string ToString() => $"X: {X}, Y: {Y}";

        #region Math

        public static Vector2 Floor(Vector2 value) => new Vector2(Math.Floor(value.X), Math.Floor(value.Y));
        public Vector2 Floor() => Floor(this);

        public static Vector2 Ceiling(Vector2 value) => new Vector2(Math.Ceiling(value.X), Math.Ceiling(value.Y));
        public Vector2 Ceiling() => Ceiling(this);

        private static float Square(float num) => num * num;
        public static float DistanceTo(Vector2 a, Vector2 b) => a.DistanceTo(b);
        public float DistanceTo(Vector2 other) => (float) Math.Sqrt(Square(other.X - X) + Square(other.Y - Y));

        /// <summary>
        /// Finds the distance of this vector from Vector2.Zero
        /// </summary>
        public double Distance() => DistanceTo(Zero);

        public static Vector2 Min(Vector2 a, Vector2 b) => new Vector2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        public Vector2 Min(Vector2 other) => new Vector2(Math.Min(X, other.X), Math.Min(Y, other.Y));

        public static Vector2 Max(Vector2 a, Vector2 b) => new Vector2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        public Vector2 Max(Vector2 other) => new Vector2(Math.Max(X, other.X), Math.Max(Y, other.Y));

        public static Vector2 Delta(Vector2 a, Vector2 b) => a - b;
        public Vector2 Delta(Vector2 other) => this - other;

        public static Vector2 Normalize(Vector2 value)
        {
            var factor = 1f / DistanceTo(value, Zero);
            return value * factor;
        }
        public Vector2 Normalize() => Normalize(this);

        #endregion

        #region Operators

        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);
        public static Vector2 operator ++(Vector2 a) => new Vector2(a.X, a.Y) + 1.0;
        public static Vector2 operator --(Vector2 a) => new Vector2(a.X, a.Y) - 1.0;
        
        public static bool operator !=(Vector2 a, Vector2 b) => !a.Equals(b);
        public static bool operator ==(Vector2 a, Vector2 b) => a.Equals(b);
        public static bool operator >(Vector2 a, Vector2 b) => a.X > b.X && a.Y > b.Y;
        public static bool operator <(Vector2 a, Vector2 b) => a.X < b.X && a.Y < b.Y;
        public static bool operator >=(Vector2 a, Vector2 b) => a.X >= b.X && a.Y >= b.Y;
        public static bool operator <=(Vector2 a, Vector2 b) => a.X <= b.X && a.Y <= b.Y;

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator %(Vector2 a, Vector2 b) => new Vector2(a.X % b.X, a.Y % b.Y);

        public static Vector2 operator +(Vector2 a, double b) => new Vector2(a.X + b, a.Y + b);
        public static Vector2 operator -(Vector2 a, double b) => new Vector2(a.X - b, a.Y - b);
        public static Vector2 operator *(Vector2 a, double b) => new Vector2(a.X * b, a.Y * b);
        public static Vector2 operator /(Vector2 a, double b) => new Vector2(a.X / b, a.Y / b);
        public static Vector2 operator %(Vector2 a, double b) => new Vector2(a.X % b, a.Y % b);

        public static Vector2 operator +(double a, Vector2 b) => new Vector2(a + b.X, a + b.Y);
        public static Vector2 operator -(double a, Vector2 b) => new Vector2(a - b.X, a - b.Y);
        public static Vector2 operator *(double a, Vector2 b) => new Vector2(a * b.X, a * b.Y);
        public static Vector2 operator /(double a, Vector2 b) => new Vector2(a / b.X, a / b.Y);
        public static Vector2 operator %(double a, Vector2 b) => new Vector2(a % b.X, a % b.Y);

        #endregion

        #region Constants

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            if (obj is float)
                return Equals((float) obj);

            if (obj is Vector2)
                return Equals((Vector2) obj);

            return false;
        }
        public bool Equals(float other) => other.Equals(X) && other.Equals(Y);
        public bool Equals(Vector2 other) => other.X.Equals(X) && other.Y.Equals(Y);

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    }
}