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

    protected ShaderParameter ScreenPixelRangeParam { get; private set; }

    public override MTSDFShaderMaterial CreateMaterial() {
        return new(this);
    }

    protected override void Loaded() {
        base.Loaded();
        ScreenPixelRangeParam = RetrieveParameter("ScreenPixelRange");

        // default values
        ScreenPixelRange = 0f;
    }
}
