using Midnight.Diagnostics;

namespace Midnight;

public abstract class Drawable {
    private VertexPositionColorTexture[] _vertices, _finalVertices;
    private ShaderMaterial _material;

    public Drawable() {
        Transform = new();
    }

    public Transform2D Transform { get; }

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

    public void Draw(DeltaTime dt, RenderingServer r) {
        Params = new() {
            Transform = Transform,
            Material = Material,
            DrawSettings = DrawSettings,
            Color = Color,
        };

        Draw(dt, r, Params);
    }

    public void Draw(DeltaTime dt, RenderingServer r, DrawParams drawParams) {
        //System.Console.WriteLine($"Drawable '{GetType().Name}' Draw Begin");
        Params = new() {
            Transform = drawParams.IsTransformDefined ? drawParams.Transform : Transform,
            Material = drawParams.IsMaterialDefined ? drawParams.Material : Material,
            DrawSettings = drawParams.IsDrawSettingsDefined ? drawParams.DrawSettings : DrawSettings,
            Color = drawParams.IsColorDefined ? drawParams.Color : Color,
        };

        //System.Console.WriteLine($"params are = global pos: {Params.Transform.GlobalPosition}; pos: {Params.Transform.Position}; has parent? {(Params.Transform.Parent != null).ToString()}");

        if (IsRecalculateRequested) {
            RecalculateVertices();
        }

        Transform.FlushMatrix();

        if (Params.Transform != Transform) {
            Params.Transform.FlushMatrix();
            //System.Console.WriteLine($"Preparing Final Vertices with Custom Transform");
            PrepareFinalVertices(Params.Transform.Matrix * Transform.Matrix);
        } else {
            //System.Console.WriteLine($"Preparing Final Vertices");
            PrepareFinalVertices(Transform.Matrix);
        }

        Paint(dt, r);
        //System.Console.WriteLine($"Drawable '{GetType().Name}' Draw End");
    }

    public void RequestRecalculateVertices() {
        IsRecalculateRequested = true;
    }

    public void RecalculateVertices() {
        IsRecalculateRequested = false;
        UpdateVertices();
    }

    protected abstract void Paint(DeltaTime dt, RenderingServer r);
    protected abstract void UpdateVertices();

    protected void ResizeVertices(int n) {
        if (Vertices == null) {
            Vertices = new VertexPositionColorTexture[n];
        } else if (_vertices.Length != n) {
            System.Array.Resize(ref _vertices, n);
        }
    }

    private void PrepareFinalVertices(Matrix m) {
        Assert.NotNull(Vertices, "Vertices must be resized first.");

        if (FinalVertices == null) {
            FinalVertices = new VertexPositionColorTexture[Vertices.Length];
        } else if (FinalVertices.Length != Vertices.Length) {
            System.Array.Resize(ref _finalVertices, Vertices.Length);
        }

        for (int i = 0; i < Vertices.Length; i++) {
            var v = Vertices[i];
            //System.Console.WriteLine($"v[{i}] = Pos: {v.Position}, Pos Scale: {PositionScale}, result: {m * (v.Position * PositionScale)}");
            FinalVertices[i] = new(
                m * (v.Position * PositionScale),
                (v.Color.Normalized() * Params.Color).ToByte(),
                v.TextureCoordinate
            );
        }
    }
}
