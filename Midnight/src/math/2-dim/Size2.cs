using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Size2 : System.IEquatable<Size2> {
    public static readonly Size2
            Empty = new(0.0f, 0.0f),
            Zero = Empty,
            One = new(1.0f, 1.0f),
            UnitWidth = new(1.0f, 0.0f),
            UnitHeight = new(0.0f, 1.0f);

    public float Width, Height;

    public Size2(float w, float h) {
        Width = Math.Max(0.0f, w);
        Height = Math.Max(0.0f, h);
    }

    public Size2(float wh) : this(wh, wh) {
    }

    public Size2(Vector2 vec2) : this(vec2.X, vec2.Y) {
    }

    internal Size2(Xna.Vector2 xnaVector2) : this(xnaVector2.X, xnaVector2.Y) {
    }

    public static Size2 Between(Vector2 a, Vector2 b) {
        Vector2 diff = b - a;
        return new(Math.Abs(diff.X), Math.Abs(diff.Y));
    }

    public static Size2 MaxComponents(Size2 a, Size2 b) {
        return new(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
    }

    public float Length() {
        return Math.Sqrt(Width * Width + Height * Height);
    }

    public bool ApproxEquals(Size2 s) {
        return Math.ApproxEquals(Width, s.Width)
            && Math.ApproxEquals(Height, s.Height);
    }

    public bool ApproxZero() {
        return ApproxEquals(Zero);
    }

    public bool IsEmpty() {
        return Math.ApproxEquals(Width, Zero.Width)
            || Math.ApproxEquals(Height, Zero.Height);
    }

    public bool Equals(Size2 s) {
        return Width == s.Width && Height == s.Height;
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Size2 v && Equals(v);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + Width.GetHashCode();
            hashCode = hashCode * 1610612741 + Height.GetHashCode();
        }

        return hashCode;
    }

    public Vector2 ToVector2() {
        return new(Width, Height);
    }

    public Size2I ToInt() {
        return new((int) Width, (int) Height);
    }

    public override string ToString() {
        return $"{Width} x {Height}";
    }

    public static Size2 operator +(Size2 a, Size2 b) {
        return new(a.Width + b.Width, a.Height + b.Height);
    }

    public static Size2 operator +(Size2 s, float n) {
        return new(s.Width + n, s.Height + n);
    }

    public static Size2 operator +(float n, Size2 s) {
        return new(n + s.Width, n + s.Height);
    }

    public static Size2 operator +(Size2 a, Size2I b) {
        return new(a.Width + b.Width, a.Height + b.Height);
    }

    public static Size2 operator -(Size2 a, Size2 b) {
        return new(a.Width - b.Width, a.Height - b.Height);
    }

    public static Size2 operator -(Size2 s, float n) {
        return new(s.Width - n, s.Width - n);
    }

    public static Size2 operator -(float n, Size2 s) {
        return new(n - s.Width, n - s.Height);
    }

    public static Size2 operator -(Size2 a, Size2I b) {
        return new(a.Width - b.Width, a.Height - b.Height);
    }

    public static Size2 operator *(Size2 a, Size2 b) {
        return new(a.Width * b.Width, a.Height * b.Height);
    }

    public static Size2 operator *(Size2 s, float n) {
        return new(s.Width * n, s.Height * n);
    }

    public static Size2 operator *(float n, Size2 s) {
        return new(n * s.Width, n * s.Height);
    }

    public static Size2 operator *(Size2 a, Size2I b) {
        return new(a.Width * b.Width, a.Height * b.Height);
    }

    public static Size2 operator /(Size2 a, Size2 b) {
        return new(a.Width / b.Width, a.Height / b.Height);
    }

    public static Size2 operator /(Size2 s, float n) {
        return new(s.Width / n, s.Height / n);
    }

    public static Size2 operator /(float n, Size2 s) {
        return new(n / s.Width, n / s.Height);
    }

    public static Size2 operator /(Size2 a, Size2I b) {
        return new(a.Width / b.Width, a.Height / b.Height);
    }

    public static bool operator ==(Size2 a, Size2 b) {
        return a.Width == b.Width && a.Height == b.Height;
    }

    public static bool operator !=(Size2 a, Size2 b) {
        return a.Width != b.Width || a.Height != b.Height;
    }
}
