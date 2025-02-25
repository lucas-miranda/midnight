using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Vector2I : System.IEquatable<Vector2I> {
    public static readonly Vector2I
            Zero = new(0, 0),
            One = new(1, 1),
            Up = new(0, -1),
            Right = new(1, 0),
            Down = new(0, 1),
            Left = new(-1, 0),
            UpRight = Up + Right,
            DownRight = Down + Right,
            DownLeft = Down + Left,
            UpLeft = Up + Left;

    public int X, Y;

    public Vector2I(int x, int y) {
        X = x;
        Y = y;
    }

    public Vector2I(int xy) : this(xy, xy) {
    }

    public int Dot(Vector2I v) {
        return X * v.X + Y * v.Y;
    }

    public Vector2 Normalized() {
        if (ApproxZero()) {
            return Vector2.Zero;
        }

        return this / Length();
    }

    public float Length() {
        return Math.Sqrt(X * X + Y * Y);
    }

    public bool ApproxEquals(Vector2I v) {
        return Math.ApproxEquals(X, v.X)
            && Math.ApproxEquals(Y, v.Y);
    }

    public bool ApproxZero() {
        return ApproxEquals(Zero);
    }

    public Vector2 ToFloat() {
        return new(X, Y);
    }

    public bool Equals(Vector2I v) {
        return X == v.X && Y == v.Y;
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Vector2I v && Equals(v);
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

    public static Vector2I operator -(Vector2I v) {
        return new(-v.X, -v.Y);
    }

    public static Vector2I operator +(Vector2I a, Vector2I b) {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator +(Vector2I a, Vector2 b) {
        return new(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2I operator +(Vector2I v, int n) {
        return new(v.X + n, v.Y + n);
    }

    public static Vector2 operator +(Vector2I v, float n) {
        return new(v.X + n, v.Y + n);
    }

    public static Vector2I operator +(int n, Vector2I v) {
        return new(n + v.X, n + v.Y);
    }

    public static Vector2 operator +(float n, Vector2I v) {
        return new(n + v.X, n + v.Y);
    }

    public static Vector2 operator +(Vector2I v, Size2 s) {
        return new(v.X + s.Width, v.Y + s.Height);
    }

    public static Vector2I operator +(Vector2I v, Size2I s) {
        return new(v.X + s.Width, v.Y + s.Height);
    }

    public static Vector2I operator -(Vector2I a, Vector2I b) {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator -(Vector2I a, Vector2 b) {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2I operator -(Vector2I v, int n) {
        return new(v.X - n, v.Y - n);
    }

    public static Vector2 operator -(Vector2I v, float n) {
        return new(v.X - n, v.Y - n);
    }

    public static Vector2I operator -(int n, Vector2I v) {
        return new(n - v.X, n - v.Y);
    }

    public static Vector2 operator -(float n, Vector2I v) {
        return new(n - v.X, n - v.Y);
    }

    public static Vector2 operator -(Vector2I a, Size2 b) {
        return new(a.X - b.Width, a.Y - b.Height);
    }

    public static Vector2I operator -(Vector2I a, Size2I b) {
        return new(a.X - b.Width, a.Y - b.Height);
    }

    public static Vector2I operator *(Vector2I a, Vector2I b) {
        return new(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2 operator *(Vector2I a, Vector2 b) {
        return new(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2I operator *(Vector2I v, int n) {
        return new(v.X * n, v.Y * n);
    }

    public static Vector2 operator *(Vector2I v, float n) {
        return new(v.X * n, v.Y * n);
    }

    public static Vector2I operator *(int n, Vector2I v) {
        return new(n * v.X, n * v.Y);
    }

    public static Vector2 operator *(float n, Vector2I v) {
        return new(n * v.X, n * v.Y);
    }

    public static Vector2 operator *(Vector2I a, Size2 b) {
        return new(a.X * b.Width, a.Y * b.Height);
    }

    public static Vector2I operator *(Vector2I a, Size2I b) {
        return new(a.X * b.Width, a.Y * b.Height);
    }

    public static Vector2I operator /(Vector2I a, Vector2I b) {
        return new(a.X / b.X, a.Y / b.Y);
    }

    public static Vector2I operator /(Vector2I v, int n) {
        return new(v.X / n, v.Y / n);
    }

    public static Vector2 operator /(Vector2I v, float n) {
        return new(v.X / n, v.Y / n);
    }

    public static Vector2I operator /(int n, Vector2I v) {
        return new(n / v.X, n / v.Y);
    }

    public static Vector2 operator /(float n, Vector2I v) {
        return new(n / v.X, n / v.Y);
    }

    public static Vector2 operator /(Vector2I v, Size2 s) {
        return new(v.X / s.Width, v.Y / s.Height);
    }

    public static Vector2I operator /(Vector2I v, Size2I s) {
        return new(v.X / s.Width, v.Y / s.Height);
    }

    public static bool operator ==(Vector2I a, Vector2I b) {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2I a, Vector2I b) {
        return a.X != b.X || a.Y != b.Y;
    }

    internal Xna.Vector2 ToXna() {
        return new(X, Y);
    }
}
