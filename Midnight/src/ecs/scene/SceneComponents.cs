using System.Collections;
using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class SceneComponents : IEnumerable<Component> {
    private Components _allComponents = new();
    private List<Component> _buffer = new();

    public SceneComponents(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public IList<C> QueryAll<C>() where C : Component {
        return _allComponents.GetAll<C>();
    }

    public IList<(C1, C2)> QueryAll<C1, C2>()
        where C1 : Component
        where C2 : Component
    {
        List<(C1, C2)> result = new();

        foreach (Entity e in Scene.Entities) {
            (C1, C2)? c = e.Components.Get<C1, C2>();

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

        foreach (Entity e in Scene.Entities) {
            (C1, C2, C3)? c = e.Components.Get<C1, C2, C3>();

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

    public IEnumerable<KeyValuePair<Entity, Components>> EntityComponents() {
        foreach (Entity e in Scene.Entities) {
            yield return new(e, e.Components);
        }
    }

    public IEnumerator<Component> GetEnumerator() {
        return _allComponents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    internal void EntityCreated(Entity entity) {
        Assert.True(entity.IsDefined);

        // ensure every component has entity before running anything
        foreach (Component component in entity.Components) {
            component.Entity = entity;
        }

        foreach (Component component in entity.Components) {
            EntityComponentAdded(component);
        }

        entity.Components.OnAdded += EntityComponentAdded;
        entity.Components.OnRemoved += EntityComponentRemoved;
    }

    internal void EntityRemoved(Entity entity) {
        Assert.True(entity.IsDefined);

        entity.Components.OnAdded -= EntityComponentAdded;
        entity.Components.OnRemoved -= EntityComponentRemoved;

        foreach (Component component in entity.Components) {
            EntityComponentRemoved(component);
        }
    }

    internal void EntityComponentAdded(Component component) {
        Assert.False(_allComponents.Contains(component));
        _allComponents.Add(component);
        Scene.Systems.Send(new ECS.ComponentAddedEvent(component));
    }

    internal void EntityComponentRemoved(Component component) {
        Assert.True(_allComponents.Contains(component));
        _allComponents.Remove(component);
        Scene.Systems.Send(new ECS.ComponentRemovedEvent(component));
    }
}
