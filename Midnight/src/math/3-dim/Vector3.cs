using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Vector3 {
    public float X, Y, Z;

    public Vector3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(float xyz) : this(xyz, xyz, xyz) {
    }

    public float Length() {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public static Vector3 operator +(Vector3 a, Vector3 b) {
        return new(b.X + a.X, b.Y + a.Y, b.Z + a.Z);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b) {
        return new(b.X - a.X, b.Y - a.Y, b.Z - a.Z);
    }

    internal Xna.Vector3 ToXna() {
        return new(X, Y, Z);
    }
}
