using Midnight.Diagnostics;

using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public sealed class RenderingServer {
    private Camera _mainCamera;

    internal RenderingServer(XnaGraphics.GraphicsDevice xnaDevice) {
        Assert.NotNull(xnaDevice);
        XnaGraphicsDevice = xnaDevice;
        Batcher = new();
        MainCamera = new();
        Target = new(xnaDevice);
        Layers = new();
    }

    public DrawBatcher<VertexPositionColorTexture> Batcher { get; }

    public Camera MainCamera {
        get => _mainCamera;
        set {
            _mainCamera = value;
            _mainCamera?.RequestRecalculate();
        }
    }

    public Canvas MainCanvas { get; private set; }
    public RenderTarget Target { get; }
    public RenderLayers Layers { get; }

    internal XnaGraphics.GraphicsDevice XnaGraphicsDevice { get; }

    public void Clear(Color color) {
        XnaGraphicsDevice.Clear(color.ToXna());
    }

    public void Clear(ClearOptions options, Color color, float depth, int stencil) {
        XnaGraphicsDevice.Clear(options.ToXna(), color.ToXna().ToVector4(), depth, stencil);
    }

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

    internal void GraphicsReady() {
        Layers.LoadContent();

        MainCanvas = Canvas.FromBackBuffer(DepthFormat.Depth24Stencil8);
        Layers.Register(0, MainCanvas);

        // set default shader material for Batcher
        SpriteShader shader = Shader.Load<SpriteShader>(Embedded.Resources.Shaders.Sprite);
        Batcher.DefaultMaterial = new SpriteShaderMaterial(shader);

        Batcher.LoadContent();
    }

    internal void ResourceRelease() {
        Layers.ResourceRelease();
        MainCanvas.Release();
    }

    internal void PrepareRender() {
        MainCamera?.Recalculate();

        // TODO  add Viewport
        Clear(ClearOptions.All, Color.Transparent, XnaGraphicsDevice.Viewport.MaxDepth, 0);

        Batcher.ResetStats();
    }

    internal void Flush() {
        Batcher.Flush(this);
    }
}
