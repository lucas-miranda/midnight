
namespace Midnight.GUI;

public class Frame : Object, IContainer {
    private VertexPositionColorTexture[] _borderVertices = new VertexPositionColorTexture[] {
        new(Vector3.Zero, Color.White, Vector2.Zero),
        new(Vector3.Zero, Color.White, Vector2.Right),
        new(Vector3.Zero, Color.White, Vector2.DownRight),
        new(Vector3.Zero, Color.White, Vector2.Down),
        new(Vector3.Zero, Color.White, Vector2.Zero),
    };

    private VertexPositionColorTexture[] _backgroundVertices = new VertexPositionColorTexture[] {
        new(Vector3.Zero, Color.White, Vector2.Zero),
        new(Vector3.Zero, Color.White, Vector2.Right),
        new(Vector3.Zero, Color.White, Vector2.Down),

        new(Vector3.Zero, Color.White, Vector2.Down),
        new(Vector3.Zero, Color.White, Vector2.Right),
        new(Vector3.Zero, Color.White, Vector2.DownRight),
    };

    public Frame() {
        Size = new(200, 150);

        BackgroundMaterial = new SpriteShaderMaterial((SpriteShader) Game.Rendering.Batcher.DefaultMaterial.BaseShader) {
            ColorF = ColorF.White,
        };

        BorderMaterial = new SpriteShaderMaterial((SpriteShader) Game.Rendering.Batcher.DefaultMaterial.BaseShader) {
            ColorF = ColorF.Black,
        };

        System.Console.WriteLine($"Bg Mat: {BackgroundMaterial.GetHashCode()}; Border Mat: {BorderMaterial.GetHashCode()}");
    }

    public Container Container { get; } = new();
    public ShaderMaterial BackgroundMaterial { get; set; }
    public ShaderMaterial BorderMaterial { get; set; }
    public Color Background { get; set; }
    public Color Border { get; set; } = Color.Black;

    public override void Render(DeltaTime dt, RenderingServer r) {
        r.Draw(
            null,
            _backgroundVertices,
            0,
            _backgroundVertices.Length,
            null,
            0,
            0,
            BackgroundMaterial,
            DrawSettings.Default with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.TriangleList,
            }
        );

        r.Draw(
            null,
            _borderVertices,
            0,
            _borderVertices.Length,
            null,
            0,
            0,
            BorderMaterial,
            DrawSettings.Default with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.LineStrip,
            }
        );
    }

    public override string TreeToString() {
        string str = "[Frame |";

        foreach (Object obj in Container) {
            str += $" {obj.TreeToString()};";
        }

        return str + "]";
    }

    protected override void SizeChanged() {
        // border
        _borderVertices[0].Position = Vector3.Zero;
        _borderVertices[1].Position = new(Size.X, 0.0f, 0.0f);
        _borderVertices[2].Position = new(Size, 0.0f);
        _borderVertices[3].Position = new(0.0f, Size.Y, 0.0f);
        _borderVertices[4].Position = Vector3.Zero;

        // background
        _backgroundVertices[0].Position = Vector3.Zero;
        _backgroundVertices[1].Position = new(Size.X, 0.0f, 0.0f);
        _backgroundVertices[2].Position = new(0.0f, Size.Y, 0.0f);

        _backgroundVertices[3].Position = new(0.0f, Size.Y, 0.0f);
        _backgroundVertices[4].Position = new(Size.X, 0.0f, 0.0f);
        _backgroundVertices[5].Position = new(Size, 0.0f);
    }
}
