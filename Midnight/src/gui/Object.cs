
namespace Midnight.GUI;

public abstract class Object : IRenderable, ITransformObject {
    private Size2 _size;

    public Object() {
        Transform = new() {
            Owner = this,
        };
    }

    public Transform2D Transform { get; }

    public Size2 Size {
        get => _size;
        set {
            if (value == _size) {
                return;
            }

            _size = value;
            RequestLayout();
            SizeChanged();
        }
    }

    public Rectangle Bounds => new(Transform.GlobalPosition, Size);
    public bool IsLayoutRequested { get; private set; } = true;

    public virtual void Update(DeltaTime dt) {
        TryLayout();
    }

    public abstract void Render(DeltaTime dt, RenderingServer rendering);

    public void AddedContainer(Container c) {
    }

    public void RemovedContainer(Container c) {
    }

    public void ForceLayout() {
        Layout();
        IsLayoutRequested = false;
    }

    public void TryLayout() {
        if (IsLayoutRequested) {
            Layout();
            IsLayoutRequested = false;
        }
    }

    public void RequestLayout() {
        IsLayoutRequested = true;
    }

    public virtual string TreeToString() {
        return $"[{GetType().Name}]";
    }

    protected abstract void Layout();
    protected abstract void SizeChanged();
}
