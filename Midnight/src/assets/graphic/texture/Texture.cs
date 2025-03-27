using System.IO;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public abstract class Texture : IAsset {
    private XnaGraphics.Texture _underlying;

    internal Texture() {
        Assert.True(RenderingServer.IsInitialized, $"{nameof(RenderingServer)} isn't ready yet.\nMaybe it's at wrong engine step?");
    }

    internal Texture(XnaGraphics.Texture xnaTexture) : this() {
        Underlying = xnaTexture;
    }

    public string Name { get; set; }
    public string[] Filepaths { get; protected set; } = new string[1];
    public string Filepath { get => Filepaths[0]; protected set => Filepaths[0] = value; }
    public bool IsReleased { get; private set; }
    public SurfaceFormat Format => (SurfaceFormat) Underlying.Format;

    internal virtual XnaGraphics.Texture Underlying {
        get => _underlying;
        set {
            Assert.NotNull(value);
            _underlying = value;
        }
    }

    public abstract bool Reload();
    public abstract bool Reload(Stream stream);

    public virtual bool Release() {
        IsReleased = true;
        Underlying?.Dispose();
        return true;
    }

    public override string ToString() {
        return $"{GetType()}  Name: {Name}; Filepath: {string.Join(", ", Filepaths)}";
    }
}
