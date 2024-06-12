namespace Midnight;

public class SpriteShader : BaseSpriteShader {
    [System.Flags]
    public enum TechniqueIndex {
        VertexColor = 0,
        Texture     = 1 << 0,
        DepthWrite  = 1 << 1,
    }

    private TechniqueSettings _settings;

    public SpriteShader() {
    }

    public override Texture2D Texture {
        get => base.Texture;
        set {
            base.Texture = value;

            Settings = Settings with {
                TextureEnabled = value != null,
            };
        }
    }

    public TechniqueSettings Settings {
        get => _settings;
        set {
            if (value == _settings) {
                return;
            }

            Dirty |= DirtyFlags.TechniqueIndex;
            _settings = value;
        }
    }

    public override SpriteShaderMaterial CreateMaterial() {
        return new(this);
    }

    protected override void PreApply() {
        base.PreApply();

        if (Dirty.Has(DirtyFlags.TechniqueIndex)) {
            CurrentTechnique = Technique(SelectTechnique());
            Dirty -= DirtyFlags.TechniqueIndex;
        }
    }

    protected virtual int SelectTechnique() {
        TechniqueIndex index = TechniqueIndex.VertexColor;

        if (Settings.TextureEnabled) {
            index |= TechniqueIndex.Texture;
        }

        if (Settings.DepthWriteEnabled) {
            index |= TechniqueIndex.DepthWrite;
        }

        return (int) index;
    }

    public record struct TechniqueSettings {
        public bool TextureEnabled { get; init; }
        public bool DepthWriteEnabled { get; init; }
    }
}
