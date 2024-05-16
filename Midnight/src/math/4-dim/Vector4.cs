using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Vector4 {
    public float X, Y, Z, W;

    public Vector4(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vector4(float xyzw) : this(xyzw, xyzw, xyzw, xyzw) {
    }

    public float Length() {
        return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
    }

    public static Vector4 operator +(Vector4 a, Vector4 b) {
        return new(b.X + a.X, b.Y + a.Y, b.Z + a.Z, b.W + a.W);
    }

    public static Vector4 operator -(Vector4 a, Vector4 b) {
        return new(b.X - a.X, b.Y - a.Y, b.Z - a.Z, b.W - a.W);
    }

    internal Xna.Vector4 ToXna() {
        return new(X, Y, Z, W);
    }
}
