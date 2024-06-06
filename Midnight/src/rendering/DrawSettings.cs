
namespace Midnight;

public record struct DrawSettings {
    public static readonly DrawSettings Default = new();

    public DrawSettings() {
        Blend = BlendState.AlphaBlend;
        Samplers = new SamplerState[] { SamplerState.PointClamp };
        DepthStencil = DepthStencilState.Default;
        Rasterizer = RasterizerState.CullNone;
    }

    public BlendState Blend { get; init; }
    public SamplerState[] Samplers { get; init; }
    public DepthStencilState DepthStencil { get; init; }
    public RasterizerState Rasterizer { get; init; }
    public PrimitiveType Primitive { get; init; }

    internal void Apply() {
        var xnaGD = Program.Rendering.XnaGraphicsDevice;
        xnaGD.BlendState = Blend.Underlying;

        if (Samplers != null) {
            for (int i = 0; i < Samplers.Length; i++) {
                xnaGD.SamplerStates[i] = Samplers[i].Underlying;
            }
        }

        xnaGD.DepthStencilState = DepthStencil.Underlying;
        xnaGD.RasterizerState = Rasterizer.Underlying;
    }
}
