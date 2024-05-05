namespace Midnight;

public struct Vector2 {
    public float X, Y;

    public Vector2(float x, float y) {
        X = x;
        Y = y;
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
}
