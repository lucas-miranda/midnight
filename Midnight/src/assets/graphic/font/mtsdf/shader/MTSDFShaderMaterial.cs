namespace Midnight;

public record class MTSDFShaderMaterial(MTSDFShader Shader)
    : ShaderMaterial<MTSDFShader>(Shader),
      IColorUniform
{
    public ColorF? ColorF { get; set; }
    public float ScreenPixelRange { get; set; }

    public void ChangeScreenPixelRange(
        Font<MTSDF> font,
        float? fontSize = null,
        float fontScale = 1.0f
    ) {
        ScreenPixelRange = CalculateScreenPixelRange(
                               font,
                               fontSize.GetValueOrDefault(font.Size),
                               fontScale
                           );
    }

    public bool IsSafeFontScale(Font<MTSDF> font, float fontSize, float fontScale) {
        return CalculateScreenPixelRange(font, fontSize, fontScale) >= MTSDFShader.SafeMinScreenPixelRange;
    }

    protected override void Applied() {
        if (ColorF.HasValue) {
            Shader.ColorF = ColorF.Value;
        }

        Shader.ScreenPixelRange = ScreenPixelRange;
    }

    protected override MTSDFShaderMaterial Duplicated() {
        return new(Shader) {
            ColorF = ColorF,
            ScreenPixelRange = ScreenPixelRange,
        };
    }

    private float CalculateScreenPixelRange(Font<MTSDF> font, float fontSize, float fontScale) {
        return ((fontScale * fontSize) / font.Typesetting.NominalWidth) * font.Typesetting.Data.Atlas.DistanceRange;
    }
}
