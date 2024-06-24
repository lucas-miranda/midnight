
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

    private Color _backgroundColor = Color.White,
                  _borderColor = Color.Black;

    public Frame() {
        Size = new(200, 150);
    }

    public Container Container { get; } = new();
    public ShaderMaterial BackgroundMaterial { get; set; }
    public ShaderMaterial BorderMaterial { get; set; }

    public Color BackgroundColor {
        get => _backgroundColor;
        set {
            _backgroundColor = value;
            UpdateVertices();
        }
    }

    public Color BorderColor {
        get => _borderColor;
        set {
            _borderColor = value;
            UpdateVertices();
        }
    }

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
        UpdateVertices();
    }

    private void UpdateVertices() {
        // border
        _borderVertices[0] = new(Vector3.Zero, BorderColor, Vector2.Zero);
        _borderVertices[1] = new(new(Size.X, 0.0f, 0.0f), BorderColor, Vector2.Right);
        _borderVertices[2] = new(new(Size, 0.0f), BorderColor, Vector2.DownRight);
        _borderVertices[3] = new(new(0.0f, Size.Y, 0.0f), BorderColor, Vector2.Down);
        _borderVertices[4] = new(Vector3.Zero, BorderColor, Vector2.Zero);

        // background
        _backgroundVertices[0] = new(Vector3.Zero, BackgroundColor, Vector2.Zero);
        _backgroundVertices[1] = new(new(Size.X, 0.0f, 0.0f), BackgroundColor, Vector2.Right);
        _backgroundVertices[2] = new(new(0.0f, Size.Y, 0.0f), BackgroundColor, Vector2.Down);

        _backgroundVertices[3] = new(new(0.0f, Size.Y, 0.0f), BackgroundColor, Vector2.Down);
        _backgroundVertices[4] = new(new(Size.X, 0.0f, 0.0f), BackgroundColor, Vector2.Right);
        _backgroundVertices[5] = new(new(Size, 0.0f), BackgroundColor, Vector2.DownRight);
    }
}
