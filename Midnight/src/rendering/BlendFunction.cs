using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public enum BlendFunction {
    /// <summary>
    /// The function will add destination to the source. (srcColor * srcBlend) + (destColor * destBlend)
    /// </summary>
    Add = XnaGraphics.BlendFunction.Add,

    /// <summary>
    /// The function will subtract destination from source. (srcColor * srcBlend) - (destColor * destBlend)
    /// </summary>
    Subtract = XnaGraphics.BlendFunction.Subtract,

    /// <summary>
    /// The function will subtract source from destination. (destColor * destBlend) - (srcColor * srcBlend)
    /// </summary>
    ReverseSubtract = XnaGraphics.BlendFunction.ReverseSubtract,

    /// <summary>
    /// The function will extract minimum of the source and destination. min((srcColor * srcBlend),(destColor * destBlend))
    /// </summary>
    Max = XnaGraphics.BlendFunction.Max,

    /// <summary>
    /// The function will extract maximum of the source and destination. max((srcColor * srcBlend),(destColor * destBlend))
    /// </summary>
    Min = XnaGraphics.BlendFunction.Min,
}

internal static class BlendFunctionExtensions {
    public static XnaGraphics.BlendFunction ToXna(this BlendFunction fn) {
        return (XnaGraphics.BlendFunction) fn;
    }
}
