using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.GUI;

public abstract class ContainerBuilder : ObjectBuilder, System.IDisposable {
    private static Dictionary<System.Type, System.Type> _builder = new();

    //private Dictionary<int, ObjectBuilder> _components = new();
    private List<ObjectBuilder> _children = new();
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

        foreach (ObjectBuilder b in _children) {
            b.Reset();
        }

        //_children.Clear();
    }

    public ObjectBuilder Create<T>() where T : Prototype, new() {
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

    private ObjectBuilder RetrieveBuilder(System.Type type) {
        ObjectBuilder b;

        if (DesignBuilder.IsBuilded) {
            Assert.True(_index < _children.Count);
            b = _children[_index];
            Assert.True(b.Result.GetType().IsAssignableTo(type), $"Expected '{type.Name}', but get '{b.GetType().Name}'");
            _index += 1;

            b.Prepare();
        } else {
            b = System.Activator.CreateInstance(
                    _builder[type],
                    new object[] { DesignBuilder }
                ) as ObjectBuilder;

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
