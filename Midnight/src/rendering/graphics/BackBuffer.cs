using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public class BackBuffer {
    internal BackBuffer(Xna.GraphicsDeviceManager xnaGDM) {
        XnaGDM = xnaGDM;
    }

    public bool IsFullScreen {
        get => XnaGDM.IsFullScreen;
        set => XnaGDM.IsFullScreen = value;
    }

    public bool MultiSampling {
        get => XnaGDM.PreferMultiSampling;
        set => XnaGDM.PreferMultiSampling = value;
    }

    public SurfaceFormat Format {
        get => (SurfaceFormat) XnaGDM.PreferredBackBufferFormat;
        set => XnaGDM.PreferredBackBufferFormat = value.ToXna();
    }

    public int Width {
        get => XnaGDM.PreferredBackBufferWidth;
        set => XnaGDM.PreferredBackBufferWidth = value;
    }

    public int Height {
        get => XnaGDM.PreferredBackBufferHeight;
        set => XnaGDM.PreferredBackBufferHeight = value;
    }

    public Size2I Size {
        get => new(Width, Height);
        set {
            Width = value.Width;
            Height = value.Height;
        }
    }

    public DepthFormat DepthStencilFormat {
        get => (DepthFormat) XnaGDM.PreferredDepthStencilFormat;
        set => XnaGDM.PreferredDepthStencilFormat = value.ToXna();
    }

    public bool VSync {
        get => XnaGDM.SynchronizeWithVerticalRetrace;
        set => XnaGDM.SynchronizeWithVerticalRetrace = value;
    }

    internal Xna.GraphicsDeviceManager XnaGDM { get; }

    public override string ToString() {
        return $"FullScreen? {IsFullScreen}, MultiSampling? {MultiSampling}, Format: {Format}, Size: {Size}, DepthStencilFormat: {DepthStencilFormat}, VSync: {VSync}";
    }

    internal void LoadConfig(BackBufferConfig config) {
        IsFullScreen = config.IsFullScreen;
        MultiSampling = config.MultiSampling;
        Format = config.Format;
        Width = config.Width;
        Height = config.Height;
        DepthStencilFormat = config.DepthStencilFormat;
        VSync = config.VSync;
    }
}
