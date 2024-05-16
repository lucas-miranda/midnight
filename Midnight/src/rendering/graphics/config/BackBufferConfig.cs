
namespace Midnight;

public readonly record struct BackBufferConfig() {
    public int Width { get; init; } = 1920 / 4;
    public int Height { get; init; } = 1080 / 4;
    public bool VSync { get; init; } = true;
    public bool MultiSampling { get; init; } = false;
    public SurfaceFormat Format { get; init; }= SurfaceFormat.Color;
    public DepthFormat DepthStencilFormat { get; init; } = DepthFormat.Depth24Stencil8;
    public bool IsFullScreen { get; init; }
}
