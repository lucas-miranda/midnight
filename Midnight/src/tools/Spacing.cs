
namespace Midnight;

public struct Spacing {
    public static readonly Spacing Empty = new(0);

    public float Top, Right, Bottom, Left;

    public Spacing(float top, float right, float bottom, float left) {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }

    public Spacing(float h, float v) : this(v, h, v, h) {
    }

    public Spacing(float all) : this(all, all, all, all) {
    }

    public SpacingI ToInt() {
        return new((int) Top, (int) Right, (int) Bottom, (int) Left);
    }

    public Vector2 TopLeft => new(Left, Top);
    public Vector2 TopRight => new(Right, Top);
    public Vector2 BottomLeft => new(Left, Bottom);
    public Vector2 BottomRight => new(Right, Bottom);
    public float Horizontal => Left + Right;
    public float Vertical => Top + Bottom;

    public override string ToString() {
        return $"T: {Top}, R: {Right}, B: {Bottom}, L: {Left}";
    }
}
