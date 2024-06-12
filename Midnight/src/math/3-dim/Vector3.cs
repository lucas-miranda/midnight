using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Vector3 : System.IEquatable<Vector3> {
    public static readonly Vector3
            Zero = new(0.0f, 0.0f, 0.0f),
            One = new(1.0f, 1.0f, 1.0f);

    public float X, Y, Z;

    public Vector3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(float xyz) : this(xyz, xyz, xyz) {
    }

    public Vector3(Vector2 vec2, float z) : this(vec2.X, vec2.Y, z) {
    }

    public Vector3(Vector2 vec2) : this(vec2.X, vec2.Y, 0.0f) {
    }

    internal Vector3(Xna.Vector3 xnaVector3) : this(xnaVector3.X, xnaVector3.Y, xnaVector3.Z) {
    }

    public float Dot(Vector3 v) {
        return X * v.X + Y * v.Y + Z * v.Z;
    }

    public Vector3 Cross(Vector3 v) {
        return new(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
    }

    public Vector3 Normalized() {
        if (ApproxZero()) {
            return Zero;
        }

        return this / Length();
    }

    public float Length() {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public bool ApproxEquals(Vector3 v) {
        return Math.ApproxEquals(X, v.X)
            && Math.ApproxEquals(Y, v.Y)
            && Math.ApproxEquals(Z, v.Z);
    }

    public bool ApproxZero() {
        return ApproxEquals(Zero);
    }

    public bool Equals(Vector3 v) {
        return !(X != v.X || Y != v.Y || Z != v.Z);
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Vector3 v && Equals(v);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + X.GetHashCode();
            hashCode = hashCode * 1610612741 + Y.GetHashCode();
            hashCode = hashCode * 1610612741 + Z.GetHashCode();
        }

        return hashCode;
    }

    public override string ToString() {
        return $"{X}, {Y}, {Z}";
    }

    public static Vector3 operator -(Vector3 v) {
        return new(-v.X, -v.Y, -v.Z);
    }

    public static Vector3 operator +(Vector3 a, Vector3 b) {
        return new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3 operator +(Vector3 v, float n) {
        return new(v.X + n, v.Y + n, v.Z + n);
    }

    public static Vector3 operator +(float n, Vector3 v) {
        return new(n + v.X, n + v.Y, n + v.Z);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b) {
        return new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3 operator -(Vector3 v, float n) {
        return new(v.X - n, v.Y - n, v.Z - n);
    }

    public static Vector3 operator -(float n, Vector3 v) {
        return new(n - v.X, n - v.Y, n - v.Z);
    }

    public static Vector3 operator *(Vector3 a, Vector3 b) {
        return new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }

    public static Vector3 operator *(Vector3 v, float n) {
        return new(v.X * n, v.Y * n, v.Z * n);
    }

    public static Vector3 operator *(float n, Vector3 v) {
        return new(n * v.X, n * v.Y, n * v.Z);
    }

    public static Vector3 operator /(Vector3 a, Vector3 b) {
        return new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    }

    public static Vector3 operator /(Vector3 v, float n) {
        return new(v.X / n, v.Y / n, v.Z / n);
    }

    public static Vector3 operator /(float n, Vector3 v) {
        return new(n / v.X, n / v.Y, n / v.Z);
    }

    public static bool operator ==(Vector3 a, Vector3 b) {
        return !(a.X != b.X || a.Y != b.Y || a.Z != b.Z);
    }

    public static bool operator !=(Vector3 a, Vector3 b) {
        return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    }

    internal Xna.Vector3 ToXna() {
        return new(X, Y, Z);
    }
}
