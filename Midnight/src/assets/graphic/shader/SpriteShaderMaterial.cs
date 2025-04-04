namespace Midnight;

public record class SpriteShaderMaterial(SpriteShader Shader)
    : ShaderMaterial<SpriteShader>(Shader),
      IColorUniform,
      ITextureUniform<Texture2D>,
      IWVPUniform
{
    public ColorF? ColorF { get; set; }
    public Texture2D Texture { get; set; }
    public Matrix? WorldViewProjection { get; set; }
    public bool UseDepthBuffer { get; set; }

    protected override void Applied() {
        if (ColorF.HasValue) {
            Shader.ColorF = ColorF.Value;
        }

        Shader.Texture = Texture;

        if (WorldViewProjection.HasValue) {
            Shader.WorldViewProjection = WorldViewProjection.Value;
        }

        Shader.Settings = Shader.Settings with {
            DepthWriteEnabled = UseDepthBuffer,
        };
    }

    protected override SpriteShaderMaterial Duplicated() {
        return new(Shader) {
            ColorF = ColorF,
            Texture = Texture,
            WorldViewProjection = WorldViewProjection,
            UseDepthBuffer = UseDepthBuffer,
        };
    }
}
