using System.Collections.Generic;
using System.Reflection;
using Midnight.Diagnostics;

namespace Midnight.GUI;

public class Prototypes {
    private static Prototypes _instance;
    private Dictionary<System.Type, Prototype> _entries = new();

    private Prototypes() {
        // register any class which derives from Prototype
        foreach (System.Type type in ReflectionHelper.IterateDescendantsTypes<Prototype>()) {
            PrototypeRegistryAttribute attr = type.GetCustomAttribute<PrototypeRegistryAttribute>();

            if (attr != null && attr.ComponentType != null) {
                Register(attr.ComponentType, type);
            } else {
                Register(type);
            }
        }
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
        P prototype = new P();

        _entries.Add(typeof(T), prototype);
        _entries.Add(typeof(P), prototype);
    }

    public void Register(System.Type prototypeType) {
        Assert.Is<Prototype>(prototypeType);
        _entries.Add(prototypeType, (Prototype) System.Activator.CreateInstance(prototypeType));
    }

    public void Register(System.Type componentType, System.Type prototypeType) {
        Assert.Is<Component>(componentType);
        Assert.Is<Prototype>(prototypeType);
        Prototype prototype = (Prototype) System.Activator.CreateInstance(prototypeType);

        _entries.Add(componentType, prototype);
        _entries.Add(prototypeType, prototype);
    }

    internal static void Initialize() {
        if (_instance != null) {
            return;
        }

        _instance = new();
    }
}
