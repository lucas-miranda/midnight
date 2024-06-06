using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public enum Blend {
    /// <summary>
    /// Each component of the color is multiplied by {1, 1, 1, 1}.
    /// </summary>
    One = XnaGraphics.Blend.One,

    /// <summary>
    /// Each component of the color is multiplied by {0, 0, 0, 0}.
    /// </summary>
    Zero = XnaGraphics.Blend.Zero,

    /// <summary>
    /// Each component of the color is multiplied by the source color.
    /// {Rs, Gs, Bs, As}, where Rs, Gs, Bs, As are color source values.
    /// </summary>
    SourceColor = XnaGraphics.Blend.SourceColor,

    /// <summary>
    /// Each component of the color is multiplied by the inverse of the source color.
    /// {1 - Rs, 1 - Gs, 1 - Bs, 1 - As}, where Rs, Gs, Bs, As are color source values.
    /// </summary>
    InverseSourceColor = XnaGraphics.Blend.InverseSourceColor,

    /// <summary>
    /// Each component of the color is multiplied by the alpha value of the source.
    /// {As, As, As, As}, where As is the source alpha value.
    /// </summary>
    SourceAlpha = XnaGraphics.Blend.SourceAlpha,

    /// <summary>
    /// Each component of the color is multiplied by the inverse of the alpha value of the source.
    /// {1 - As, 1 - As, 1 - As, 1 - As}, where As is the source alpha value.
    /// </summary>
    InverseSourceAlpha = XnaGraphics.Blend.InverseSourceAlpha,

    /// <summary>
    /// Each component color is multiplied by the destination color.
    /// {Rd, Gd, Bd, Ad}, where Rd, Gd, Bd, Ad are color destination values.
    /// </summary>
    DestinationColor = XnaGraphics.Blend.DestinationColor,

    /// <summary>
    /// Each component of the color is multiplied by the inversed destination color.
    /// {1 - Rd, 1 - Gd, 1 - Bd, 1 - Ad}, where Rd, Gd, Bd, Ad are color destination values.
    /// </summary>
    InverseDestinationColor = XnaGraphics.Blend.InverseDestinationColor,

    /// <summary>
    /// Each component of the color is multiplied by the alpha value of the destination.
    /// {Ad, Ad, Ad, Ad}, where Ad is the destination alpha value.
    /// </summary>
    DestinationAlpha = XnaGraphics.Blend.DestinationAlpha,

    /// <summary>
    /// Each component of the color is multiplied by the inversed alpha value of the destination.
    /// {1 - Ad, 1 - Ad, 1 - Ad, 1 - Ad}, where Ad is the destination alpha value.
    /// </summary>
    InverseDestinationAlpha = XnaGraphics.Blend.InverseDestinationAlpha,

    /// <summary>
    /// Each component of the color is multiplied by a constant in the <see cref="GraphicsDevice.BlendFactor"/>.
    /// </summary>
    BlendFactor = XnaGraphics.Blend.BlendFactor,

    /// <summary>
    /// Each component of the color is multiplied by a inversed constant in the <see cref="GraphicsDevice.BlendFactor"/>.
    /// </summary>
    InverseBlendFactor = XnaGraphics.Blend.InverseBlendFactor,

    /// <summary>
    /// Each component of the color is multiplied by either the alpha of the source color, or the inverse of the alpha of the source color, whichever is greater.
    /// {f, f, f, 1}, where f = min(As, 1 - As), where As is the source alpha value.
    /// </summary>
    SourceAlphaSaturation = XnaGraphics.Blend.SourceAlphaSaturation,
}

internal static class BlendExtensions {
    public static XnaGraphics.Blend ToXna(this Blend blend) {
        return (XnaGraphics.Blend) blend;
    }
}
