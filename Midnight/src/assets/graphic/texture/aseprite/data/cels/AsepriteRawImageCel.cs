
namespace Midnight.Assets.Aseprite;

public class AsepriteRawImageCel : AsepriteCel {
    internal AsepriteRawImageCel(AsepriteData data) : base(data) {
    }

    public Texture2D Texture { get; set; }
}
