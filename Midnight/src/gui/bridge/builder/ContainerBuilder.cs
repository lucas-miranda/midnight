using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.GUI;

public abstract class ContainerBuilder : ObjectBuilder, System.IDisposable {
    private static Dictionary<System.Type, System.Type> _builder = new();

    //private Dictionary<int, ObjectBuilder> _components = new();
    private List<ObjectBuilder> _children = new();
    private int _index;

    static ContainerBuilder() {
        _builder.Add(typeof(Frame), typeof(FrameBuilder));
        _builder.Add(typeof(Button), typeof(ButtonBuilder));
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

    public ObjectBuilder Create<T>() where T : GUI.Object, new() {
        return RetrieveBuilder(typeof(T));
    }

    public FrameBuilder Frame() {
        return Create<Frame>() as FrameBuilder;
    }

    public ButtonBuilder Button(string label) {
        ButtonBuilder b = Create<Button>() as ButtonBuilder;
        Label l = b.Result.Container.Find<Label>();

        if (l != null) {
            l.Text = label;
        } else {
            b.Result.Container.Add(new Label() { Text = label });
        }

        return b;
    }

    private ObjectBuilder RetrieveBuilder(System.Type type) {
        ObjectBuilder b;

        if (DesignBuilder.IsBuilded) {
            Debug.Assert(_index < _children.Count);
            b = _children[_index];
            Debug.Assert(b.Result.GetType().IsAssignableTo(type), $"Expected '{type.Name}', but get '{b.GetType().Name}'");
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
            IContainer c = (IContainer) Result;
            c.Container.Add(b.Result);
        }

        return b;
    }

    public void Dispose() {
    }
}
