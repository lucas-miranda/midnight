using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Size2I : System.IEquatable<Size2I> {
    public static readonly Size2I
            Zero = new(0, 0),
            One = new(1, 1),
            UnitWidth = new(1, 0),
            UnitHeight = new(0, 1);

    public int Width, Height;

    public Size2I(int w, int h) {
        Width = Math.Max(0, w);
        Height = Math.Max(0, h);
    }

    public Size2I(int wh) : this(wh, wh) {
    }

    public Size2I(Vector2 vec2) : this((int) vec2.X, (int) vec2.Y) {
    }

    public Size2I(Vector2I vec2) : this(vec2.X, vec2.Y) {
    }

    internal Size2I(Xna.Vector2 xnaVector2) : this((int) xnaVector2.X, (int) xnaVector2.Y) {
    }

    public int Area => Width * Height;

    public float Length() {
        return Math.Sqrt(Width * Width + Height * Height);
    }

    public bool ApproxEquals(Size2I s) {
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

    public bool Equals(Size2I s) {
        return Width == s.Width && Height == s.Height;
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Size2I v && Equals(v);
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

    public Size2 ToFloat() {
        return new(Width, Height);
    }

    public override string ToString() {
        return $"{Width} x {Height}";
    }

    public static Size2I operator +(Size2I a, Size2I b) {
        return new(a.Width + b.Width, a.Height + b.Height);
    }

    public static Size2I operator +(Size2I s, int n) {
        return new(s.Width + n, s.Height + n);
    }

    public static Size2I operator +(int n, Size2I s) {
        return new(n + s.Width, n + s.Height);
    }

    public static Size2I operator -(Size2I a, Size2I b) {
        return new(a.Width - b.Width, a.Height - b.Height);
    }

    public static Size2I operator -(Size2I s, int n) {
        return new(s.Width - n, s.Width - n);
    }

    public static Size2I operator -(int n, Size2I s) {
        return new(n - s.Width, n - s.Height);
    }

    public static Size2I operator *(Size2I a, Size2I b) {
        return new(a.Width * b.Width, a.Height * b.Height);
    }

    public static Size2 operator *(Size2I a, Size2 b) {
        return new(a.Width * b.Width, a.Height * b.Height);
    }

    public static Size2I operator *(Size2I s, int n) {
        return new(s.Width * n, s.Height * n);
    }

    public static Size2 operator *(Size2I s, float n) {
        return new(s.Width * n, s.Height * n);
    }

    public static Size2I operator *(int n, Size2I s) {
        return new(n * s.Width, n * s.Height);
    }

    public static Size2 operator *(float n, Size2I s) {
        return new(n * s.Width, n * s.Height);
    }

    public static Size2I operator /(Size2I a, Size2I b) {
        return new(a.Width / b.Width, a.Height / b.Height);
    }

    public static Size2 operator /(Size2I a, Size2 b) {
        return new(a.Width / b.Width, a.Height / b.Height);
    }

    public static Size2I operator /(Size2I s, int n) {
        return new(s.Width / n, s.Height / n);
    }

    public static Size2 operator /(Size2I s, float n) {
        return new(s.Width / n, s.Height / n);
    }

    public static Size2I operator /(int n, Size2I s) {
        return new(n / s.Width, n / s.Height);
    }

    public static bool operator ==(Size2I a, Size2I b) {
        return a.Width == b.Width && a.Height == b.Height;
    }

    public static bool operator !=(Size2I a, Size2I b) {
        return a.Width != b.Width || a.Height != b.Height;
    }
}
