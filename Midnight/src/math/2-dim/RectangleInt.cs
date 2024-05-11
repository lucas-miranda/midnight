using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct RectangleInt {
    public int X, Y, Width, Height;

    public RectangleInt(int x, int y, int width, int height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectangleInt(int width, int height)
        : this(0, 0, width, height) {
    }

    internal Xna.Rectangle ToXna() {
        return new(X, Y, Width, Height);
    }
}

public static class RectangleIntExtensions {
    internal static Xna.Rectangle? ToXna(this RectangleInt? r) {
        return r.HasValue ? r.Value.ToXna() : null;
    }
}
