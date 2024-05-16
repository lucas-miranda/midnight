using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>Defines formats for depth-stencil buffer.</summary>
public enum DepthFormat {
    /// <summary>Depth-stencil buffer will not be created.</summary>
    None = XnaGraphics.DepthFormat.None,

    /// <summary>16-bit depth buffer.</summary>
    Depth16 = XnaGraphics.DepthFormat.Depth16,

    /// <summary>24-bit depth buffer.</summary>
    Depth24 = XnaGraphics.DepthFormat.Depth24,

    /// <summary>
    /// 32-bit depth-stencil buffer. Where 24-bit depth and 8-bit for stencil used.
    /// </summary>
    Depth24Stencil8 = XnaGraphics.DepthFormat.Depth24Stencil8,
}

public static class DepthFormatExtensions {
    internal static XnaGraphics.DepthFormat ToXna(this DepthFormat usage) {
        return (XnaGraphics.DepthFormat) usage;
    }
}
