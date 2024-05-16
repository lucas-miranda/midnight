using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public class Canvas : Texture2D {
    public Canvas(
        int width,
        int height
    )
        : this(
            width,
            height,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.DiscardContents
        ) {
    }

    public Canvas(
        int width,
        int height,
        bool mipMap,
        SurfaceFormat format,
        DepthFormat depthFormat,
        RenderTargetUsage usage
    )
        : this(
            width,
            height,
            mipMap,
            format,
            depthFormat,
            0,
            RenderTargetUsage.DiscardContents
        ) {
    }

    public Canvas(
        int width,
        int height,
        bool mipMap,
        SurfaceFormat format,
        DepthFormat depthFormat,
        int multiSampleCount,
        RenderTargetUsage usage
    )
        : base(new XnaGraphics.RenderTarget2D(
            Program.Rendering.XnaGraphicsDevice,
            width,
            height,
            mipMap,
            format.ToXna(),
            depthFormat.ToXna(),
            multiSampleCount,
            usage.ToXna()
        ))
    {
        DepthStencilFormat = depthFormat;
        MultiSampleCount = multiSampleCount;
        Usage = usage;
    }

    public DepthFormat DepthStencilFormat { get; }
    public int MultiSampleCount { get; }
    public RenderTargetUsage Usage { get; }

    internal override XnaGraphics.RenderTarget2D Underlying { get => (XnaGraphics.RenderTarget2D) base.Underlying; }

    public override void Dispose() {
        base.Dispose();
    }
}
