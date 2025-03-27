using Midnight.Diagnostics;

namespace Midnight.Assets.Aseprite;

public abstract class AsepriteCel {
    internal AsepriteCel(AsepriteData data) {
        Assert.NotNull(data);
        Data = data;
    }

    public AsepriteData Data { get; }
    public AsepriteLayer Layer { get; set; }
    public Vector2I Position { get; set; }
    public Vector2 PrecisePosition { get; set; }
    public Size2 Size { get; set; }
    public byte Opacity { get; set; }
    public short ZIndex { get; set; }
    public AsepriteCelFlags Flags { get; set; }

    public override string ToString() {
        return $"{GetType()} Pos: {Position}; PrecisePos: {PrecisePosition}; CelSize: {Size}; Opacity: {Opacity}; ZIndex: {ZIndex}; Flags: {new BitTag(Flags)}";
    }
}
