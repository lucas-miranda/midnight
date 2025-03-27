using Midnight.Diagnostics;

namespace Midnight.Assets.Aseprite;

public class AsepriteLinkedCel : AsepriteCel {
    internal AsepriteLinkedCel(AsepriteData data) : base(data) {
    }

    public ushort FromFrame { get; set; }

    public AsepriteCel GetLinked() {
        Assert.InRange(FromFrame, 0, Data.Frames.Count - 1);
        return Layer.Cels[FromFrame];
    }

    public override string ToString() {
        return $"{base.ToString()}; FromFrame: {FromFrame}";
    }
}
