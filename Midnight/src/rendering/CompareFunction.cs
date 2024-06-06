using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// The comparison function used for depth, stencil, and alpha tests.
/// </summary>
public enum CompareFunction {
    /// <summary>
    /// Always passes the test.
    /// </summary>
    Always = XnaGraphics.CompareFunction.Always,

    /// <summary>
    /// Never passes the test.
    /// </summary>
    Never = XnaGraphics.CompareFunction.Never,

    /// <summary>
    /// Passes the test when the new pixel value is less than current pixel value.
    /// </summary>
    Less = XnaGraphics.CompareFunction.Less,

    /// <summary>
    /// Passes the test when the new pixel value is less than or equal to current pixel value.
    /// </summary>
    LessEqual = XnaGraphics.CompareFunction.LessEqual,

    /// <summary>
    /// Passes the test when the new pixel value is equal to current pixel value.
    /// </summary>
    Equal = XnaGraphics.CompareFunction.Equal,

    /// <summary>
    /// Passes the test when the new pixel value is greater than or equal to current pixel value.
    /// </summary>
    GreaterEqual = XnaGraphics.CompareFunction.GreaterEqual,

    /// <summary>
    /// Passes the test when the new pixel value is greater than current pixel value.
    /// </summary>
    Greater = XnaGraphics.CompareFunction.Greater,

    /// <summary>
    /// Passes the test when the new pixel value does not equal to current pixel value.
    /// </summary>
    NotEqual = XnaGraphics.CompareFunction.NotEqual,
}

internal static class CompareFunctionExtensions {
    public static XnaGraphics.CompareFunction ToXna(this CompareFunction fn) {
        return (XnaGraphics.CompareFunction) fn;
    }
}
