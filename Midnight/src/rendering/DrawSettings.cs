using System.Diagnostics.CodeAnalysis;

namespace Midnight;

public struct DrawSettings : System.IEquatable<DrawSettings> {
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

    public bool Equals(DrawSettings s) {
        if (Samplers.Length != s.Samplers.Length) {
            return false;
        }

        for (int i = 0; i < Samplers.Length; i++) {
            if (!Samplers[i].Equals(s.Samplers[i])) {
                return false;
            }
        }

        return !(Blend.Equals(s.Blend)
            || DepthStencil.Equals(s.DepthStencil)
            || Rasterizer.Equals(s.Rasterizer)
            || Primitive.Equals(s.Primitive));
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is RasterizerState s && Equals(s);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + Blend.GetHashCode();

            for (int i = 0; i < Samplers.Length; i++) {
                hashCode = hashCode * 1610612741 + Samplers[i].GetHashCode();
            }

            hashCode = hashCode * 1610612741 + DepthStencil.GetHashCode();
            hashCode = hashCode * 1610612741 + Rasterizer.GetHashCode();
            hashCode = hashCode * 1610612741 + Primitive.GetHashCode();
        }

        return hashCode;
    }
}
