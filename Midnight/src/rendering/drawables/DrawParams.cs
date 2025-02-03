
namespace Midnight;

public struct DrawParams {
    public Transform2D Transform;
    public ShaderMaterial Material;
    public DrawSettings? DrawSettings;
    public Color Color;

    public DrawParams() {
        Color = Color.White;
    }
}
