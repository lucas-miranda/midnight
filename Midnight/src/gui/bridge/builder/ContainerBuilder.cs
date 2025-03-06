using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.GUI;

public abstract class ContainerBuilder : WidgetBuilder, System.IDisposable {
    private static Dictionary<System.Type, System.Type> _builder = new();

    //private Dictionary<int, ObjectBuilder> _components = new();
    private List<WidgetBuilder> _children = new();
    private int _index;

    static ContainerBuilder() {
        _builder.Add(typeof(FramePrototype), typeof(FrameBuilder));
        _builder.Add(typeof(PushButtonPrototype), typeof(PushButtonBuilder));
    }

    public ContainerBuilder(DesignBuilder designBuilder) : base(designBuilder) {
        /*
        if (!(Result is IContainer)) {
            throw new System.InvalidOperationException("Result must be an IContainer.");
        }
        */
    }

    public override void Reset() {
        base.Reset();
        _index = 0;

        foreach (WidgetBuilder b in _children) {
            b.Reset();
        }

        //_children.Clear();
    }

    public WidgetBuilder Create<T>() where T : Prototype, new() {
        return RetrieveBuilder(typeof(T));
    }

    public FrameBuilder Frame() {
        return Create<FramePrototype>() as FrameBuilder;
    }

    public PushButtonBuilder PushButton(string label) {
        PushButtonBuilder b = Create<PushButtonPrototype>() as PushButtonBuilder;
        Components components = b.Result.GetComponents();
        Transform transform = components.Get<Transform>();
        Label childLabel = transform.FindFirstChildWithComponent<Label>();
        DrawableDisplayer displayer = null;

        if (childLabel == null) {
            Entity labelEntity = Prototypes.Instantiate<Label>(b.Result);
            displayer = labelEntity.Get<DrawableDisplayer>();
        } else {
            displayer = childLabel.Entity.Get<DrawableDisplayer>();
        }

        Assert.NotNull(displayer);
        Assert.Is<StringDrawable>(displayer.Drawable);

        // set label text value
        StringDrawable text = (StringDrawable) displayer.Drawable;
        text.Value = label;

        return b;
    }

    public void Dispose() {
    }

    private WidgetBuilder RetrieveBuilder(System.Type prototypeType) {
        WidgetBuilder b;

        if (DesignBuilder.IsBuilded) {
            Assert.True(_index < _children.Count);
            b = _children[_index];
            //System.Type builderType = _builder[type];
            Assert.True(
                b.Result.Prototype != null && b.Result.Prototype.GetType().IsAssignableTo(prototypeType),
                $"Expected '{prototypeType.Name}', but get '{b.Result.GetType().Name}'"
            );
            _index += 1;

            b.Prepare();
        } else {
            b = System.Activator.CreateInstance(
                    _builder[prototypeType],
                    new object[] { DesignBuilder }
                ) as WidgetBuilder;

            _children.Add(b);

            b.Prepare();

            // register as child
            Transform transform = Result.Get<Transform>();
            Transform childTransform = b.Result.Get<Transform>();

            Assert.NotNull(transform);
            Assert.NotNull(childTransform);
            Assert.True(transform != childTransform, $"{Result} != {b.Result}");

            childTransform.Parent = transform;
        }

        return b;
    }
}
