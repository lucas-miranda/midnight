using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Midnight;

public sealed class EntityBuilder {
    private Entities _entities;
    private List<Component> _components = new();

    internal EntityBuilder(Entities entities) {
        _entities = entities;
        Components = _components.AsReadOnly();
    }

    public ReadOnlyCollection<Component> Components { get; }

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
        return _entities.Submit(this);
    }
}
