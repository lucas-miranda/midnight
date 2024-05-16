using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct Vector2 {
    public static readonly Vector2
            Zero = new(0.0f, 0.0f),
            One = new(1.0f, 1.0f);

    public float X, Y;

    public Vector2(float x, float y) {
        X = x;
        Y = y;
    }

    public Vector2(float xy) : this(xy, xy) {
    }

    public float Length() {
        return Math.Sqrt(X * X + Y * Y);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b) {
        return new(b.X + a.X, b.Y + a.Y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b) {
        return new(b.X - a.X, b.Y - a.Y);
    }

    internal Xna.Vector2 ToXna() {
        return new(X, Y);
    }
}
