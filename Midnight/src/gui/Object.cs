
namespace Midnight.GUI;

public abstract class Object : IRenderable {
    private Vector2 _size;

    public Vector2 Size {
        get => _size;
        set {
            _size = value;
            SizeChanged();
        }
    }

    public abstract void Render(DeltaTime dt, RenderingServer rendering);

    public virtual string TreeToString() {
        return $"[{GetType().Name}]";
    }

    protected abstract void SizeChanged();
}
