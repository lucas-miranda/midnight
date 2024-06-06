using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines filtering types for texture sampler.
/// </summary>
public enum TextureFilter {
    /// <summary>
    /// Use linear filtering.
    /// </summary>
    Linear = XnaGraphics.TextureFilter.Linear,

    /// <summary>
    /// Use point filtering.
    /// </summary>
    Point = XnaGraphics.TextureFilter.Point,

    /// <summary>
    /// Use anisotropic filtering.
    /// </summary>
    Anisotropic = XnaGraphics.TextureFilter.Anisotropic,
    /// <summary>
    /// Use linear filtering to shrink or expand, and point filtering between mipmap levels (mip).
    /// </summary>
    LinearMipPoint = XnaGraphics.TextureFilter.LinearMipPoint,

    /// <summary>
    /// Use point filtering to shrink (minify) or expand (magnify), and linear filtering between mipmap levels.
    /// </summary>
    PointMipLinear = XnaGraphics.TextureFilter.PointMipLinear,

    /// <summary>
    /// Use linear filtering to shrink, point filtering to expand, and linear filtering between mipmap levels.
    /// </summary>
    MinLinearMagPointMipLinear = XnaGraphics.TextureFilter.MinLinearMagPointMipLinear,

    /// <summary>
    /// Use linear filtering to shrink, point filtering to expand, and point filtering between mipmap levels.
    /// </summary>
    MinLinearMagPointMipPoint = XnaGraphics.TextureFilter.MinLinearMagPointMipPoint,

    /// <summary>
    /// Use point filtering to shrink, linear filtering to expand, and linear filtering between mipmap levels.
    /// </summary>
    MinPointMagLinearMipLinear = XnaGraphics.TextureFilter.MinPointMagLinearMipLinear,

    /// <summary>
    /// Use point filtering to shrink, linear filtering to expand, and point filtering between mipmap levels.
    /// </summary>
    MinPointMagLinearMipPoint = XnaGraphics.TextureFilter.MinPointMagLinearMipPoint,
}

internal static class TextureFilterExtensions {
    public static XnaGraphics.TextureFilter ToXna(this TextureFilter filter) {
        return (XnaGraphics.TextureFilter) filter;
    }
}
