using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines if the previous content in a render target is preserved when it set on the graphics device.
/// </summary>
public enum RenderTargetUsage {
    /// <summary>
    /// The render target content will not be preserved.
    /// </summary>
    DiscardContents = XnaGraphics.RenderTargetUsage.DiscardContents,
    /// <summary>
    /// The render target content will be preserved even if it is slow or requires extra memory.
    /// </summary>
    PreserveContents = XnaGraphics.RenderTargetUsage.PreserveContents,

    /// <summary>
    /// The render target content might be preserved if the platform can do so without a penalty in performance or memory usage.
    /// </summary>
    PlatformContents = XnaGraphics.RenderTargetUsage.PlatformContents,
}

public static class RenderTargetUsageExtensions {
    internal static XnaGraphics.RenderTargetUsage ToXna(this RenderTargetUsage usage) {
        return (XnaGraphics.RenderTargetUsage) usage;
    }
}
