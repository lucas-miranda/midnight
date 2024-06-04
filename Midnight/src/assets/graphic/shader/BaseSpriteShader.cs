namespace Midnight;

public abstract class BaseSpriteShader : Shader {
    [System.Flags]
    public enum DirtyFlags {
        None                    = 0,
        TechniqueIndex          = 1 << 0,
        WorldViewProjection     = 1 << 1,
    }

    private Matrix _world, _view, _projection;

    public BaseSpriteShader() {
    }

    public virtual Matrix World {
        get => _world;
        set {
            _world = value;
            Dirty |= DirtyFlags.WorldViewProjection;
        }
    }

    public virtual Matrix View {
        get => _view;
        set {
            _view = value;
            Dirty |= DirtyFlags.WorldViewProjection;
        }
    }

    public virtual Matrix Projection {
        get => _projection;
        set {
            _projection = value;
            Dirty |= DirtyFlags.WorldViewProjection;
        }
    }

    public virtual Matrix WorldViewProj {
        get => WorldViewProjParam.GetMatrix();
        set => WorldViewProjParam.Set(value);
    }

    public virtual Color Color {
        get => ColorParam.GetColor();
        set => ColorParam.Set(value);
    }

    public virtual ColorF ColorF {
        get => ColorParam.GetColorF();
        set => ColorParam.Set(value);
    }

    public virtual Texture2D Texture {
        get => ColorParam.GetTexture2D();
        set => ColorParam.Set(value);
    }

    protected ShaderParameter WorldViewProjParam { get; private set; }
    protected ShaderParameter ColorParam { get; private set; }
    protected ShaderParameter TextureParam { get; private set; }

    protected BitTag Dirty { get; set; }

    public void ChangeTransform(ref Matrix world, ref Matrix view, ref Matrix proj) {
        _world = world;
        _view = view;
        _projection = proj;
        Dirty |= DirtyFlags.WorldViewProjection;
    }

    protected override void Loaded() {
        WorldViewProjParam = RetrieveParameter("WorldViewProj");
        ColorParam = RetrieveParameter("Color");
        TextureParam = RetrieveParameter("Texture");

        // default values
        // TODO  add reset()
        Color = Color.White;
    }

    protected override void PreApply() {
        base.PreApply();

        if (Dirty.Has(DirtyFlags.WorldViewProjection)) {
            //Matrix viewProj = Matrix.Multiply(ref _view, ref _projection);
            //WorldViewProj = Matrix.Multiply(ref _world, ref viewProj);
            Matrix worldView = Matrix.Multiply(ref _world, ref _view);
            WorldViewProj = Matrix.Multiply(ref worldView, ref _projection);
            Dirty -= DirtyFlags.WorldViewProjection;
        }
    }
}
