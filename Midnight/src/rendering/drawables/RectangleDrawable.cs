
namespace Midnight;

public class RectangleDrawable : Drawable {
    private bool _filled = true,
                 _hasCustomSize,
                 _hasCustomClippingRegion;

    private Size2 _size;
    private Texture2D _texture;
    private RectangleI _clipRegion;

    public RectangleDrawable() {
    }

    public override Size2 Size {
        get {
            if (!_hasCustomSize && _texture != null) {
                return ClipRegion.Size.ToFloat();
            }

            return _size;
        }
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

            if (_texture != null) {
                if (!_hasCustomSize) {
                    _size = _texture.Size.ToFloat();
                }

                if (!_hasCustomClippingRegion) {
                    _clipRegion = new(_texture.Size);
                }
            }

            RequestRecalculateVertices();
        }
    }

    public RectangleI ClipRegion {
        get => _clipRegion;
        set {
            _clipRegion = value;
            _hasCustomClippingRegion = true;
            RequestRecalculateVertices();
        }
    }

    protected override void Paint(DeltaTime dt) {
        DrawSettings settings = Params.DrawSettings.GetValueOrDefault(Midnight.DrawSettings.Default);

        if (Filled) {
            settings = settings with {
                Samplers = new SamplerState[0],
                Primitive = PrimitiveType.TriangleList,
            };
        } else {
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
    }

    protected override void UpdateVertices() {
        Rectangle uv;

        if (Texture != null) {
            uv = ClipRegion.ToFloat() / Texture.Size;
        } else {
            uv = Rectangle.Unit;
        }

        if (Filled) {
            ResizeVertices(6);

            Vertices[0] = new(new(Vector2.Zero, 0.0f), Color.White, uv.TopLeft);
            Vertices[1] = new(new(Size.Width, 0.0f, 0.0f), Color.White, uv.TopRight);
            Vertices[2] = new(new(0.0f, Size.Height, 0.0f), Color.White, uv.BottomLeft);

            Vertices[3] = new(new(0.0f, Size.Height, 0.0f), Color.White, uv.BottomLeft);
            Vertices[4] = new(new(Size.Width, 0.0f, 0.0f), Color.White, uv.TopRight);
            Vertices[5] = new(new(Size.ToVector2(), 0.0f), Color.White, uv.BottomRight);
        } else {
            ResizeVertices(5);

            Vertices[0] = new(new(Vector2.Zero, 0.0f), Color.White, uv.TopLeft);
            Vertices[1] = new(new(Size.Width, 0.0f, 0.0f), Color.White, uv.TopRight);
            Vertices[2] = new(new(Size.ToVector2(), 0.0f), Color.White, uv.BottomRight);
            Vertices[3] = new(new(0.0f, Size.Height, 0.0f), Color.White, uv.BottomLeft);
            Vertices[4] = new(new(Vector2.Zero, 0.0f), Color.White, uv.TopLeft);
        }
    }
}
