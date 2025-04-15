
namespace Midnight;

public struct DrawParams {
    private Transform2D _transform;
    private ShaderMaterial _material;
    private DrawSettings? _drawSettings;
    private Color _color;
    private DrawableLayer _layer;

    public DrawParams() {
        _color = Color.White;
    }

    public Transform2D Transform {
        get => _transform;
        set {
            _transform = value;
            IsTransformDefined = true;
        }
    }

    public ShaderMaterial Material {
        get => _material;
        set {
            _material = value;
            IsMaterialDefined = true;
        }
    }

    public DrawSettings? DrawSettings {
        get => _drawSettings;
        set {
            _drawSettings = value;
            IsDrawSettingsDefined = true;
        }
    }

    public Color Color {
        get => _color;
        set {
            _color = value;
            IsColorDefined = true;
        }
    }

    public DrawableLayer Layer {
        get => _layer;
        set {
            _layer = value;
            IsLayerDefined = true;
        }
    }

    public bool IsTransformDefined { get; private set; }
    public bool IsMaterialDefined { get; private set; }
    public bool IsDrawSettingsDefined { get; private set; }
    public bool IsColorDefined { get; private set; }
    public bool IsLayerDefined { get; private set; }
}
