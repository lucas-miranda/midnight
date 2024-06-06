using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines options for filling the primitive.
/// </summary>
public enum FillMode {
    /// <summary>
    /// Draw solid faces for each primitive.
    /// </summary>
    Solid = XnaGraphics.FillMode.Solid,
    /// <summary>
    /// Draw lines for each primitive.
    /// </summary>
    WireFrame = XnaGraphics.FillMode.WireFrame,
}

internal static class FillModeExtensions {
    public static XnaGraphics.FillMode ToXna(this FillMode mode) {
        return (XnaGraphics.FillMode) mode;
    }
}
