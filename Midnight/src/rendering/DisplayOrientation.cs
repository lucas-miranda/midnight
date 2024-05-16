using Xna = Microsoft.Xna.Framework;

namespace Midnight;

/// <summary>
/// Defines the orientation of the display.
/// </summary>
[System.Flags]
public enum DisplayOrientation {
    /// <summary>
    /// The default orientation.
    /// </summary>
    Default = Xna.DisplayOrientation.Default,
    /// <summary>
    /// The display is rotated counterclockwise into a landscape orientation. Width is greater than height.
    /// </summary>
    LandscapeLeft = Xna.DisplayOrientation.LandscapeLeft,
    /// <summary>
    /// The display is rotated clockwise into a landscape orientation. Width is greater than height.
    /// </summary>
    LandscapeRight = Xna.DisplayOrientation.LandscapeRight,
    /// <summary>
    /// The display is rotated as portrait, where height is greater than width.
    /// </summary>
    Portrait = Xna.DisplayOrientation.Portrait
}

public static class DisplayOrientationExtensions {
    internal static Xna.DisplayOrientation ToXna(this DisplayOrientation orientation) {
        return (Xna.DisplayOrientation) orientation;
    }
}
