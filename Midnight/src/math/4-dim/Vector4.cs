using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Vector4 : System.IEquatable<Vector4> {
    public static readonly Vector4
            Zero = new(0.0f, 0.0f, 0.0f, 0.0f),
            One = new(1.0f, 1.0f, 1.0f, 1.0f);

    public float X, Y, Z, W;

    public Vector4(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4(float xyzw) : this(xyzw, xyzw, xyzw, xyzw) {
    }

    internal Vector4(Xna.Vector4 xnaVector4)
        : this(xnaVector4.X, xnaVector4.Y, xnaVector4.Z, xnaVector4.W)
    {
    }

    public float Dot(Vector4 v) {
        return X * v.X + Y * v.Y + Z * v.Z + W * v.W;
    }

    public Vector4 Normalized() {
        if (ApproxZero()) {
            return Zero;
        }

        return this / Length();
    }

    public float Length() {
        return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
    }

    public bool ApproxEquals(Vector4 v) {
        return Math.ApproxEquals(X, v.X)
            && Math.ApproxEquals(Y, v.Y)
            && Math.ApproxEquals(Z, v.Z)
            && Math.ApproxEquals(W, v.W);
    }

    public bool ApproxZero() {
        return ApproxEquals(Zero);
    }

    public bool Equals(Vector4 v) {
        return !(X != v.X || Y != v.Y || Z != v.Z || W != v.W);
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Vector4 v && Equals(v);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + X.GetHashCode();
            hashCode = hashCode * 1610612741 + Y.GetHashCode();
            hashCode = hashCode * 1610612741 + Z.GetHashCode();
            hashCode = hashCode * 1610612741 + W.GetHashCode();
        }

        return hashCode;
    }

    public override string ToString() {
        return $"{X}, {Y}, {Z}, {W}";
    }

    public static Vector4 operator +(Vector4 a, Vector4 b) {
        return new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }

    public static Vector4 operator +(Vector4 v, float n) {
        return new(v.X + n, v.Y + n, v.Z + n, v.W + n);
    }

    public static Vector4 operator +(float n, Vector4 v) {
        return new(n + v.X, n + v.Y, n + v.Z, n + v.W);
    }

    public static Vector4 operator -(Vector4 a, Vector4 b) {
        return new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    }

    public static Vector4 operator -(Vector4 v, float n) {
        return new(v.X - n, v.Y - n, v.Z - n, v.W - n);
    }

    public static Vector4 operator -(float n, Vector4 v) {
        return new(n - v.X, n - v.Y, n - v.Z, n - v.W);
    }

    public static Vector4 operator *(Vector4 a, Vector4 b) {
        return new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
    }

    public static Vector4 operator *(Vector4 v, float n) {
        return new(v.X * n, v.Y * n, v.Z * n, v.W * n);
    }

    public static Vector4 operator *(float n, Vector4 v) {
        return new(n * v.X, n * v.Y, n * v.Z, n * v.W);
    }

    public static Vector4 operator /(Vector4 a, Vector4 b) {
        return new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
    }

    public static Vector4 operator /(Vector4 v, float n) {
        return new(v.X / n, v.Y / n, v.Z / n, v.W / n);
    }

    public static Vector4 operator /(float n, Vector4 v) {
        return new(n / v.X, n / v.Y, n / v.Z, n / v.W);
    }

    public static bool operator ==(Vector4 a, Vector4 b) {
        return !(a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W);
    }

    public static bool operator !=(Vector4 a, Vector4 b) {
        return a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;
    }

    internal Xna.Vector4 ToXna() {
        return new(X, Y, Z, W);
    }
}
