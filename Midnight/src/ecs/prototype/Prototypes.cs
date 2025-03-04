using System.Collections.Generic;

namespace Midnight.GUI;

public class Prototypes {
    private static Prototypes _instance;
    private Dictionary<System.Type, Prototype> _entries = new();

    private Prototypes() {
        Register<PushButtonPrototype>();
        Register<Label, LabelPrototype>();
        Register<FramePrototype>();
    }

    public static Entity Instantiate<T>(Entity? parent = null) {
        if (!_instance._entries.TryGetValue(typeof(T), out Prototype proto)) {
            throw new System.InvalidOperationException($"Prototype for type '{typeof(T)}' not found.");
        }

        Entity entity = proto.Instantiate();

        if (parent.HasValue
         && parent.Value.TryGet<Transform>(out Transform parentTransform)
         && entity.TryGet<Transform>(out Transform transform)
        ) {

            transform.Parent = parentTransform;
        }

        return entity;
    }

    public void Register<T>() where T : Prototype, new() {
        _entries.Add(typeof(T), new T());
    }

    public void Register<T, P>()
        where T : Component
        where P : Prototype, new()
    {
        _entries.Add(typeof(T), new P());
    }

    internal static void Initialize() {
        if (_instance != null) {
            return;
        }

        _instance = new();
    }
}
