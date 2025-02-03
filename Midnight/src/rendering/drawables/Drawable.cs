using Midnight.Diagnostics;

namespace Midnight;

public abstract class Drawable {
    private VertexPositionColorTexture[] _vertices, _finalVertices;
    private ShaderMaterial _material;

    public Transform2D Transform { get; set; }

    public ShaderMaterial Material {
        get => _material;
        set {
            _material = value;
            UsingCustomMaterial = _material != null;
        }
    }

    public DrawSettings? DrawSettings { get; set; }
    public Color Color { get; set; } = Color.White;
    public DrawParams Params { get; private set; }
    public bool IsRecalculateRequested { get; private set; } = true;

    protected VertexPositionColorTexture[] Vertices {
        get => _vertices;
        set => _vertices = value;
    }

    protected VertexPositionColorTexture[] FinalVertices {
        get => _finalVertices;
        set => _finalVertices = value;
    }

    protected bool UsingCustomMaterial { get; set; }
    protected Vector2 PositionScale { get; set; } = Vector2.One;

    public virtual void Draw(DeltaTime dt, RenderingServer r) {
        Params = new() {
            Transform = Transform,
            Material = Material,
            DrawSettings = DrawSettings,
            Color = Color,
        };

        if (IsRecalculateRequested) {
            RecalculateVertices();
        }
    }

    public void RequestRecalculateVertices() {
        IsRecalculateRequested = true;
    }

    public void RecalculateVertices() {
        IsRecalculateRequested = false;
        UpdateVertices();
    }

    protected abstract void UpdateVertices();

    protected void PrepareFinalVertices() {
        Assert.NotNull(Vertices, "Vertices must be resized first.");

        if (FinalVertices == null) {
            FinalVertices = new VertexPositionColorTexture[Vertices.Length];
        } else if (FinalVertices.Length != Vertices.Length) {
            System.Array.Resize(ref _finalVertices, Vertices.Length);
        }

        if (Params.Transform != null) {
            Params.Transform.FlushMatrix();

            for (int i = 0; i < Vertices.Length; i++) {
                var v = Vertices[i];
                FinalVertices[i] = new(
                    Params.Transform.Apply(v.Position * PositionScale),
                    (v.Color.Normalized() * Params.Color).ToByte(),
                    v.TextureCoordinate
                );
            }
        } else {
            for (int i = 0; i < Vertices.Length; i++) {
                var v = Vertices[i];
                FinalVertices[i] = new(
                    v.Position * PositionScale,
                    (v.Color.Normalized() * Params.Color).ToByte(),
                    v.TextureCoordinate
                );
            }
        }
    }

    protected void ResizeVertices(int n) {
        if (Vertices == null) {
            Vertices = new VertexPositionColorTexture[n];
        } else if (_vertices.Length != n) {
            System.Array.Resize(ref _vertices, n);
        }
    }
}
