
namespace Midnight;

public struct DrawableLayer {
    public int Value;

    public DrawableLayer(int value) {
        Value = value;
    }

    public DrawableDepthLayer ToDepth() {
        return new((Drawable.Layers / 2 - Value) / (float) Drawable.Layers);
    }

    public override string ToString() {
        return $"{Value} ({ToDepth().Value})";
    }

    public static implicit operator DrawableLayer(int value) {
        return new(value);
    }
}
