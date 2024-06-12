using System.Diagnostics.CodeAnalysis;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Color : System.IEquatable<Color> {
    public static readonly Color
            White = new(0xFFFFFFFF),
            Black = new(0x000000FF),
            Red = new(0xFF0000FF),
            Green = new(0x00FF00FF),
            Blue = new(0x0000FFFF),
            Magenta = new(0xFF00FFFF),
            Cyan = new(0x00FFFFFF),
            Yellow = new(0xFFFF00FF),
            Transparent = new(0xFFFFFF00),
            TransparentBlack = new(0x00000000);

    public byte R, G, B, A;

    public Color() {
        R = G = B = A = byte.MaxValue;
    }

    public Color(uint rgba) {
        R = System.Convert.ToByte((rgba & 0xFF000000) >> 24);
        G = System.Convert.ToByte((rgba & 0x00FF0000) >> 16);
        B = System.Convert.ToByte((rgba & 0x0000FF00) >> 8);
        A = System.Convert.ToByte(rgba & 0x000000FF);
    }

    public Color(uint rgb, uint a) : this((rgb << 8) | a) {
    }

    public Color(byte r, byte g, byte b, byte a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(byte r, byte g, byte b) : this(r, g, b, byte.MaxValue) {
    }

    public Color(int r, int g, int b, int a) {
        R = (byte) Math.Clamp(r, 0, byte.MaxValue);
        G = (byte) Math.Clamp(g, 0, byte.MaxValue);
        B = (byte) Math.Clamp(b, 0, byte.MaxValue);
        A = (byte) Math.Clamp(a, 0, byte.MaxValue);
    }

    public Color(int r, int g, int b) : this(r, g, b, byte.MaxValue) {
    }

    internal Color(Xna.Color color) {
        Color c = FromABGR(color.PackedValue);
        R = c.R;
        G = c.G;
        B = c.B;
        A = c.A;
    }

    public uint RGBA { get => (uint) ((R << 24) | (G << 16) | (B << 8) | A); }
    public uint ARGB { get => (uint) ((A << 24) | (R << 16) | (G << 8) | B); }

    public static Color FromARGB(uint argb) {
        return new(
            System.Convert.ToByte((argb & 0x00FF0000) >> 16),
            System.Convert.ToByte((argb & 0x0000FF00) >> 8),
            System.Convert.ToByte(argb & 0x000000FF),
            System.Convert.ToByte((argb & 0xFF000000) >> 24)
        );
    }

    public static Color FromABGR(uint abgr) {
        return new(
            System.Convert.ToByte(abgr & 0x000000FF),
            System.Convert.ToByte((abgr & 0x0000FF00) >> 8),
            System.Convert.ToByte((abgr & 0x00FF0000) >> 16),
            System.Convert.ToByte((abgr & 0xFF000000) >> 24)
        );
    }

    public ColorF Normalized() {
        return new(
            R / (float) byte.MaxValue,
            G / (float) byte.MaxValue,
            B / (float) byte.MaxValue,
            A / (float) byte.MaxValue
        );
    }

    public bool Equals(Color c) {
        return !(R != c.R || G != c.G || B != c.B || A != c.A);
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is Color v && Equals(v);
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
        return string.Format(System.Globalization.CultureInfo.InvariantCulture, "#{0:X2}{1:X2}{2:X2}{3:X2}", R, G, B, A);
    }

    public static implicit operator Color(int n) {
        return new((uint) n);
    }

    public static implicit operator Color(uint n) {
        return new(n);
    }

    public static bool operator ==(Color a, Color b) {
        return !(a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A);
    }

    public static bool operator !=(Color a, Color b) {
        return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
    }

    internal Xna.Color ToXna() {
        return new(R, G, B, A);
    }
}
