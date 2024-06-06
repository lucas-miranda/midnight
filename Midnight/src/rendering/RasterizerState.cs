using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public struct RasterizerState {
    public static readonly RasterizerState
            CullCW = new(XnaGraphics.RasterizerState.CullClockwise),
            CullCCW = new(XnaGraphics.RasterizerState.CullCounterClockwise),
            CullNone = new(XnaGraphics.RasterizerState.CullNone);

    public RasterizerState() {
        Underlying = new();
    }

    public RasterizerState(CullMode cullMode) : this() {
        CullMode = cullMode;
    }

    internal RasterizerState(XnaGraphics.RasterizerState xnaRasterizerState) {
        Underlying = xnaRasterizerState;
    }

    public CullMode CullMode {
        get => (CullMode) Underlying.CullMode;
        set => Underlying.CullMode = value.ToXna();
    }

    public float DepthBias {
        get => Underlying.DepthBias;
        set => Underlying.DepthBias = value;
    }

    public FillMode FillMode {
        get => (FillMode) Underlying.FillMode;
        set => Underlying.FillMode = value.ToXna();
    }

    public bool MultiSampleAntiAlias {
        get => Underlying.MultiSampleAntiAlias;
        set => Underlying.MultiSampleAntiAlias = value;
    }

    public bool ScissorTestEnable {
        get => Underlying.ScissorTestEnable;
        set => Underlying.ScissorTestEnable = value;
    }

    public float SlopeScaleDepthBias {
        get => Underlying.SlopeScaleDepthBias;
        set => Underlying.SlopeScaleDepthBias = value;
    }

    internal XnaGraphics.RasterizerState Underlying { get; }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + CullMode.GetHashCode();
            hashCode = hashCode * 1610612741 + DepthBias.GetHashCode();
            hashCode = hashCode * 1610612741 + FillMode.GetHashCode();
            hashCode = hashCode * 1610612741 + MultiSampleAntiAlias.GetHashCode();
            hashCode = hashCode * 1610612741 + ScissorTestEnable.GetHashCode();
            hashCode = hashCode * 1610612741 + SlopeScaleDepthBias.GetHashCode();
        }

        return hashCode;
    }
}
