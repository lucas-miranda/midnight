
namespace Midnight;

public struct DrawParams {
    public Transform2D _transform;
    public ShaderMaterial _material;
    public DrawSettings? _drawSettings;
    public Color _color;

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

    public bool IsTransformDefined { get; private set; }
    public bool IsMaterialDefined { get; private set; }
    public bool IsDrawSettingsDefined { get; private set; }
    public bool IsColorDefined { get; private set; }
}
