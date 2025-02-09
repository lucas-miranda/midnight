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
        Label l = b.Result.Find<Label>();

        if (l != null) {
            l.Value = label;
        } else {
            b.Result.Add(new Label() { Value = label });
        }

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
            Assert.Is<Container>(Result);
            Container c = (Container) Result;
            c.Add(b.Result);
        }

        return b;
    }
}
