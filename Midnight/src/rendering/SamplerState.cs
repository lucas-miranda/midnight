using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public struct SamplerState {
    public static readonly SamplerState
            AnisotropicClamp = new(XnaGraphics.SamplerState.AnisotropicClamp),
            AnisotropicWrap = new(XnaGraphics.SamplerState.AnisotropicWrap),
            LinearClamp = new(XnaGraphics.SamplerState.LinearClamp),
            LinearWrap = new(XnaGraphics.SamplerState.LinearWrap),
            PointClamp = new(XnaGraphics.SamplerState.PointClamp),
            PointWrap = new(XnaGraphics.SamplerState.PointWrap);

    public SamplerState() {
        Underlying = new();
    }

    public SamplerState(
        TextureFilter filter,
        TextureAddressMode addressU,
        TextureAddressMode addressV,
        TextureAddressMode addressW
    ) : this() {
        Filter = filter;
        AddressU = addressU;
        AddressV = addressV;
        AddressW = addressW;
    }

    internal SamplerState(XnaGraphics.SamplerState xnaSamplerState) {
        Underlying = xnaSamplerState;
    }

    public TextureAddressMode AddressU {
        get => (TextureAddressMode) Underlying.AddressU;
        set => Underlying.AddressU = value.ToXna();
    }

    public TextureAddressMode AddressV {
        get => (TextureAddressMode) Underlying.AddressV;
        set => Underlying.AddressV = value.ToXna();
    }

    public TextureAddressMode AddressW {
        get => (TextureAddressMode) Underlying.AddressW;
        set => Underlying.AddressW = value.ToXna();
    }

    public TextureFilter Filter {
        get => (TextureFilter) Underlying.Filter;
        set => Underlying.Filter = value.ToXna();
    }

    public int MaxAnisotropy {
        get => Underlying.MaxAnisotropy;
        set => Underlying.MaxAnisotropy = value;
    }

    public int MaxMipLevel {
        get => Underlying.MaxMipLevel;
        set => Underlying.MaxMipLevel = value;
    }

    public float MipLODBias {
        get => Underlying.MipMapLevelOfDetailBias;
        set => Underlying.MipMapLevelOfDetailBias = value;
    }

    internal XnaGraphics.SamplerState Underlying { get; }

    public override int GetHashCode() {
        int hashCode = 486187739;

        unchecked {
            hashCode = hashCode * 1610612741 + AddressU.GetHashCode();
            hashCode = hashCode * 1610612741 + AddressV.GetHashCode();
            hashCode = hashCode * 1610612741 + AddressW.GetHashCode();
            hashCode = hashCode * 1610612741 + Filter.GetHashCode();
            hashCode = hashCode * 1610612741 + MaxAnisotropy.GetHashCode();
            hashCode = hashCode * 1610612741 + MaxMipLevel.GetHashCode();
            hashCode = hashCode * 1610612741 + MipLODBias.GetHashCode();
        }

        return hashCode;
    }
}
