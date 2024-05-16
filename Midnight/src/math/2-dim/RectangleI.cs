using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct RectangleI {
    public int X, Y, Width, Height;

    public RectangleI(int x, int y, int width, int height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectangleI(int width, int height)
        : this(0, 0, width, height) {
    }

    public RectangleI(int wh)
        : this(0, 0, wh, wh) {
    }

    public Rectangle ToFloat() {
        return new(X, Y, Width, Height);
    }

    internal Xna.Rectangle ToXna() {
        return new(X, Y, Width, Height);
    }
}

public static class RectangleIExtensions {
    internal static Xna.Rectangle? ToXna(this RectangleI? r) {
        return r.HasValue ? r.Value.ToXna() : null;
    }
}
