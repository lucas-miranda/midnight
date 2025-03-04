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

    internal Entity Submit(EntityBuilder builder) {
        Entity e = new(_nextUid);
        _nextUid += 1;

        foreach (Component component in builder.Components) {
            Assert.False(component.Entity.IsDefined);
            component.Entity = e;
            Scene.Components.Register(component);
        }

        // register
        _entries.Add(e);

        return e;
    }
}
