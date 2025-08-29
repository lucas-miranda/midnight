using System.Collections;
using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class Entities : IEnumerable<Entity> {
    private ulong _nextUid = 1;
    private HashSet<Entity> _entries = new();

    public Entities(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public EntityBuilder Create() {
        return new(this);
    }

    public void Register(Entity e) {
        if (!_entries.Add(e)) {
            throw new System.InvalidOperationException($"{e} already registered.");
        }

        Scene.Components.EntityCreated(e);
    }

    public void RegisterOnce(Entity e) {
        if (_entries.Add(e)) {
            Scene.Components.EntityCreated(e);
        }
    }

    public bool Remove(Entity e) {
        if (!_entries.Remove(e)) {
            return false;
        }

        Scene.Components.EntityRemoved(e);
        return true;
    }

    public bool Has(Entity entity) {
        return _entries.Contains(entity);
    }

    public IEnumerator<Entity> GetEnumerator() {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    internal Entity Submit(EntityBuilder builder, Prototype prototype, Components components) {
        Assert.NotNull(components);
        Entity e = new(_nextUid, prototype, components);
        _nextUid += 1;
        Register(e);
        return e;
    }
}
