using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines a culling mode for faces in rasterization process.
/// </summary>
public enum CullMode {
    /// <summary>
    /// Do not cull faces.
    /// </summary>
    None = XnaGraphics.CullMode.None,

    /// <summary>
    /// Cull faces with clockwise order.
    /// </summary>
    ClockwiseFace = XnaGraphics.CullMode.CullClockwiseFace,

    /// <summary>
    /// Cull faces with counter clockwise order.
    /// </summary>
    CounterClockwiseFace = XnaGraphics.CullMode.CullCounterClockwiseFace,
}

internal static class CullModeExtensions {
    public static XnaGraphics.CullMode ToXna(this CullMode mode) {
        return (XnaGraphics.CullMode) mode;
    }
}
