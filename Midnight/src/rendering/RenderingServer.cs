using Midnight.Diagnostics;

using XnaGraphics = Microsoft.Xna.Framework.Graphics;

namespace Midnight;

public sealed class RenderingServer {
    private static RenderingServer _instance;
    private Camera _mainCamera;
    private XnaGraphics.GraphicsDevice _xnaGraphicsDevice;

    private RenderingServer(XnaGraphics.GraphicsDevice xnaDevice) {
        Assert.NotNull(xnaDevice);
        _xnaGraphicsDevice = xnaDevice;
    }

    public static bool IsInitialized => _instance != null;
    public static DrawBatcher<VertexPositionColorTexture> Batcher { get; private set; }

    public static Camera MainCamera {
        get => _instance._mainCamera;
        set {
            _instance._mainCamera = value;
            _instance._mainCamera?.RequestRecalculate();
        }
    }

    public static Canvas MainCanvas { get; private set; }
    public static RenderTarget Target { get; private set; }
    public static RenderLayers Layers { get; private set; }

    internal static XnaGraphics.GraphicsDevice XnaGraphicsDevice => _instance._xnaGraphicsDevice;

    public static void Clear(Color color) {
        XnaGraphicsDevice.Clear(color.ToXna());
    }

    public static void Clear(ClearOptions options, Color color, float depth, int stencil) {
        XnaGraphicsDevice.Clear(options.ToXna(), color.ToXna().ToVector4(), depth, stencil);
    }

    public static void Draw(
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

    public static void Draw(Texture texture, VertexPositionColorTexture[] vertexData) {
        Draw(texture, vertexData, 0, vertexData.Length, null, 0, vertexData.Length / 2, null, null);
    }

    internal static void Initialize(XnaGraphics.GraphicsDevice xnaDevice) {
        if (_instance != null) {
            return;
        }

        _instance = new(xnaDevice);
        _instance.Initialized();
    }

    private void Initialized() {
        Batcher = new();
        _instance._mainCamera = new();
        Target = new(XnaGraphicsDevice);
        Layers = new();
    }

    internal static void GraphicsReady() {
        Layers.LoadContent();

        MainCanvas = Canvas.FromBackBuffer(DepthFormat.Depth24Stencil8);
        Layers.Register(0, MainCanvas);

        // set default shader material for Batcher
        SpriteShader shader = Shader.Load<SpriteShader>(Embedded.Resources.Shaders.Sprite);
        Batcher.DefaultMaterial = new SpriteShaderMaterial(shader);

        Batcher.LoadContent();
    }

    internal static void ResourceRelease() {
        Layers.ResourceRelease();
        MainCanvas.Release();
    }

    internal static void PrepareRender() {
        MainCamera?.Recalculate();

        // TODO  add Viewport
        //Clear(ClearOptions.All, Color.Transparent, XnaGraphicsDevice.Viewport.MaxDepth, 0);

        Batcher.ResetStats();
    }

    internal static void Flush() {
        Batcher.Flush();
    }
}
