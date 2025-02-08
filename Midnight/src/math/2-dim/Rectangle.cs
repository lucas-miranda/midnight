namespace Midnight;

public struct Rectangle {
    public Vector2 Position;
    public Size2 Size;

    public Rectangle(Vector2 position, Size2 size) {
        Position = position;
        Size = size;
    }

    public Rectangle(Vector2 topLeft, Vector2 bottomRight) : this(
        topLeft,
        new Size2(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y)
    ) {
    }

    public Rectangle(float x, float y, float width, float height) : this (
        new Vector2(x, y),
        new Size2(width, height)
    ) {
    }

    public float X => Position.X;
    public float Y => Position.Y;
    public float Width => Size.Width;
    public float Height => Size.Height;
    public Vector2 TopLeft => Position;
    public Vector2 TopRight => new(X + Width, Y);
    public Vector2 BottomLeft => new(X, Y + Height);
    public Vector2 BottomRight => new(X + Width, Y + Height);

    public static Rectangle Enclose(Rectangle a, Rectangle b) {
        return new(
            new Vector2(Math.Min(a.TopLeft.X, b.TopLeft.X), Math.Min(a.TopLeft.Y, b.TopLeft.Y)),
            new Vector2(Math.Max(a.BottomRight.X, b.BottomRight.X), Math.Max(a.BottomRight.Y, b.BottomRight.Y))
        );
    }

    public RectangleI ToInt() {
        return new((int) X, (int) Y, (int) Width, (int) Height);
    }

    public override string ToString() {
        return $"{X}, {Y}, {Width} x {Height}";
    }
}
