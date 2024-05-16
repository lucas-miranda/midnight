using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

/// <summary>
/// Defines a set of graphic capabilities.
/// </summary>
public enum GraphicsProfile {
    /// <summary>
    /// Use a limited set of graphic features and capabilities, allowing the game to support the widest variety of devices.
    /// </summary>
    Reach = XnaGraphics.GraphicsProfile.Reach,
    /// <summary>
    /// Use the largest available set of graphic features and capabilities to target devices, that have more enhanced graphic capabilities.
    /// </summary>
    HiDef = XnaGraphics.GraphicsProfile.HiDef,
}

public static class GraphicsProfileExtensions {
    internal static XnaGraphics.GraphicsProfile ToXna(this GraphicsProfile profile) {
        return (XnaGraphics.GraphicsProfile) profile;
    }
}
