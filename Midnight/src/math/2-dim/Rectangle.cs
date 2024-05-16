namespace Midnight;

public struct Rectangle {
    public float X, Y, Width, Height;

    public Rectangle(float x, float y, float width, float height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectangleI ToInt() {
        return new((int) X, (int) Y, (int) Width, (int) Height);
    }
}
