using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct ColorF : System.IEquatable<ColorF> {
    public static readonly ColorF
            White = Color.White.Normalized(),
            Black = Color.Black.Normalized(),
            Red = Color.Red.Normalized(),
            Green = Color.Green.Normalized(),
            Blue = Color.Blue.Normalized(),
            Magenta = Color.Magenta.Normalized(),
            Cyan = Color.Cyan.Normalized(),
            Yellow = Color.Yellow.Normalized(),
            Transparent = Color.Transparent.Normalized(),
            TransparentBlack = Color.TransparentBlack.Normalized();

    public float R, G, B, A;

    public ColorF() {
        R = G = B = A = 1.0f;
    }

    public ColorF(float r, float g, float b, float a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public ColorF(float r, float g, float b) : this(r, g, b, 1.0f) {
    }

    public ColorF(float rgb, float a) : this(rgb, rgb, rgb, a) {
    }

    public ColorF(float rgba) : this(rgba, rgba, rgba, rgba) {
    }

    public ColorF(Vector4 vec) : this(vec.X, vec.Y, vec.Z, vec.W) {
    }

    public Color ToByte() {
        return new(
            (byte) Math.Floor((float) byte.MaxValue * Math.Clamp01(R)),
            (byte) Math.Floor((float) byte.MaxValue * Math.Clamp01(G)),
            (byte) Math.Floor((float) byte.MaxValue * Math.Clamp01(B)),
            (byte) Math.Floor((float) byte.MaxValue * Math.Clamp01(A))
        );
    }

    public Vector4 ToVec4() {
        return new(R, G, B, A);
    }

    public bool Equals(ColorF c) {
        return !(R != c.R || G != c.G || B != c.B || A != c.A);
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is ColorF v && Equals(v);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + R.GetHashCode();
            hashCode = hashCode * 1610612741 + G.GetHashCode();
            hashCode = hashCode * 1610612741 + B.GetHashCode();
            hashCode = hashCode * 1610612741 + A.GetHashCode();
        }

        return hashCode;
    }

    public override string ToString() {
        return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F2};{1:F2};{2:F2};{3:F2};", R, G, B, A);
    }

    public static implicit operator ColorF(int n) {
        return new Color((uint) n).Normalized();
    }

    public static implicit operator ColorF(uint n) {
        return new Color(n).Normalized();
    }

    public static bool operator ==(ColorF a, ColorF b) {
        return !(a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
    }

    public static bool operator !=(ColorF a, ColorF b) {
        return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
    }

    public static ColorF operator +(ColorF a, ColorF b) {
        return new(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);
    }

    public static ColorF operator +(ColorF a, Color b) {
        return new(
            a.R + (b.R / (float) byte.MaxValue),
            a.G + (b.G / (float) byte.MaxValue),
            a.B + (b.B / (float) byte.MaxValue),
            a.A + (b.A / (float) byte.MaxValue)
        );
    }

    public static ColorF operator +(Color a, ColorF b) {
        return new(
            (a.R / (float) byte.MaxValue) + b.R,
            (a.G / (float) byte.MaxValue) + b.G,
            (a.B / (float) byte.MaxValue) + b.B,
            (a.A / (float) byte.MaxValue) + b.A
        );
    }

    public static ColorF operator -(ColorF a, ColorF b) {
        return new(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);
    }

    public static ColorF operator -(ColorF a, Color b) {
        return new(
            a.R - (b.R / (float) byte.MaxValue),
            a.G - (b.G / (float) byte.MaxValue),
            a.B - (b.B / (float) byte.MaxValue),
            a.A - (b.A / (float) byte.MaxValue)
        );
    }

    public static ColorF operator -(Color a, ColorF b) {
        return new(
            (a.R / (float) byte.MaxValue) - b.R,
            (a.G / (float) byte.MaxValue) - b.G,
            (a.B / (float) byte.MaxValue) - b.B,
            (a.A / (float) byte.MaxValue) - b.A
        );
    }

    public static ColorF operator *(ColorF a, ColorF b) {
        return new(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A);
    }

    public static ColorF operator *(ColorF a, Color b) {
        return new(
            a.R * (b.R / (float) byte.MaxValue),
            a.G * (b.G / (float) byte.MaxValue),
            a.B * (b.B / (float) byte.MaxValue),
            a.A * (b.A / (float) byte.MaxValue)
        );
    }

    public static ColorF operator *(Color a, ColorF b) {
        return new(
            (a.R / (float) byte.MaxValue) * b.R,
            (a.G / (float) byte.MaxValue) * b.G,
            (a.B / (float) byte.MaxValue) * b.B,
            (a.A / (float) byte.MaxValue) * b.A
        );
    }

    public static ColorF operator /(ColorF a, ColorF b) {
        return new(a.R / b.R, a.G / b.G, a.B / b.B, a.A / b.A);
    }

    public static ColorF operator /(ColorF a, Color b) {
        return new(
            a.R / (b.R / (float) byte.MaxValue),
            a.G / (b.G / (float) byte.MaxValue),
            a.B / (b.B / (float) byte.MaxValue),
            a.A / (b.A / (float) byte.MaxValue)
        );
    }

    public static ColorF operator /(Color a, ColorF b) {
        return new(
            (a.R / (float) byte.MaxValue) / b.R,
            (a.G / (float) byte.MaxValue) / b.G,
            (a.B / (float) byte.MaxValue) / b.B,
            (a.A / (float) byte.MaxValue) / b.A
        );
    }

    internal Xna.Color ToXna() {
        return new(R, G, B, A);
    }
}
