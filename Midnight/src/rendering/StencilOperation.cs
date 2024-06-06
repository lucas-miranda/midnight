using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines stencil buffer operations.
/// </summary>
public enum StencilOperation {
    /// <summary>
    /// Does not update the stencil buffer entry.
    /// </summary>
    Keep = XnaGraphics.StencilOperation.Keep,

    /// <summary>
    /// Sets the stencil buffer entry to 0.
    /// </summary>
    Zero = XnaGraphics.StencilOperation.Zero,

    /// <summary>
    /// Replaces the stencil buffer entry with a reference value.
    /// </summary>
    Replace = XnaGraphics.StencilOperation.Replace,

    /// <summary>
    /// Increments the stencil buffer entry, wrapping to 0 if the new value exceeds the maximum value.
    /// </summary>
    Increment = XnaGraphics.StencilOperation.Increment,

    /// <summary>
    /// Decrements the stencil buffer entry, wrapping to the maximum value if the new value is less than 0.
    /// </summary>
    Decrement = XnaGraphics.StencilOperation.Decrement,

    /// <summary>
    /// Increments the stencil buffer entry, clamping to the maximum value.
    /// </summary>
    IncrementSaturation = XnaGraphics.StencilOperation.IncrementSaturation,

    /// <summary>
    /// Decrements the stencil buffer entry, clamping to 0.
    /// </summary>
    DecrementSaturation = XnaGraphics.StencilOperation.DecrementSaturation,

    /// <summary>
    /// Inverts the bits in the stencil buffer entry.
    /// </summary>
    Invert = XnaGraphics.StencilOperation.Invert,
}

internal static class StencilOperationExtensions {
    public static XnaGraphics.StencilOperation ToXna(this StencilOperation op) {
        return (XnaGraphics.StencilOperation) op;
    }
}
