using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.GUI;

public abstract class ContainerBuilder : WidgetBuilder, System.IDisposable {
    private static Dictionary<System.Type, System.Type> _builder = new();

    private List<WidgetBuilder> _children = new();
    private int _index;

    static ContainerBuilder() {
        foreach ((System.Type PrototypeType, WidgetPrototypeRegistryAttribute Attr) entry in ReflectionHelper.IterateTypesWithAttribute<WidgetPrototypeRegistryAttribute>()) {
            _builder.Add(entry.PrototypeType, entry.Attr.BuilderType);
        }
    }

    public ContainerBuilder(DesignBuilder designBuilder) : base(designBuilder) {
    }

    public override void Reset() {
        base.Reset();
        _index = 0;

        foreach (WidgetBuilder b in _children) {
            b.Reset();
        }
    }

    public override void Ended() {
        foreach (WidgetBuilder builder in _children) {
            builder.Ended();
        }

        base.Ended();
    }

    public WidgetBuilder Create<T>() where T : Prototype, new() {
        return RetrieveBuilder(typeof(T));
    }

    public ContainerBuilder With(Layout layout) {
        Widget widget = Result.Get<Widget>();

        if (widget != null) {
            widget.Layout = layout;
        }

        return this;
    }

    #region Components

    public FrameBuilder Frame(Layout layout = Layout.BoxHorizontal) {
        Logger.DebugLine("Frame");
        FrameBuilder b = Create<FramePrototype>() as FrameBuilder;
        return (FrameBuilder) b.With(layout);
    }

    public PushButtonBuilder PushButton(string label) {
        Logger.DebugLine($"PushButton '{label}'");
        PushButtonBuilder b = Create<PushButtonPrototype>() as PushButtonBuilder;
        return b.Label(label);
    }

    public ItemListBuilder ItemList(Layout layout = Layout.BoxHorizontal) {
        Logger.DebugLine("ItemList");
        ItemListBuilder b = Create<ItemListPrototype>() as ItemListBuilder;
        return (ItemListBuilder) b.With(layout);
    }

    public LabelBuilder Label(string text) {
        Logger.DebugLine($"Label '{text}'");
        LabelBuilder b = Create<LabelPrototype>() as LabelBuilder;
        return b.With(text);
    }

    #endregion Components

    public void Dispose() {
    }

    private WidgetBuilder RetrieveBuilder(System.Type prototypeType) {
        WidgetBuilder b;

        if (DesignBuilder.IsBuilded) {
            Assert.True(_index < _children.Count);
            b = _children[_index];

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
        }

        // register as child
        Transform transform = Result.Get<Transform>();
        Transform childTransform = b.Result.Get<Transform>();

        Assert.NotNull(transform);
        Assert.NotNull(childTransform);
        Assert.True(transform != childTransform, $"{Result} != {b.Result}");

        childTransform.Parent = transform;

        return b;
    }
}
