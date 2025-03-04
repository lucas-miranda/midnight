using Midnight.Diagnostics;

namespace Midnight;

public sealed class EntityBuilder {
    private Entities _entities;
    private Components _components = new();
    private bool _submitted;

    internal EntityBuilder(Entities entities) {
        _entities = entities;
        Components = _components.AsReadOnly();
    }

    public ReadOnlyComponents Components { get; }

    public C Add<C>() where C : Component, new() {
        C component = new C();
        _components.Add(component);
        return component;
    }

    public C Add<C>(C component) where C : Component, new() {
        Assert.NotNull(component);
        _components.Add(component);
        return component;
    }

    public EntityBuilder With<C>() where C : Component, new() {
        _components.Add(new C());
        return this;
    }

    public EntityBuilder With<C>(C component) where C : Component {
        _components.Add(component);
        return this;
    }

    public EntityBuilder With<C>(out C component) where C : Component, new() {
        component = new C();
        _components.Add(component);
        return this;
    }

    public Entity Submit() {
        if (_submitted) {
            throw new System.InvalidOperationException("Already submitted.");
        }

        _submitted = true;
        return _entities.Submit(this);
    }
}
