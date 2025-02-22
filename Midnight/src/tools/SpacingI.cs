
namespace Midnight;

public struct SpacingI {
    public int Top, Right, Bottom, Left;

    public SpacingI(int top, int right, int bottom, int left) {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }

    public Spacing ToFloat() {
        return new(Top, Right, Bottom, Left);
    }

    public override string ToString() {
        return $"T: {Top}, R: {Right}, B: {Bottom}, L: {Left}";
    }
}
