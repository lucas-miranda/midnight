namespace Midnight;

public struct RectangleD {
    public double X, Y, Width, Height;

    public RectangleD(double x, double y, double width, double height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectangleI ToInt() {
        return new((int) X, (int) Y, (int) Width, (int) Height);
    }
}
