using Midnight.Diagnostics;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public sealed class RenderingServer {
    private Camera _mainCamera;

    internal RenderingServer(XnaGraphics.GraphicsDevice xnaDevice) {
        Debug.AssertNotNull(xnaDevice);
        XnaGraphicsDevice = xnaDevice;
        MainCamera = new();
        Batcher = new();
    }

    public DrawBatcher<VertexPositionColorTexture> Batcher { get; }

    public Camera MainCamera {
        get => _mainCamera;
        set {
            _mainCamera = value;
            _mainCamera?.RequestRecalculate();
        }
    }

    internal XnaGraphics.GraphicsDevice XnaGraphicsDevice { get; }

    public void Draw(
        Texture texture,
        System.Span<VertexPositionColorTexture> vertexData,
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
        // set default shader material for Batcher
        SpriteShader shader = Shader.Load<SpriteShader>(Embedded.Resources.Shaders.Sprite);
        Batcher.DefaultMaterial = new SpriteShaderMaterial(shader);

        Batcher.LoadContent();
    }

    internal void PrepareRender() {
    }

    internal void Flush() {
        MainCamera?.Recalculate();
        Batcher.Flush(this);
    }
}
