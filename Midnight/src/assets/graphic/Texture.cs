using System.IO;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public abstract class Texture : IAsset {
    internal Texture(XnaGraphics.Texture xnaTexture) {
        Assert.True(RenderingServer.IsInitialized, $"{nameof(RenderingServer)} isn't ready yet.\nMaybe it's at wrong engine step?");
        Assert.NotNull(xnaTexture);
        Underlying = xnaTexture;
        Format = (SurfaceFormat) Underlying.Format;
    }

    public string Name { get; set; }
    public string[] Filepaths { get; protected set; } = new string[1];
    public string Filepath { get => Filepaths[0]; protected set => Filepaths[0] = value; }
    public bool IsReleased { get; private set; }

    public SurfaceFormat Format { get; private set; }

    internal virtual XnaGraphics.Texture Underlying { get; }

    public abstract bool Reload();
    public abstract bool Reload(Stream stream);

    public virtual bool Release() {
        IsReleased = true;
        Underlying?.Dispose();
        return true;
    }
}
