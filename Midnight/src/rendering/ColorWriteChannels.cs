using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines the color channels for render target blending operations.
/// </summary>
[System.Flags]
public enum ColorWriteChannels {
    /// <summary>
    /// No channels selected.
    /// </summary>
    None = XnaGraphics.ColorWriteChannels.None,

    /// <summary>
    /// Red channel selected.
    /// </summary>
    Red = XnaGraphics.ColorWriteChannels.Red,

    /// <summary>
    /// Green channel selected.
    /// </summary>
    Green = XnaGraphics.ColorWriteChannels.Green,

    /// <summary>
    /// Blue channel selected.
    /// </summary>
    Blue = XnaGraphics.ColorWriteChannels.Blue,

    /// <summary>
    /// Alpha channel selected.
    /// </summary>
    Alpha = XnaGraphics.ColorWriteChannels.Alpha,

    /// <summary>
    /// All channels selected.
    /// </summary>
    All = XnaGraphics.ColorWriteChannels.All,
}

internal static class ColorWriteChannelsExtensions {
    public static XnaGraphics.ColorWriteChannels ToXna(this ColorWriteChannels channels) {
        return (XnaGraphics.ColorWriteChannels) channels;
    }
}
