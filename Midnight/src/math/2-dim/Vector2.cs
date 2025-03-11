using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Vector2 : System.IEquatable<Vector2> {
    public static readonly Vector2
            Zero = new(0.0f, 0.0f),
            One = new(1.0f, 1.0f),
            Up = new(0.0f, -1.0f),
            Right = new(1.0f, 0.0f),
            Down = new(0.0f, 1.0f),
            Left = new(-1.0f, 0.0f),
            UpRight = Up + Right,
            DownRight = Down + Right,
            DownLeft = Down + Left,
            UpLeft = Up + Left;

    public float X, Y;

    public Vector2(float x, float y) {
        X = x;
        Y = y;
    }

    public Vector2(float xy) : this(xy, xy) {
    }

    internal Vector2(Xna.Vector2 xnaVector2) : this(xnaVector2.X, xnaVector2.Y) {
    }

    public float Dot(Vector2 v) {
        return X * v.X + Y * v.Y;
    }

    public Vector2 Normalized() {
        if (ApproxZero()) {
            return Zero;
        }

        return this / Length();
    }

    public float Length() {
        return Math.Sqrt(X * X + Y * Y);
    }

    public bool ApproxEquals(Vector2 v) {
        return Math.ApproxEquals(X, v.X)
            && Math.ApproxEquals(Y, v.Y);
    }

    public bool ApproxZero() {
        return ApproxEquals(Zero);
    }

    public Vector2 Floor() {
        return new(Math.Floor(X), Math.Floor(Y));
    }

    public Vector2 Ceil() {
        return new(Math.Ceil(X), Math.Ceil(Y));
    }

    public Vector2 Round() {
        return new(Math.Round(X), Math.Round(Y));
    }

    public Vector2 Round(System.MidpointRounding mode) {
        return new(Math.Round(X, mode), Math.Round(Y, mode));
    }

    public Vector2I ToInt() {
        return new((int) X, (int) Y);
    }

    public bool Equals(Vector2 v) {
        return X == v.X && Y == v.Y;
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Vector2 v && Equals(v);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + X.GetHashCode();
            hashCode = hashCode * 1610612741 + Y.GetHashCode();
        }

        return hashCode;
    }

    public override string ToString() {
        return $"{X}, {Y}";
    }

    public static Vector2 operator -(Vector2 v) {
        return new(-v.X, -v.Y);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b) {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator +(Vector2 a, Vector2I b) {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator +(Vector2 v, float n) {
        return new(v.X + n, v.Y + n);
    }

    public static Vector2 operator +(float n, Vector2 v) {
        return new(n + v.X, n + v.Y);
    }

    public static Vector2 operator +(Vector2 v, Size2 s) {
        return new(v.X + s.Width, v.Y + s.Height);
    }

    public static Vector2 operator +(Vector2 v, Size2I s) {
        return new(v.X + s.Width, v.Y + s.Height);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b) {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator -(Vector2 a, Vector2I b) {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator -(Vector2 v, float n) {
        return new(v.X - n, v.Y - n);
    }

    public static Vector2 operator -(float n, Vector2 v) {
        return new(n - v.X, n - v.Y);
    }

    public static Vector2 operator -(Vector2 a, Size2 b) {
        return new(a.X - b.Width, a.Y - b.Height);
    }

    public static Vector2 operator -(Vector2 a, Size2I b) {
        return new(a.X - b.Width, a.Y - b.Height);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b) {
        return new(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2 operator *(Vector2 a, Vector2I b) {
        return new(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2 operator *(Vector2 v, float n) {
        return new(v.X * n, v.Y * n);
    }

    public static Vector2 operator *(float n, Vector2 v) {
        return new(n * v.X, n * v.Y);
    }

    public static Vector2 operator *(Vector2 a, Size2 b) {
        return new(a.X * b.Width, a.Y * b.Height);
    }

    public static Vector2 operator *(Vector2 a, Size2I b) {
        return new(a.X * b.Width, a.Y * b.Height);
    }

    public static Vector2 operator /(Vector2 a, Vector2 b) {
        return new(a.X / b.X, a.Y / b.Y);
    }

    public static Vector2 operator /(Vector2 v, float n) {
        return new(v.X / n, v.Y / n);
    }

    public static Vector2 operator /(float n, Vector2 v) {
        return new(n / v.X, n / v.Y);
    }

    public static Vector2 operator /(Vector2 v, Size2 s) {
        return new(v.X / s.Width, v.Y / s.Height);
    }

    public static Vector2 operator /(Vector2 v, Size2I s) {
        return new(v.X / s.Width, v.Y / s.Height);
    }

    public static Vector2 operator %(Vector2 a, Vector2 b) {
        return new(a.X % b.X, a.Y % b.Y);
    }

    public static Vector2 operator %(Vector2 v, Size2 s) {
        return new(v.X % s.Width, v.Y % s.Height);
    }

    public static bool operator ==(Vector2 a, Vector2 b) {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2 a, Vector2 b) {
        return a.X != b.X || a.Y != b.Y;
    }

    internal Xna.Vector2 ToXna() {
        return new(X, Y);
    }
}
