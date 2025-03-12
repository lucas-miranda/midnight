
namespace Midnight;

public class LineDrawable : Drawable {
    private Vector2 _pointA, _pointB;
    private Size2 _size;
    private float? _width;

    public LineDrawable() {
    }

    public Vector2 PointA {
        get => _pointA;
        set {
            if (value == _pointA) {
                return;
            }

            _pointA = value;
            _size = Size2.Between(PointA, PointB);
            RequestRecalculateVertices();
        }
    }

    public Vector2 PointB {
        get => _pointB;
        set {
            if (value == _pointB) {
                return;
            }

            _pointB = value;
            _size = Size2.Between(PointA, PointB);
            RequestRecalculateVertices();
        }
    }

    public override Size2 Size {
        get => _size;
        set {
            if (value == _size) {
                return;
            }

            _size = value;
            PointB = PointA + _size;
            RequestRecalculateVertices();
        }
    }

    public float? Width {
        get => _width;
        set {
            if (value == _width) {
                return;
            }

            _width = value;
            RequestRecalculateVertices();
        }
    }

    protected override void Paint(DeltaTime dt) {
        DrawSettings settings = Params.DrawSettings.GetValueOrDefault(Midnight.DrawSettings.Default);

        if (Width.HasValue) {
            settings = settings with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.TriangleList,
            };
        } else {
            settings = settings with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.LineList,
            };
        }

        RenderingServer.Draw(
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
    }

    protected override void UpdateVertices() {
        if (Width.HasValue) {
            ResizeVertices(6);
            Vector2 end = PointB - PointA,
                    dir = end.ApproxZero() ? Vector2.Right : end.Normalized(),
                    down = dir.X > 0.0f ? dir.PerpendicularCW() : dir.PerpendicularCCW();

            Vertices[0] = new(new(PointA, 0.0f), Color.White, Vector2.Zero);
            Vertices[1] = new(new(PointB, 0.0f), Color.White, Vector2.Right);
            Vertices[2] = new(new(PointA + down * Width.Value, 0.0f), Color.White, Vector2.Down);

            Vertices[3] = new(Vertices[2].Position, Color.White, Vector2.Down);
            Vertices[4] = new(Vertices[1].Position, Color.White, Vector2.Right);
            Vertices[5] = new(new(PointB + down * Width.Value, 0.0f), Color.White, Vector2.DownRight);
        } else {
            ResizeVertices(2);

            Vertices[0] = new(new(PointA, 0.0f), Color.White, Vector2.Zero);
            Vertices[1] = new(new(PointB, 0.0f), Color.White, Vector2.Right);
        }
    }
}
