using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct ColorF : System.IEquatable<ColorF> {
    public float R, G, B, A;

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
            hashCode = hashCode * 23 + R.GetHashCode();
            hashCode = hashCode * 23 + G.GetHashCode();
            hashCode = hashCode * 23 + B.GetHashCode();
            hashCode = hashCode * 23 + A.GetHashCode();
        }

        return hashCode;
    }

    public override string ToString() {
        return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F2};{1:F2};{2:F2};{3:F2};", R, G, B, A);
    }

    public static bool operator ==(ColorF a, ColorF b) {
        return !(a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
    }

    public static bool operator !=(ColorF a, ColorF b) {
        return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
    }

    internal Xna.Color ToXna() {
        return new(R, G, B, A);
    }
}
