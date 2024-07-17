using System.Collections.Generic;

namespace Midnight;

public class TextDisplayer : GraphicDisplayer {
    private List<VertexPositionColorTexture> _vertices = new List<VertexPositionColorTexture>();

    private Font _font;
    private string _value;
    private ShaderMaterial _material;
    private bool _needRegenerate, _hasCustomMaterial;

    public TextDisplayer() {
    }

    public Size2 SizeEm { get; set; }

    public Font Font {
        get => _font;
        set {
            _font = value;
            RequestGenerate();

            if (Material == null && !_hasCustomMaterial && _font is Font<MTSDF>) {
                MTSDFShader shader = Shader.Load<MTSDFShader>(
                        Embedded.Resources
                                .Fonts
                                .Shaders
                                .MTSDF
                    );

                _material = shader.CreateMaterial();
            }
        }
    }

    public string Value {
        get => _value;
        set {
            _value = value;
            RequestGenerate();
        }
    }

    public ShaderMaterial Material {
        get => _material;
        set {
            _material = value;
            _hasCustomMaterial = _material != null;
            RequestGenerate();
        }
    }

    public override void Render(DeltaTime dt, RenderingServer r) {
        if (_needRegenerate) {
            Generate();
        }

        Transform2D trans = Entity.Components.Get<Transform2D>();
        trans.FlushMatrix();

        System.Span<VertexPositionColorTexture> renderBuffer = stackalloc VertexPositionColorTexture[_vertices.Count];

        for (int i = 0; i < renderBuffer.Length; i++) {
            var v = _vertices[i];

            renderBuffer[i] = new(
                trans.Apply(v.Position * Font.Size),
                v.Color,
                v.TextureCoordinate
            );
        }

        if (Font is Font<MTSDF> font) {
            if (Material is MTSDFShaderMaterial mat) {
                mat.ChangeScreenPixelRange(font, Font.Size, 1.0f);
            }

            r.Draw(
                font.Typesetting.Texture,
                renderBuffer,
                0,
                renderBuffer.Length,
                null,
                0,
                0,
                Material,
                DrawSettings.Default with {
                    Samplers = new SamplerState[] { SamplerState.LinearClamp },
                }
            );
        }
    }

    private void TextureChanged() {
    }

    private void Generate() {
        _needRegenerate = false;
        _vertices.Clear();

        if (Value.IsEmpty()) {
            return;
        }

        if (Font is Font<MTSDF> font) {
            TextTypesetting text = font.Build(Value);
            _vertices.EnsureCapacity(text.Length * 3 * 2); // (3 vertices * 2 triangles) / codepoint

            float fontSizeRatio = 1.0f / font.Typesetting.NominalWidth;
            SizeEm = text.Size;

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

                _vertices.Add(new(new(pos), Color.White, uv.TopLeft));
                _vertices.Add(new(new(pos + new Vector2(quadSize.Width, 0.0f)), Color.White, uv.TopRight));
                _vertices.Add(new(new(pos + new Vector2(0.0f, quadSize.Height)), Color.White, uv.BottomLeft));

                /*
                       4
                      /|
                     / |
                    /  |
                   3---5
                */

                _vertices.Add(new(new(pos + new Vector2(0.0f, quadSize.Height)), Color.White, uv.BottomLeft));
                _vertices.Add(new(new(pos + new Vector2(quadSize.Width, 0.0f)), Color.White, uv.TopRight));
                _vertices.Add(new(new(pos + quadSize), Color.White, uv.BottomRight));
            }
        }
    }

    private void RequestGenerate() {
        _needRegenerate = true;
    }
}
