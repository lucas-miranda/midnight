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
        : base(Program.Rendering != null ?
            new XnaGraphics.RenderTarget2D(
                Program.Rendering?.XnaGraphicsDevice,
                width,
                height,
                mipMap,
                format.ToXna(),
                depthFormat.ToXna(),
                multiSampleCount,
                usage.ToXna()
            )
            : null)
    {
        DepthStencilFormat = depthFormat;
        MultiSampleCount = multiSampleCount;
        Usage = usage;
    }

    public DepthFormat DepthStencilFormat { get; }
    public int MultiSampleCount { get; }
    public RenderTargetUsage Usage { get; }

    internal override XnaGraphics.RenderTarget2D Underlying { get => (XnaGraphics.RenderTarget2D) base.Underlying; }

    /// <summary>
    /// Create a <see cref="Canvas"/> based on <see cref="BackBuffer"/> settings.
    /// </summary>
    public static Canvas FromBackBuffer(
        DepthFormat? depthFormat = null,
        int? multiSampleCount = null
    ) {
        return new(
            Program.Graphics.BackBuffer.Width,
            Program.Graphics.BackBuffer.Height,
            false,
            Program.Graphics.BackBuffer.Format,
            depthFormat.GetValueOrDefault(DepthFormat.None),
            multiSampleCount.GetValueOrDefault(0),
            RenderTargetUsage.PreserveContents
        );
    }
}
