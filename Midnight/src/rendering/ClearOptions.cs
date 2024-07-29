using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

[System.Flags]
public enum ClearOptions {
    Target = XnaGraphics.ClearOptions.Target,
    DepthBuffer = XnaGraphics.ClearOptions.DepthBuffer,
    Stencil = XnaGraphics.ClearOptions.Stencil,
    All = Target | DepthBuffer | Stencil,
}

internal static class ClearOptionsExtensions {
    public static XnaGraphics.ClearOptions ToXna(this ClearOptions opt) {
        return (XnaGraphics.ClearOptions) opt;
    }
}
