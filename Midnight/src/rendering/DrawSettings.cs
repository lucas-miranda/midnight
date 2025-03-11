using System.Diagnostics.CodeAnalysis;

namespace Midnight;

public struct DrawSettings : System.IEquatable<DrawSettings> {
    public static readonly DrawSettings Default = new();

    public DrawSettings() {
        Blend = BlendState.AlphaBlend;
        Samplers = new SamplerState[] { SamplerState.PointClamp };
        DepthStencil = DepthStencilState.Default;
        Rasterizer = RasterizerState.CullNone;
        Primitive = PrimitiveType.TriangleList;
        WorldViewProjection = null;
        Immediate = false;
    }

    public BlendState Blend { get; init; }
    public SamplerState[] Samplers { get; init; }
    public DepthStencilState DepthStencil { get; init; }
    public RasterizerState Rasterizer { get; init; }
    public PrimitiveType Primitive { get; init; }
    public Matrix? WorldViewProjection { get; init; }

    /// <summary>
    /// Should it be submmitted as immediate?
    /// It'll break current batching and never be grouped with any other thing.
    /// </summary>
    public bool Immediate { get; init; }

    internal void Apply() {
        var xnaGD = RenderingServer.XnaGraphicsDevice;
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

        return Blend.Equals(s.Blend)
            && DepthStencil.Equals(s.DepthStencil)
            && Rasterizer.Equals(s.Rasterizer)
            && Primitive.Equals(s.Primitive)
            && Immediate.Equals(s.Immediate)
            && WorldViewProjection.Equals(s.WorldViewProjection);
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is DrawSettings s && Equals(s);
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
            hashCode = hashCode * 1610612741 + Immediate.GetHashCode();

            if (WorldViewProjection != null) {
                hashCode = hashCode * 1610612741 + WorldViewProjection.GetHashCode();
            }
        }

        return hashCode;
    }
}
