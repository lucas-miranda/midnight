namespace Midnight;

public record class SpriteShaderMaterial(BaseSpriteShader Shader)
    : ShaderMaterial<BaseSpriteShader>(Shader),
      IColorUniform,
      ITextureUniform<Texture2D>
{
    public ColorF ColorF { get; set; } = ColorF.White;
    public Texture2D Texture { get; set; }

    protected override void Applied() {
        Shader.ColorF = ColorF;
        Shader.Texture = Texture;
    }

    protected override SpriteShaderMaterial Duplicated() {
        return new(Shader) {
            ColorF = ColorF,
            Texture = Texture,
        };
    }
}
