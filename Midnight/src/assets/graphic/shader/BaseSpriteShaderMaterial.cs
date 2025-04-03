namespace Midnight;

public record class BaseSpriteShaderMaterial(BaseSpriteShader Shader)
    : ShaderMaterial<BaseSpriteShader>(Shader),
      IColorUniform,
      ITextureUniform<Texture2D>,
      IWVPUniform
{
    public ColorF? ColorF { get; set; }
    public Texture2D Texture { get; set; }
    public Matrix? WorldViewProjection { get; set; }

    protected override void Applied() {
        if (ColorF.HasValue) {
            Shader.ColorF = ColorF.Value;
        }

        Shader.Texture = Texture;

        if (WorldViewProjection.HasValue) {
            Shader.WorldViewProjection = WorldViewProjection.Value;
        }
    }

    protected override BaseSpriteShaderMaterial Duplicated() {
        return new(Shader) {
            ColorF = ColorF,
            Texture = Texture,
            WorldViewProjection = WorldViewProjection,
        };
    }
}
