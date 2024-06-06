using Midnight.Diagnostics;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public sealed class RenderingServer {

    internal RenderingServer(XnaGraphics.GraphicsDevice xnaDevice) {
        Debug.AssertNotNull(xnaDevice);
        XnaGraphicsDevice = xnaDevice;
        Batcher = new();
    }

    public DrawBatcher<VertexPositionColorTexture> Batcher { get; }
    internal XnaGraphics.GraphicsDevice XnaGraphicsDevice { get; }

    public void Draw(
        Texture texture,
        VertexPositionColorTexture[] vertexData,
        int minVertexIndex,
        int verticesLength,
        int[] indices,
        int minIndex,
        int primitivesCount,
        //bool isHollow,
        //Vector2 position,
        //float rotation,
        //Vector2 scale,
        //Color color,
        //Vector2 origin,
        //Vector2 scroll,
        Shader shader,
        DrawSettings? settings
        //IShaderParameters shaderParameters,
        //float layerDepth = 1.0f
    ) {
        // triangle strip
        Batcher.Push(
            texture,
            vertexData,
            minVertexIndex,
            verticesLength,
            indices,
            minIndex,
            primitivesCount,
            null,
            settings
        );
    }

    public void Draw(Texture texture, VertexPositionColorTexture[] vertexData) {
        // triangle strip
        Draw(texture, vertexData, 0, vertexData.Length, null, 0, vertexData.Length / 2, null, null);
    }

    /*
    public void Draw(Texture2D texture, Vector2 position, Color color) {
        //_batcher.Draw(texture.Underlying, position.ToXna(), color.ToXna());
    }
    */

    internal void LoadContent() {
        SpriteShader shader = Shader.Load<SpriteShader>(Embedded.Resources.Shaders.Sprite);

        BackBuffer backbuffer = Program.Graphics.BackBuffer;

        // TODO  calculate view bounds from something else,
        //       backbuffer and view can have different sizes
        Matrix world = Matrix.Identity;
        Matrix proj = Matrix.Ortho(backbuffer.Width, backbuffer.Height, -100, 100);
        Matrix view = Matrix.LookAt(Vector3.Zero, new(0.0f, 0.0f, 1.0f), new(0.0f, -1.0f, 0.0f));

        shader.ChangeTransform(
            ref world,
            ref view,
            ref proj
        );

        Batcher.DefaultShader = shader;
    }

    internal void PrepareRender() {
        //_batcher.Begin();
    }

    internal void Flush() {
        //_batcher.End();
        Batcher.Flush(this);
    }
}
