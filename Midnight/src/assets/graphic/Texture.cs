using System.IO;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public abstract class Texture : IAsset {
    internal Texture(XnaGraphics.Texture xnaTexture) {
        Debug.AssertNotNull(xnaTexture);
        Underlying = xnaTexture;
        Format = (SurfaceFormat) Underlying.Format;
    }

    public string Name { get; set; }
    public string[] Filepaths { get; protected set; } = new string[1];
    public string Filepath { get => Filepaths[0]; protected set => Filepaths[0] = value; }
    public bool IsDisposed { get; private set; }

    public SurfaceFormat Format { get; private set; }

    internal virtual XnaGraphics.Texture Underlying { get; }

    public abstract void Reload();
    public abstract void Reload(Stream stream);

    public virtual void Dispose() {
        IsDisposed = true;
        Underlying?.Dispose();
    }
}
