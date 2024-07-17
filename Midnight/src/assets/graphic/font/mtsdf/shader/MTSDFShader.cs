namespace Midnight;

public class MTSDFShader : BaseSpriteShader {
    /// <summary>
    /// Safe value to ensure font it's crispy.
    /// </summary>
    public const float SafeMinScreenPixelRange = 2.0f;

    public float ScreenPixelRange {
        get => ScreenPixelRangeParam.GetSingle();
        set => ScreenPixelRangeParam.Set(value);
    }

    public float DistanceRange {
        get => DistanceRangeParam.GetSingle();
        set => DistanceRangeParam.Set(value);
    }

    public float Rounding {
        get => RoundingParam.GetSingle();
        set => RoundingParam.Set(value);
    }

    public ColorF InnerOutlineColor {
        get => InnerOutlineColorParam.GetColorF();
        set => InnerOutlineColorParam.Set(value);
    }

    public ColorF OuterOutlineColor {
        get => OuterOutlineColorParam.GetColorF();
        set => OuterOutlineColorParam.Set(value);
    }

    public float InnerOutlineThickness {
        get => InnerOutlineThicknessParam.GetSingle();
        set => InnerOutlineThicknessParam.Set(value);
    }

    public float OuterOutlineThickness {
        get => OuterOutlineThicknessParam.GetSingle();
        set => OuterOutlineThicknessParam.Set(value);
    }

    public float OutlineRounding {
        get => OutlineRoundingParam.GetSingle();
        set => OutlineRoundingParam.Set(value);
    }

    public ColorF GlowColor {
        get => GlowColorParam.GetColorF();
        set => GlowColorParam.Set(value);
    }

    public float GlowLength {
        get => GlowLengthParam.GetSingle();
        set => GlowLengthParam.Set(value);
    }

    public float GlowIntensity {
        get => GlowIntensityParam.GetSingle();
        set => GlowIntensityParam.Set(value);
    }

    public float GlowRounding {
        get => GlowRoundingParam.GetSingle();
        set => GlowRoundingParam.Set(value);
    }

    public Vector2 GlowDisplacement {
        get => GlowDisplacementParam.GetVector2();
        set => GlowDisplacementParam.Set(value);
    }

    protected ShaderParameter ScreenPixelRangeParam { get; private set; }
    protected ShaderParameter DistanceRangeParam { get; private set; }
    protected ShaderParameter RoundingParam { get; private set; }
    protected ShaderParameter InnerOutlineColorParam { get; private set; }
    protected ShaderParameter OuterOutlineColorParam { get; private set; }
    protected ShaderParameter InnerOutlineThicknessParam { get; private set; }
    protected ShaderParameter OuterOutlineThicknessParam { get; private set; }
    protected ShaderParameter OutlineRoundingParam { get; private set; }
    protected ShaderParameter GlowColorParam { get; private set; }
    protected ShaderParameter GlowLengthParam { get; private set; }
    protected ShaderParameter GlowIntensityParam { get; private set; }
    protected ShaderParameter GlowRoundingParam { get; private set; }
    protected ShaderParameter GlowDisplacementParam { get; private set; }

    public override MTSDFShaderMaterial CreateMaterial() {
        return new(this);
    }

    protected override void Loaded() {
        base.Loaded();
        ScreenPixelRangeParam = RetrieveParameter("u_ScreenPixelRange");
        DistanceRangeParam = RetrieveParameter("u_DistanceRange");
        RoundingParam = RetrieveParameter("u_Rounding");
        InnerOutlineColorParam = RetrieveParameter("u_InnerOutlineColor");
        OuterOutlineColorParam = RetrieveParameter("u_OuterOutlineColor");
        InnerOutlineThicknessParam = RetrieveParameter("u_InnerOutlineThickness");
        OuterOutlineThicknessParam = RetrieveParameter("u_OuterOutlineThickness");
        OutlineRoundingParam = RetrieveParameter("u_OutlineRounding");
        GlowColorParam = RetrieveParameter("u_GlowColor");
        GlowLengthParam = RetrieveParameter("u_GlowLength");
        GlowIntensityParam = RetrieveParameter("u_GlowIntensity");
        GlowRoundingParam = RetrieveParameter("u_GlowRounding");
        GlowDisplacementParam = RetrieveParameter("u_GlowDisplacement");

        // default values
        ScreenPixelRange = 0f;
        DistanceRange = 0f;
        Rounding = 0f;
        InnerOutlineColor = ColorF.White;
        OuterOutlineColor = ColorF.White;
        InnerOutlineThickness = 0f;
        OuterOutlineThickness = 0f;
        OutlineRounding = 0f;
        GlowColor = ColorF.Black;
        GlowLength = 0f;
        GlowIntensity = 0f;
        GlowRounding = 1f;
        GlowDisplacement = Vector2.Zero;
    }
}
