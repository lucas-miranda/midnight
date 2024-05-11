using Midnight.Diagnostics;
using Xna = Microsoft.Xna.Framework;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public class RenderingServer : IRenderable {
    private XnaGraphics.SpriteBatch _batcher;

    internal RenderingServer(XnaGraphics.GraphicsDevice xnaDevice) {
        Debug.AssertNotNull(xnaDevice);
        XnaDevice = xnaDevice;

        _batcher = new(XnaDevice);
    }

    internal XnaGraphics.GraphicsDevice XnaDevice { get; }

    public void PreRender() {
        _batcher.Begin();
    }

    public void Render() {
        _batcher.End();
    }

    public void Draw(Texture2D texture) {
        _batcher.Draw(texture.Underlying, Xna.Vector2.Zero, Xna.Color.White);
    }
}
