using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public struct RectangleI {
    public Vector2I Position;
    public Size2I Size;

    public RectangleI(Vector2I position, Size2I size) {
        Position = position;
        Size = size;
    }

    public RectangleI(int x, int y, int width, int height)
        : this(new Vector2I(x, y), new Size2I(width, height))
    {
    }

    public RectangleI(Size2I size)
        : this(Vector2I.Zero, size)
    {
    }

    public RectangleI(int width, int height)
        : this(Vector2I.Zero, new Size2I(width, height))
    {
    }

    public RectangleI(int wh)
        : this(Vector2I.Zero, new Size2I(wh))
    {
    }

    public int X => Position.X;
    public int Y => Position.Y;
    public int Width => Size.Width;
    public int Height => Size.Height;
    public int Left => X;
    public int Top => Y;
    public int Right => X + Width;
    public int Bottom => Y + Height;
    public Vector2I TopLeft => Position;
    public Vector2I TopRight => new(Right, Top);
    public Vector2I BottomLeft => new(Left, Bottom);
    public Vector2I BottomRight => new(Right, Bottom);

    public Rectangle ToFloat() {
        return new(X, Y, Width, Height);
    }

    internal Xna.Rectangle ToXna() {
        return new(X, Y, Width, Height);
    }

    public static RectangleI operator /(RectangleI r, Size2I s) {
        return new(r.Position / s, r.Size / s);
    }

    public static Rectangle operator /(RectangleI r, Size2 s) {
        return new(r.Position / s, r.Size / s);
    }

    public static RectangleI operator *(RectangleI r, Size2I s) {
        return new(r.Position * s, r.Size * s);
    }

    public static Rectangle operator *(RectangleI r, Size2 s) {
        return new(r.Position * s, r.Size * s);
    }
}

public static class RectangleIExtensions {
    internal static Xna.Rectangle? ToXna(this RectangleI? r) {
        return r.HasValue ? r.Value.ToXna() : null;
    }
}
