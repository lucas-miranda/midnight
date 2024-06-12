namespace Midnight;

public class SpriteDisplayer : GraphicDisplayer {
    private Texture2D _texture;

    private VertexPositionColorTexture[] _vertices = new VertexPositionColorTexture[] {
        new(Vector3.Zero, Color.White, Vector2.Zero),
        new(Vector3.Zero, Color.White, Vector2.Right),
        new(Vector3.Zero, Color.White, Vector2.Down),

        new(Vector3.Zero, Color.White, Vector2.Down),
        new(Vector3.Zero, Color.White, Vector2.Right),
        new(Vector3.Zero, Color.White, Vector2.DownRight),
    };

    public Texture2D Texture {
        get => _texture;
        set {
            if (value == _texture) {
                return;
            }

            _texture = value;
            TextureChanged();
        }
    }

    public ShaderMaterial Material { get; set; }

    public override void Render(DeltaTime dt, RenderingServer r) {
        Transform2D trans = Entity.Components.Get<Transform2D>();
        trans.FlushMatrix();

        VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[_vertices.Length];

        for (int i = 0; i < vertices.Length; i++) {
            var v = _vertices[i];
            vertices[i] = new(
                trans.Apply(v.Position),
                v.Color,
                v.TextureCoordinate
            );
        }

        r.Draw(
            Texture,
            vertices,
            0,
            vertices.Length,
            null,
            0,
            0,
            Material,
            DrawSettings.Default
        );
    }

    private void TextureChanged() {
        if (Texture == null) {
            return;
        }

        _vertices[0].Position = Vector3.Zero;
        _vertices[1].Position = new(Texture.Width, 0.0f, 0.0f);
        _vertices[2].Position = new(0.0f, Texture.Height, 0.0f);

        _vertices[3].Position = new(0.0f, Texture.Height, 0.0f);
        _vertices[4].Position = new(Texture.Width, 0.0f, 0.0f);
        _vertices[5].Position = new(Texture.Width, Texture.Height, 0.0f);
    }
}
