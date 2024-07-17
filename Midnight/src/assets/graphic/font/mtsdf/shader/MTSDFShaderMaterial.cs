namespace Midnight;

public record class MTSDFShaderMaterial(MTSDFShader Shader)
    : ShaderMaterial<MTSDFShader>(Shader),
      IColorUniform
{
    public float ScreenPixelRange { get; set; }
    public float DistanceRange { get; set; }

    public ColorF? ColorF { get; set; }
    public float? Rounding { get; set; }

    public ColorF? InnerOutlineColor { get; set; }
    public ColorF? OuterOutlineColor { get; set; }
    public float? RawInnerOutlineThickness { get; set; }
    public float? RawOuterOutlineThickness { get; set; }

    public float? InnerOutlineThickness {
        get {
            if (!RawInnerOutlineThickness.HasValue) {
                return null;
            }

            return Math.Map(RawInnerOutlineThickness.Value, 0.0f, 2.5f, 0.0f, 5.0f);
        }
        set {
            if (!value.HasValue) {
                RawInnerOutlineThickness = null;
                return;
            }

            RawInnerOutlineThickness = Math.Map(value.Value, 0.0f, 5.0f, 0.0f, 2.5f);
        }
    }

    public float? OuterOutlineThickness {
        get {
            if (!RawOuterOutlineThickness.HasValue) {
                return null;
            }

            return Math.Map(RawOuterOutlineThickness.Value, 0.0f, .48f, 0.0f, 5.0f);
        }
        set {
            if (!value.HasValue) {
                RawOuterOutlineThickness = null;
                return;
            }

            RawOuterOutlineThickness = Math.Map(value.Value, 0.0f, 5.0f, 0.0f, .48f);
        }
    }

    public float? OutlineRounding { get; set; }
    public ColorF? GlowColor { get; set; }
    public float? GlowLength { get; set; }
    public float? GlowIntensity { get; set; }
    public float? GlowRounding { get; set; }
    public Vector2? GlowDisplacement { get; set; }

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

        DistanceRange = font.Typesetting.Data.Atlas.DistanceRange;
    }

    public bool IsSafeFontScale(Font<MTSDF> font, float fontSize, float fontScale) {
        return CalculateScreenPixelRange(font, fontSize, fontScale) >= MTSDFShader.SafeMinScreenPixelRange;
    }

    protected override void Applied() {
        // mtsdf

        Shader.ScreenPixelRange = ScreenPixelRange;
        Shader.DistanceRange = DistanceRange;

        // font

        if (ColorF.HasValue) {
            Shader.ColorF = ColorF.Value;
        }

        if (Rounding.HasValue) {
            Shader.Rounding = Rounding.Value;
        }

        // outline

        if (InnerOutlineColor.HasValue) {
            Shader.InnerOutlineColor = InnerOutlineColor.Value;
        }

        if (OuterOutlineColor.HasValue) {
            Shader.OuterOutlineColor = OuterOutlineColor.Value;
        }

        if (RawInnerOutlineThickness.HasValue) {
            Shader.InnerOutlineThickness = RawInnerOutlineThickness.Value;
        }

        if (RawOuterOutlineThickness.HasValue) {
            Shader.OuterOutlineThickness = RawOuterOutlineThickness.Value;
        }

        if (OutlineRounding.HasValue) {
            Shader.OutlineRounding = OutlineRounding.Value;
        }

        // glow

        if (GlowColor.HasValue) {
            Shader.GlowColor = GlowColor.Value;
        }

        if (GlowLength.HasValue) {
            Shader.GlowLength = GlowLength.Value;
        }

        if (GlowIntensity.HasValue) {
            Shader.GlowIntensity = GlowIntensity.Value;
        }

        if (GlowRounding.HasValue) {
            Shader.GlowRounding = GlowRounding.Value;
        }

        if (GlowDisplacement.HasValue) {
            Shader.GlowDisplacement = GlowDisplacement.Value;
        }
    }

    protected override MTSDFShaderMaterial Duplicated() {
        return new(Shader) {
            ScreenPixelRange = ScreenPixelRange,
            DistanceRange = DistanceRange,
            ColorF = ColorF,
            Rounding = Rounding,
            InnerOutlineColor = InnerOutlineColor,
            OuterOutlineColor = OuterOutlineColor,
            InnerOutlineThickness = InnerOutlineThickness,
            OuterOutlineThickness = OuterOutlineThickness,
            OutlineRounding = OutlineRounding,
            GlowColor = GlowColor,
            GlowLength = GlowLength,
            GlowIntensity = GlowIntensity,
            GlowRounding = GlowRounding,
        };
    }

    private float CalculateScreenPixelRange(Font<MTSDF> font, float fontSize, float fontScale) {
        return ((fontScale * fontSize) / font.Typesetting.NominalWidth) * font.Typesetting.Data.Atlas.DistanceRange;
    }
}
