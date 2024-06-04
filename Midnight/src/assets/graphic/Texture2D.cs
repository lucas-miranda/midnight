using System.IO;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class Texture2D : Texture {
    public Texture2D(int width, int height)
        : this(new(
            Program.Rendering.XnaGraphicsDevice,
            width,
            height
        )) {
    }

    public Texture2D(int width, int height, bool mipMap, SurfaceFormat format)
        : this(new(
            Program.Rendering.XnaGraphicsDevice,
            width,
            height,
            mipMap,
            format.ToXna()
        )) {
    }

    internal Texture2D(XnaGraphics.Texture2D xnaTexture) : base(xnaTexture) {
        Width = Underlying.Width;
        Height = Underlying.Height;
    }

    public int Width { get; private set; }
    public int Height { get; private set; }

    internal override XnaGraphics.Texture2D Underlying { get => (XnaGraphics.Texture2D) base.Underlying; }

    public static Texture2D Load(string filepath) {
        using (FileStream stream = File.OpenRead(filepath)) {
            Texture2D texture = Load(stream);
            texture.Filepath = filepath;
            return texture;
        }
    }

    public static Texture2D Load(string filepath, int width, int height, bool zoom) {
        using (FileStream stream = File.OpenRead(filepath)) {
            return Load(stream, width, height, zoom);
        }
    }

    public static Texture2D Load(Stream stream) {
        Debug.AssertNotNull(stream);
        XnaGraphics.Texture2D xnaTexture = XnaGraphics.Texture2D.FromStream(
            Program.Rendering.XnaGraphicsDevice,
            stream
        );

        return new(xnaTexture);
    }

    public static Texture2D Load(Stream stream, int width, int height, bool zoom) {
        Debug.AssertNotNull(stream);
        XnaGraphics.Texture2D xnaTexture = XnaGraphics.Texture2D.FromStream(
            Program.Rendering.XnaGraphicsDevice,
            stream,
            width,
            height,
            zoom
        );

        return new(xnaTexture);
    }

    public override void Reload() {
        using (FileStream stream = File.OpenRead(((IAsset) this).Filepath)) {
            Reload(stream);
        }
    }

    public override void Reload(Stream stream) {
        Debug.AssertNotNull(stream);
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
    }

    public override void Dispose() {
        base.Dispose();
    }

    public void Read<T>(T[] data) where T : struct {
        Debug.AssertNotNull(Underlying);
        Debug.AssertNotNull(data);
        Underlying.GetData(data);
    }

    public void Read<T>(T[] data, int startIndex, int elementCount) where T : struct {
        Debug.AssertNotNull(Underlying);
        Debug.AssertNotNull(data);
        Underlying.GetData(data, startIndex, elementCount);
    }

    public void Read<T>(
        int level,
        RectangleI? bounds,
        T[] data,
        int startIndex,
        int elementCount
    ) where T : struct {
        Debug.AssertNotNull(Underlying);
        Debug.AssertNotNull(data);
        Underlying.GetData(level, bounds.ToXna(), data, startIndex, elementCount);
    }

    public void Write<T>(T[] data) where T : struct {
        Debug.AssertNotNull(Underlying);
        Debug.AssertNotNull(data);
        Underlying.SetData(data);
    }

    public void Write<T>(T[] data, int startIndex, int elementCount) where T : struct {
        Debug.AssertNotNull(Underlying);
        Debug.AssertNotNull(data);
        Underlying.SetData(data, startIndex, elementCount);
    }

    public void Write<T>(
        int level,
        RectangleI? bounds,
        T[] data,
        int startIndex,
        int elementCount
    ) where T : struct {
        Debug.AssertNotNull(Underlying);
        Debug.AssertNotNull(data);
        Underlying.SetData(level, bounds.ToXna(), data, startIndex, elementCount);
    }
}
