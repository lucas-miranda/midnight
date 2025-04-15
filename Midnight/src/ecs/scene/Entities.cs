using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class Entities {
    private ulong _nextUid = 1;
    private List<Entity> _entries = new();

    public Entities(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public EntityBuilder Create() {
        return new(this);
    }

    internal Entity Submit(EntityBuilder builder, Prototype prototype = null) {
        Entity e = new(_nextUid, prototype);
        _nextUid += 1;

        foreach (Component component in builder.Components) {
            Scene.Components.Add(e, component);
        }

        // register
        _entries.Add(e);

        return e;
    }
}
