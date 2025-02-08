
namespace Midnight;

public class RectangleDrawable : Drawable {
    private bool _filled = true;
    private Size2 _size;

    public RectangleDrawable() {
    }

    public Size2 Size {
        get => _size;
        set {
            _size = value;
            RequestRecalculateVertices();
        }
    }

    public bool Filled {
        get => _filled;
        set {
            _filled = value;
            RequestRecalculateVertices();
        }
    }

    protected override void Paint(DeltaTime dt, RenderingServer r) {
        //System.Console.WriteLine($"Rectangle Draw Begin (global pos: {Params.Transform.GlobalPosition}; pos: {Params.Transform.Position}; has parent? {(Params.Transform.Parent != null).ToString()})");
        //System.Console.WriteLine($"- Vertices: {Vertices.Length}, Final Vertices: {FinalVertices.Length}");
        DrawSettings settings = Params.DrawSettings.GetValueOrDefault(Midnight.DrawSettings.Default);

        if (Filled) {
            //System.Console.WriteLine("- Filled");
            settings = settings with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.TriangleList,
                //Immediate = true,
            };
        } else {
            //System.Console.WriteLine("- Hollow");
            settings = settings with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.LineStrip,
                //Immediate = true,
            };
        }

        r.Draw(
            null,
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
