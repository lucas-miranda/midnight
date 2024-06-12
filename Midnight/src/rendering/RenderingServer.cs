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

        //Vector2 position,
        //float rotation,
        //Vector2 scale,
        //Color color,
        //Vector2 origin,
        //Vector2 scroll,

        ShaderMaterial material,
        DrawSettings? settings

        //IShaderParameters shaderParameters,
        //float layerDepth = 1.0f
    ) {
        Batcher.Push(
            texture,
            vertexData,
            minVertexIndex,
            verticesLength,
            indices,
            minIndex,
            primitivesCount,
            material,
            settings
        );
    }

    public void Draw(Texture texture, VertexPositionColorTexture[] vertexData) {
        Draw(texture, vertexData, 0, vertexData.Length, null, 0, vertexData.Length / 2, null, null);
    }

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

        Batcher.DefaultMaterial = new SpriteShaderMaterial(shader);
        Batcher.LoadContent();
    }

    internal void PrepareRender() {
    }

    internal void Flush() {
        Batcher.Flush(this);
    }
}
