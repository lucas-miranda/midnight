using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class SceneComponents {
    private Components _allComponents = new();
    private List<Component> _buffer = new();
                            //_stagingComponents = new();
    private Dictionary<Entity, Components> _entityComponents = new();
                                           //_stagingEntityComponents = new();

    public SceneComponents(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public Components Get(Entity entity) {
        if (!_entityComponents.TryGetValue(entity, out Components components)) {
            return null;
        }

        return components;
    }

    public C Add<C>(Entity entity, C component) where C : Component {
        Assert.NotNull(component);
        Assert.False(component.Entity.IsDefined);
        component.Entity = entity;
        Register(component);
        return component;
    }

    public C Add<C>(Entity entity) where C : Component, new() {
        return Add(entity, new C());
    }

    public C Query<C>(Entity entity) where C : Component {
        C result;

        if (_entityComponents.TryGetValue(entity, out Components components)) {
            result = components.Get<C>();
        } else {
            result = default;
        }

        return result;
    }

    public (C1, C2) Query<C1, C2>(Entity entity)
        where C1 : Component
        where C2 : Component
    {
        (C1, C2) result;

        if (_entityComponents.TryGetValue(entity, out Components components)) {
            result = (components.Get<C1>(), components.Get<C2>());
        } else {
            result = default;
        }

        return result;
    }

    public (C1, C2, C3) Query<C1, C2, C3>(Entity entity)
        where C1 : Component
        where C2 : Component
        where C3 : Component
    {
        (C1, C2, C3) result;

        if (_entityComponents.TryGetValue(entity, out Components components)) {
            result = (components.Get<C1>(), components.Get<C2>(), components.Get<C3>());
        } else {
            result = default;
        }

        return result;
    }

    public IList<C> QueryAll<C>() where C : Component {
        return _allComponents.GetAll<C>();
    }

    public IList<(C1, C2)> QueryAll<C1, C2>()
        where C1 : Component
        where C2 : Component
    {
        List<(C1, C2)> result = new();

        foreach (Components components in _entityComponents.Values) {
            (C1, C2)? c = components.Get<C1, C2>();

            if (c.HasValue) {
                result.Add(c.Value);
            }
        }

        return result;
    }

    public IList<(C1, C2, C3)> QueryAll<C1, C2, C3>()
        where C1 : Component
        where C2 : Component
        where C3 : Component
    {
        List<(C1, C2, C3)> result = new();

        foreach (Components components in _entityComponents.Values) {
            (C1, C2, C3)? c = components.Get<C1, C2, C3>();

            if (c.HasValue) {
                result.Add(c.Value);
            }
        }

        return result;
    }

    public IList<Component> QueryAll(System.Type componentType) {
        _allComponents.GetAll(componentType, ref _buffer);
        return _buffer;
    }

    public IList<C> QueryAll<C>(Entity entity) where C : Component {
        List<C> result = new();

        if (_entityComponents.TryGetValue(entity, out Components components)) {
            components.GetAll<C>(ref result);
        }

        return result;
    }

    public void Flush() {
        /*
        foreach (Component component in _stagingComponents) {
            _allComponents.Add(component);

            if (_stagingEntityComponents.TryGetValue(component.Entity, out Components stagingEntityComponents)) {
                if (!_entityComponents.TryGetValue(component.Entity, out Components entityComponents)) {
                    entityComponents = new();
                    _entityComponents[component.Entity] = entityComponents;
                }

                stagingEntityComponents.CopyTo(entityComponents);
                stagingEntityComponents.Clear();
            }
        }

        _stagingComponents.Clear();
        */
    }

    public IEnumerable<KeyValuePair<Entity, Components>> EntityComponents() {
        foreach (KeyValuePair<Entity, Components> entry in _entityComponents) {
            yield return entry;
        }
    }

    /*
    internal void Register(Component component) {
        Assert.False(_allComponents.Contains(component));
        Assert.False(_stagingComponents.Contains(component));
        _stagingComponents.Add(component);

        Components components;

        if (!_entityComponents.TryGetValue(component.Entity, out components)) {
            if (!_stagingEntityComponents.TryGetValue(component.Entity, out components)) {
                components = new();
                _stagingEntityComponents[component.Entity] = components;
            }
        }

        Assert.False(components.Contains(component));
        components.Add(component);
        Scene.Systems.Send(new ECS.ComponentAddedEvent(component));
    }
    */

    internal void Register(Component component) {
        Assert.False(_allComponents.Contains(component));
        _allComponents.Add(component);

        Components components;

        if (!_entityComponents.TryGetValue(component.Entity, out components)) {
            components = new();
            _entityComponents[component.Entity] = components;
        }

        Assert.False(components.Contains(component));
        components.Add(component);
        Scene.Systems.Send(new ECS.ComponentAddedEvent(component));
    }
}
