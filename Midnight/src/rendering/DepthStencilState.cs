using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public struct DepthStencilState {
    public static readonly DepthStencilState
            Default = new(XnaGraphics.DepthStencilState.Default),
            DepthRead = new(XnaGraphics.DepthStencilState.DepthRead),
            None = new(XnaGraphics.DepthStencilState.None);


    public DepthStencilState() {
        Underlying = new();
    }

    public DepthStencilState(bool depthBufferEnable, bool depthBufferWriteEnable) : this() {
        DepthBufferEnable = depthBufferEnable;
        DepthBufferWriteEnable = depthBufferWriteEnable;
    }

    internal DepthStencilState(XnaGraphics.DepthStencilState xnaDepthStencilState) {
        Underlying = xnaDepthStencilState;
    }

    public bool DepthBufferEnable {
        get => Underlying.DepthBufferEnable;
        set => Underlying.DepthBufferEnable = value;
    }

    public bool DepthBufferWriteEnable {
        get => Underlying.DepthBufferWriteEnable;
        set => Underlying.DepthBufferWriteEnable = value;
    }

    public StencilOperation CCWStencilDepthBufferFail {
        get => (StencilOperation) Underlying.CounterClockwiseStencilDepthBufferFail;
        set => Underlying.CounterClockwiseStencilDepthBufferFail = value.ToXna();
    }

    public StencilOperation CCWStencilFail {
        get => (StencilOperation) Underlying.CounterClockwiseStencilFail;
        set => Underlying.CounterClockwiseStencilFail = value.ToXna();
    }

    public CompareFunction CCWStencilFunction {
        get => (CompareFunction) Underlying.CounterClockwiseStencilFunction;
        set => Underlying.CounterClockwiseStencilFunction = value.ToXna();
    }

    public StencilOperation CCWStencilPass {
        get => (StencilOperation) Underlying.CounterClockwiseStencilPass;
        set => Underlying.CounterClockwiseStencilPass = value.ToXna();
    }

    public CompareFunction DepthBufferFunction {
        get => (CompareFunction) Underlying.DepthBufferFunction;
        set => Underlying.DepthBufferFunction = value.ToXna();
    }

    public int ReferenceStencil {
        get => Underlying.ReferenceStencil;
        set => Underlying.ReferenceStencil = value;
    }

    public StencilOperation StencilDepthBufferFail {
        get => (StencilOperation) Underlying.StencilDepthBufferFail;
        set => Underlying.StencilDepthBufferFail = value.ToXna();
    }

    public bool StencilEnable {
        get => Underlying.StencilEnable;
        set => Underlying.StencilEnable = value;
    }

    public StencilOperation StencilFail {
        get => (StencilOperation) Underlying.StencilFail;
        set => Underlying.StencilFail = value.ToXna();
    }

    public CompareFunction StencilFunction {
        get => (CompareFunction) Underlying.StencilFunction;
        set => Underlying.StencilFunction = value.ToXna();
    }

    public int StencilMask {
        get => Underlying.StencilMask;
        set => Underlying.StencilMask = value;
    }

    public StencilOperation StencilPass {
        get => (StencilOperation) Underlying.StencilPass;
        set => Underlying.StencilPass = value.ToXna();
    }

    public int StencilWriteMask {
        get => Underlying.StencilWriteMask;
        set => Underlying.StencilWriteMask = value;
    }

    public bool TwoSidedStencilMode {
        get => Underlying.TwoSidedStencilMode;
        set => Underlying.TwoSidedStencilMode = value;
    }

    internal XnaGraphics.DepthStencilState Underlying { get; }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + DepthBufferEnable.GetHashCode();
            hashCode = hashCode * 1610612741 + DepthBufferWriteEnable.GetHashCode();
            hashCode = hashCode * 1610612741 + CCWStencilDepthBufferFail.GetHashCode();
            hashCode = hashCode * 1610612741 + CCWStencilFail.GetHashCode();
            hashCode = hashCode * 1610612741 + CCWStencilFunction.GetHashCode();
            hashCode = hashCode * 1610612741 + CCWStencilPass.GetHashCode();
            hashCode = hashCode * 1610612741 + DepthBufferFunction.GetHashCode();
            hashCode = hashCode * 1610612741 + ReferenceStencil.GetHashCode();
            hashCode = hashCode * 1610612741 + StencilDepthBufferFail.GetHashCode();
            hashCode = hashCode * 1610612741 + StencilFail.GetHashCode();
            hashCode = hashCode * 1610612741 + StencilFunction.GetHashCode();
            hashCode = hashCode * 1610612741 + StencilMask.GetHashCode();
            hashCode = hashCode * 1610612741 + StencilPass.GetHashCode();
            hashCode = hashCode * 1610612741 + StencilWriteMask.GetHashCode();
            hashCode = hashCode * 1610612741 + TwoSidedStencilMode.GetHashCode();
        }

        return hashCode;
    }
}
