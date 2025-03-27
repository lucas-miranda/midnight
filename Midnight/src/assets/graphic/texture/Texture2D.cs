using System.IO;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class Texture2D : Texture {
    public Texture2D(int width, int height)
        : this(new(
            RenderingServer.XnaGraphicsDevice,
            width,
            height
        )) {
    }

    public Texture2D(int width, int height, bool mipMap, SurfaceFormat format)
        : this(new(
            RenderingServer.XnaGraphicsDevice,
            width,
            height,
            mipMap,
            format.ToXna()
        )) {
    }

    internal Texture2D() : base() {
    }


    internal Texture2D(XnaGraphics.Texture2D xnaTexture) : base(xnaTexture) {
    }

    public Size2I Size => new(Underlying.Width, Underlying.Height);
    public int Width => Size.Width;
    public int Height => Size.Height;

    internal override XnaGraphics.Texture2D Underlying {
        get => (XnaGraphics.Texture2D) base.Underlying;
    }

    public static Texture2D Load(string filepath) {
        using (FileStream stream = File.OpenRead(filepath)) {
            Texture2D texture = Load(stream);
            texture.Filepath = filepath;
            return texture;
        }
    }

    public static Texture2D Load(byte[] bytecode) {
        Assert.True(RenderingServer.IsInitialized);

        using (MemoryStream stream = new(bytecode, false)) {
            XnaGraphics.Texture2D texture = XnaGraphics.Texture2D.FromStream(RenderingServer.XnaGraphicsDevice, stream);

            if (texture == null) {
                return null;
            }

            return new(texture);
        }
    }

    public static Texture2D Load(string filepath, int width, int height, bool zoom) {
        using (FileStream stream = File.OpenRead(filepath)) {
            return Load(stream, width, height, zoom);
        }
    }

    public static Texture2D Load(Stream stream) {
        Assert.NotNull(stream);
        XnaGraphics.Texture2D xnaTexture = XnaGraphics.Texture2D.FromStream(
            RenderingServer.XnaGraphicsDevice,
            stream
        );

        return new(xnaTexture);
    }

    public static Texture2D Load(Stream stream, int width, int height, bool zoom) {
        Assert.NotNull(stream);
        XnaGraphics.Texture2D xnaTexture = XnaGraphics.Texture2D.FromStream(
            RenderingServer.XnaGraphicsDevice,
            stream,
            width,
            height,
            zoom
        );

        return new(xnaTexture);
    }

    public override bool Reload() {
        using (FileStream stream = File.OpenRead(((IAsset) this).Filepath)) {
            return Reload(stream);
        }
    }

    public override bool Reload(Stream stream) {
        Assert.NotNull(stream);
        XnaGraphics.Texture2D.TextureDataFromStreamEXT(
            stream,
            out int w,
            out int h,
            out byte[] pixels
        );

        Write(
            0,
            new RectangleI(w, h),
            pixels,
            0,
            pixels.Length
        );

        return true;
    }

    public override bool Release() {
        return base.Release();
    }

    public void Read<T>(T[] data) where T : struct {
        Assert.NotNull(Underlying);
        Assert.NotNull(data);
        Underlying.GetData(data);
    }

    public void Read<T>(T[] data, int startIndex, int elementCount) where T : struct {
        Assert.NotNull(Underlying);
        Assert.NotNull(data);
        Underlying.GetData(data, startIndex, elementCount);
    }

    public void Read<T>(
        int level,
        RectangleI? bounds,
        T[] data,
        int startIndex,
        int elementCount
    ) where T : struct {
        Assert.NotNull(Underlying);
        Assert.NotNull(data);
        Underlying.GetData(level, bounds.ToXna(), data, startIndex, elementCount);
    }

    public void Write<T>(T[] data) where T : struct {
        Assert.NotNull(Underlying);
        Assert.NotNull(data);
        Underlying.SetData(data);
    }

    public void Write<T>(T[] data, int startIndex, int elementCount) where T : struct {
        Assert.NotNull(Underlying);
        Assert.NotNull(data);
        Underlying.SetData(data, startIndex, elementCount);
    }

    public void Write<T>(
        int level,
        RectangleI? bounds,
        T[] data,
        int startIndex,
        int elementCount
    ) where T : struct {
        Assert.NotNull(Underlying);
        Assert.NotNull(data);
        Underlying.SetData(level, bounds.ToXna(), data, startIndex, elementCount);
    }

    public override string ToString() {
        return $"{base.ToString()}; Size: {Size}";
    }
}
