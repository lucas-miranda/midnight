
namespace Midnight.Assets.Aseprite;

public class AsepriteCompressedImageCel : AsepriteCel {
    internal AsepriteCompressedImageCel(AsepriteData data) : base(data) {
    }

    public Texture2D Texture { get; set; }

    public override string ToString() {
        if (Texture == null) {
            return $"{base.ToString()}; Missing Texture";
        }

        return $"{base.ToString()}; Texture: {Texture}";
    }
}
