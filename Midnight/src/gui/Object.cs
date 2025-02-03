
namespace Midnight.GUI;

public abstract class Object : IRenderable {
    private Size2 _size;

    public Size2 Size {
        get => _size;
        set {
            if (value == _size) {
                return;
            }

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
