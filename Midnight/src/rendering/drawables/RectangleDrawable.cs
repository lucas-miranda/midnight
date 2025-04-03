
namespace Midnight;

public class RectangleDrawable : Drawable {
    private bool _filled = true,
                 _hasCustomSize;

    private Size2 _size;
    private Texture2D _texture;

    public RectangleDrawable() {
    }

    public override Size2 Size {
        get => _size;
        set {
            if (value == _size) {
                return;
            }

            _size = value;
            _hasCustomSize = true;
            RequestRecalculateVertices();
        }
    }

    public bool Filled {
        get => _filled;
        set {
            if (value == _filled) {
                return;
            }

            _filled = value;
            RequestRecalculateVertices();
        }
    }

    public Texture2D Texture {
        get => _texture;
        set {
            if (value == _texture) {
                return;
            }

            _texture = value;

            if (!_hasCustomSize && _texture != null) {
                _size = _texture.Size.ToFloat();
            }

            RequestRecalculateVertices();
        }
    }

    protected override void Paint(DeltaTime dt) {
        //System.Console.WriteLine($"Rectangle Draw Begin (global pos: {Params.Transform.GlobalPosition}; pos: {Params.Transform.Position}; has parent? {(Params.Transform.Parent != null).ToString()})");
        //System.Console.WriteLine($"- Vertices: {Vertices.Length}, Final Vertices: {FinalVertices.Length}");
        DrawSettings settings = Params.DrawSettings.GetValueOrDefault(Midnight.DrawSettings.Default);

        if (Filled) {
            //System.Console.WriteLine("- Filled");
            settings = settings with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.TriangleList,
            };
        } else {
            //System.Console.WriteLine("- Hollow");
            settings = settings with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.LineStrip,
            };
        }

        if (Texture != null) {
            settings = settings with {
                Samplers = new SamplerState[] { SamplerState.PointClamp },
            };
        }

        RenderingServer.Draw(
            Texture,
            FinalVertices,
            0,
            FinalVertices.Length,
            null,
            0,
            0,
            Params.Material,
            settings
        );
        //System.Console.WriteLine("Rectangle Draw End");
    }

    protected override void UpdateVertices() {
        if (Filled) {
            ResizeVertices(6);

            Vertices[0] = new(new(Vector2.Zero, 0.0f), Color.White, Vector2.Zero);
            Vertices[1] = new(new(Size.Width, 0.0f, 0.0f), Color.White, Vector2.Right);
            Vertices[2] = new(new(0.0f, Size.Height, 0.0f), Color.White, Vector2.Down);

            Vertices[3] = new(new(0.0f, Size.Height, 0.0f), Color.White, Vector2.Down);
            Vertices[4] = new(new(Size.Width, 0.0f, 0.0f), Color.White, Vector2.Right);
            Vertices[5] = new(new(Size.ToVector2(), 0.0f), Color.White, Vector2.DownRight);
        } else {
            ResizeVertices(5);

            Vertices[0] = new(new(Vector2.Zero, 0.0f), Color.White, Vector2.Zero);
            Vertices[1] = new(new(Size.Width, 0.0f, 0.0f), Color.White, Vector2.Right);
            Vertices[2] = new(new(Size.ToVector2(), 0.0f), Color.White, Vector2.DownRight);
            Vertices[3] = new(new(0.0f, Size.Height, 0.0f), Color.White, Vector2.Down);
            Vertices[4] = new(new(Vector2.Zero, 0.0f), Color.White, Vector2.Zero);
        }
    }
}
