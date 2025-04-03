
namespace Midnight;

public struct DrawableDepthLayer {
    public float Value;

    public DrawableDepthLayer(float value) {
        Value = value;
    }

    public DrawableLayer ToLayer() {
        return new((Drawable.Layers / 2) - (int) (Value * Drawable.Layers));
    }

    public override string ToString() {
        return $"{Value} ({ToLayer().Value})";
    }
}
