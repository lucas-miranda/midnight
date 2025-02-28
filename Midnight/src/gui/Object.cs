
namespace Midnight.GUI;

public abstract class Object : IRenderable, ITransformObject {
#if DEBUG
    public static DebugOptions DefaultDebug;
#endif

    private Size2 _size;
#if DEBUG
    private RectangleDrawable _marginDebug, _paddingDebug;
#endif

    public Object() {
        Transform = new() {
            Owner = this,
        };

#if DEBUG
        const float debugOpacity = .2f;
        _marginDebug = new() {
            Color = 0xFF4757FF,
            Opacity = debugOpacity,
            Filled = true,
        };

        _paddingDebug = new() {
            Color = 0x2ED573FF,
            Opacity = debugOpacity,
            Filled = true,
        };
#endif
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
    public Spacing Margin { get; set; }
    public Spacing Padding { get; set; }
    public bool IsLayoutRequested { get; private set; } = true;
#if DEBUG
    public DebugOptions Debug { get; set; } = DefaultDebug;
#endif

    public virtual void Input(InputEvent e) {
    }

    public virtual void Update(DeltaTime dt) {
        TryLayout();
    }

    public virtual void Render(DeltaTime dt) {
#if DEBUG
        if (Debug.DrawMargin) {
            // top
            _marginDebug.Transform.Position = new Vector2(-Margin.Left, -Margin.Top);
            _marginDebug.Size = new(Size.Width + Margin.Horizontal, Margin.Top);

            _marginDebug.Draw(dt, new DrawParams { Transform = Transform });

            // right
            _marginDebug.Transform.Position = new Vector2(Size.Width, 0.0f);
            _marginDebug.Size = new(Margin.Right, Size.Height);

            _marginDebug.Draw(dt, new DrawParams { Transform = Transform });

            // bottom
            _marginDebug.Transform.Position = new Vector2(-Margin.Left, Size.Height);
            _marginDebug.Size = new(Size.Width + Margin.Horizontal, Margin.Bottom);

            _marginDebug.Draw(dt, new DrawParams { Transform = Transform });

            // left
            _marginDebug.Transform.Position = new Vector2(-Margin.Left, 0.0f);
            _marginDebug.Size = new(Margin.Left, Size.Height);

            _marginDebug.Draw(dt, new DrawParams { Transform = Transform });
        }

        if (Debug.DrawPadding) {
            // top
            _paddingDebug.Transform.Position = Vector2.Zero;
            _paddingDebug.Size = new(Size.Width, Padding.Top);

            _paddingDebug.Draw(dt, new DrawParams { Transform = Transform });

            // right
            _paddingDebug.Transform.Position = new Vector2(Size.Width - Padding.Right, Padding.Top);
            _paddingDebug.Size = new(Padding.Right, Size.Height - Padding.Vertical);

            _paddingDebug.Draw(dt, new DrawParams { Transform = Transform });

            // bottom
            _paddingDebug.Transform.Position = new Vector2(0.0f, Size.Height - Padding.Bottom);
            _paddingDebug.Size = new(Size.Width, Padding.Bottom);

            _paddingDebug.Draw(dt, new DrawParams { Transform = Transform });

            // left
            _paddingDebug.Transform.Position = new Vector2(0.0f, Padding.Top);
            _paddingDebug.Size = new(Padding.Left, Size.Height - Padding.Vertical);

            _paddingDebug.Draw(dt, new DrawParams { Transform = Transform });
        }
#endif
    }

    public void AddedContainer(Container c) {
    }

    public void RemovedContainer(Container c) {
    }

    public void ForceLayout() {
        ExecuteLayout();
        IsLayoutRequested = false;
    }

    public void TryLayout() {
        if (IsLayoutRequested) {
            ExecuteLayout();
            IsLayoutRequested = false;
        }
    }

    public void RequestLayout() {
        IsLayoutRequested = true;
    }

    public virtual string TreeToString() {
        return $"[{GetType().Name}]";
    }

    protected abstract void ExecuteLayout();
    protected abstract void SizeChanged();

#if DEBUG
    public struct DebugOptions {
        public bool DrawMargin, DrawPadding;
    }
#endif
}
