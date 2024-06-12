using System.Diagnostics.CodeAnalysis;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public struct BlendState : System.IEquatable<BlendState> {
    public static readonly BlendState
            Additive = new(XnaGraphics.BlendState.Additive),
            AlphaBlend = new(XnaGraphics.BlendState.AlphaBlend),
            NonPremultiplied = new(XnaGraphics.BlendState.NonPremultiplied),
            Opaque = new(XnaGraphics.BlendState.Opaque);

    public BlendState() {
        Underlying = new();
    }

    public BlendState(Blend colorSourceBlend, Blend alphaSourceBlend, Blend colorDestBlend, Blend alphaDestBlend) : this() {
        ColorSourceBlend = colorDestBlend;
        AlphaSourceBlend = alphaSourceBlend;
        ColorDestinationBlend = colorDestBlend;
        AlphaDestinationBlend = alphaDestBlend;
    }

    internal BlendState(XnaGraphics.BlendState xnaBlendState) {
        Underlying = xnaBlendState;
    }

    public BlendFunction AlphaBlendFunction {
        get => (BlendFunction) Underlying.AlphaBlendFunction;
        set => Underlying.AlphaBlendFunction = value.ToXna();
    }

    public Blend AlphaDestinationBlend {
        get => (Blend) Underlying.AlphaDestinationBlend;
        set => Underlying.AlphaDestinationBlend = value.ToXna();
    }

    public Blend AlphaSourceBlend {
        get => (Blend) Underlying.AlphaSourceBlend;
        set => Underlying.AlphaSourceBlend = value.ToXna();
    }

    public BlendFunction ColorBlendFunction {
        get => (BlendFunction) Underlying.ColorBlendFunction;
        set => Underlying.ColorBlendFunction = value.ToXna();
    }

    public Blend ColorDestinationBlend {
        get => (Blend) Underlying.ColorDestinationBlend;
        set => Underlying.ColorDestinationBlend = value.ToXna();
    }

    public Blend ColorSourceBlend {
        get => (Blend) Underlying.ColorSourceBlend;
        set => Underlying.ColorSourceBlend = value.ToXna();
    }

    public ColorWriteChannels ColorWriteChannels {
        get => (ColorWriteChannels) Underlying.ColorWriteChannels;
        set => Underlying.ColorWriteChannels = value.ToXna();
    }

    public ColorWriteChannels ColorWriteChannels1 {
        get => (ColorWriteChannels) Underlying.ColorWriteChannels1;
        set => Underlying.ColorWriteChannels1 = value.ToXna();
    }

    public ColorWriteChannels ColorWriteChannels2 {
        get => (ColorWriteChannels) Underlying.ColorWriteChannels2;
        set => Underlying.ColorWriteChannels2 = value.ToXna();
    }

    public ColorWriteChannels ColorWriteChannels3 {
        get => (ColorWriteChannels) Underlying.ColorWriteChannels3;
        set => Underlying.ColorWriteChannels3 = value.ToXna();
    }

    public Color BlendFactor {
        get => new(Underlying.BlendFactor);
        set => Underlying.BlendFactor = value.ToXna();
    }

    public int MultiSampleMask {
        get => Underlying.MultiSampleMask;
        set => Underlying.MultiSampleMask = value;
    }

    internal XnaGraphics.BlendState Underlying { get; }

    public bool Equals(BlendState s) {
        return !(AlphaBlendFunction != s.AlphaBlendFunction
            || AlphaDestinationBlend != s.AlphaDestinationBlend
            || AlphaSourceBlend != s.AlphaSourceBlend
            || ColorBlendFunction != s.ColorBlendFunction
            || ColorDestinationBlend != s.ColorDestinationBlend
            || ColorSourceBlend != s.ColorSourceBlend
            || ColorWriteChannels != s.ColorWriteChannels
            || ColorWriteChannels1 != s.ColorWriteChannels1
            || ColorWriteChannels2 != s.ColorWriteChannels2
            || ColorWriteChannels3 != s.ColorWriteChannels3
            || BlendFactor != s.BlendFactor
            || MultiSampleMask != s.MultiSampleMask);
    }

    public override bool Equals([NotNullWhen(true)] object obj) {
        return obj is BlendState s && Equals(s);
    }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + AlphaBlendFunction.GetHashCode();
            hashCode = hashCode * 1610612741 + AlphaDestinationBlend.GetHashCode();
            hashCode = hashCode * 1610612741 + AlphaSourceBlend.GetHashCode();
            hashCode = hashCode * 1610612741 + ColorBlendFunction.GetHashCode();
            hashCode = hashCode * 1610612741 + ColorDestinationBlend.GetHashCode();
            hashCode = hashCode * 1610612741 + ColorSourceBlend.GetHashCode();
            hashCode = hashCode * 1610612741 + ColorWriteChannels.GetHashCode();
            hashCode = hashCode * 1610612741 + ColorWriteChannels1.GetHashCode();
            hashCode = hashCode * 1610612741 + ColorWriteChannels2.GetHashCode();
            hashCode = hashCode * 1610612741 + ColorWriteChannels3.GetHashCode();
            hashCode = hashCode * 1610612741 + BlendFactor.GetHashCode();
            hashCode = hashCode * 1610612741 + MultiSampleMask.GetHashCode();
        }

        return hashCode;
    }
}
