
namespace Midnight;

public class StringDrawable : Drawable {
    private Font _font;
    private string _value;

    public StringDrawable() {
    }

    public Size2 SizeEm { get; private set; }
    public override Size2 Size {
        get => SizeEm * Font.Size;
        set { }
    }

    public Font Font {
        get => _font;
        set {
            _font = value;
            RecalculateVertices();

            if (Material == null && !UsingCustomMaterial && _font is Font<MTSDF>) {
                MTSDFShader shader = Shader.Load<MTSDFShader>(
                        Embedded.Resources
                                .Fonts
                                .Shaders
                                .MTSDF
                    );

                Material = shader.CreateMaterial();
                UsingCustomMaterial = false;
            }
        }
    }

    public string Value {
        get => _value;
        set {
            _value = value;
            RecalculateVertices();
        }
    }

    protected override void Paint(DeltaTime dt) {
        PositionScale = new(Font.Size);

        DrawSettings settings;
        if (Params.DrawSettings.HasValue) {
            settings = Params.DrawSettings.Value;
        } else {
            settings = Midnight.DrawSettings.Default with {
                Samplers = new SamplerState[] { SamplerState.LinearClamp },
            };
        }

        if (Font is Font<MTSDF> font) {
            if (Material is MTSDFShaderMaterial mat) {
                mat.ChangeScreenPixelRange(font, Font.Size, 1.0f);
            }

            RenderingServer.Draw(
                font.Typesetting.Texture,
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
    }

    protected override void UpdateVertices() {
        if (Value == null || Value.IsEmpty()) {
            ResizeVertices(0);
            return;
        }

        if (Font == null) {
            Font = Midnight.Font.Default();
        }

        if (Font is Font<MTSDF> font) {
            TextTypesetting text = font.Build(Value);
            ResizeVertices(text.Length * 3 * 2); // (3 vertices * 2 triangles) / codepoint

            float fontSizeRatio = 1.0f / font.Typesetting.NominalWidth;
            SizeEm = text.Size;
            int i = 0;

            foreach (GlyphTypesetting glyph in text) {
                Size2 glyphSize = glyph.SourceArea.Size;
                Size2 quadSize = glyphSize * fontSizeRatio;

                Rectangle uv = new(
                    glyph.SourceArea.Position / font.Typesetting.Texture.Size,
                    glyphSize / font.Typesetting.Texture.Size
                );

                Vector2 pos = glyph.Position;

                /*
                   0---1
                   |  /
                   | /
                   |/
                   2
                */

                Vertices[i] = new(new(pos), Color.White, uv.TopLeft);
                Vertices[i + 1] = new(new(pos + new Vector2(quadSize.Width, 0.0f)), Color.White, uv.TopRight);
                Vertices[i + 2] = new(new(pos + new Vector2(0.0f, quadSize.Height)), Color.White, uv.BottomLeft);

                /*
                       4
                      /|
                     / |
                    /  |
                   3---5
                */

                Vertices[i + 3] = new(new(pos + new Vector2(0.0f, quadSize.Height)), Color.White, uv.BottomLeft);
                Vertices[i + 4] = new(new(pos + new Vector2(quadSize.Width, 0.0f)), Color.White, uv.TopRight);
                Vertices[i + 5] = new(new(pos + quadSize), Color.White, uv.BottomRight);

                i += 6;
            }
        }
    }
}
