using Midnight.Diagnostics;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public class RenderingServer {
    private XnaGraphics.SpriteBatch _batcher;

    internal RenderingServer(XnaGraphics.GraphicsDevice xnaDevice) {
        Debug.AssertNotNull(xnaDevice);
        XnaGraphicsDevice = xnaDevice;

        _batcher = new(XnaGraphicsDevice);
    }

    internal XnaGraphics.GraphicsDevice XnaGraphicsDevice { get; }

    public void Draw(Texture2D texture, Vector2 position, Color color) {
        _batcher.Draw(texture.Underlying, position.ToXna(), color.ToXna());
    }

    internal void PrepareRender() {
        _batcher.Begin();
    }

    internal void Flush() {
        _batcher.End();
    }
}
