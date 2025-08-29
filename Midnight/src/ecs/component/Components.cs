using System.Collections;
using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public class Components : IEnumerable<Component> {
    public event System.Action<Component> OnAdded, OnRemoved;

    private Dictionary<System.Type, List<Component>> _entries = new();

    public Components() {
    }

    public Components(Entity entity) {
        Entity = entity;
    }

    public Entity? Entity { get; internal set; }
    public int Count { get; private set; }

    public C Get<C>() where C : Component {
        List<Component> components = GetOrCreateComponentList(typeof(C));

        if (components.IsEmpty()) {
            return null;
        }

        return (C) components[0];
    }

    public (C1, C2)? Get<C1, C2>()
        where C1 : Component
        where C2 : Component
    {
        List<Component> componentsA = GetOrCreateComponentList(typeof(C1)),
                        componentsB = GetOrCreateComponentList(typeof(C2));

        if (componentsA.IsEmpty() || componentsB.IsEmpty()) {
            return null;
        }

        return ((C1) componentsA[0], (C2) componentsB[0]);
    }

    public (C1, C2, C3)? Get<C1, C2, C3>()
        where C1 : Component
        where C2 : Component
        where C3 : Component
    {
        List<Component> componentsA = GetOrCreateComponentList(typeof(C1)),
                        componentsB = GetOrCreateComponentList(typeof(C2)),
                        componentsC = GetOrCreateComponentList(typeof(C3));

        if (componentsA.IsEmpty() || componentsB.IsEmpty() || componentsC.IsEmpty()) {
            return null;
        }

        return ((C1) componentsA[0], (C2) componentsB[0], (C3) componentsC[0]);
    }

    public bool TryGet<C>(out C c) where C : Component {
        if (!TryGetComponentList(typeof(C), out List<Component> list) || list.IsEmpty()) {
            c = null;
            return false;
        }

        c = (C) list[0];
        return true;
    }

    public C Add<C>(C c) where C : Component {
        System.Type type = c.GetType(),
                    endType = typeof(Component).BaseType;

        while (type != endType) {
            List<Component> components = GetOrCreateComponentList(type);
            components.Add(c);
            type = type.BaseType;
        }

        if (Entity.HasValue) {
            c.Entity = Entity.Value;
        }

        OnAdded?.Invoke(c);
        return c;
    }

    public bool Remove<C>(C c) where C : Component {
        bool removed = false;
        System.Type type = c.GetType(),
                    endType = typeof(Component).BaseType;

        while (type != endType) {
            if (TryGetComponentList(type, out List<Component> components)) {
                if (components.Remove(c)) {
                    removed = true;
                }
            }

            type = type.BaseType;
        }

        OnRemoved?.Invoke(c);
        c.Entity = Midnight.Entity.None;
        return removed;
    }

    public List<C> GetAll<C>() where C : Component {
        List<C> buffer;

        if (TryGetComponentList(typeof(C), out List<Component> components)) {
            buffer = new(components.Count);
            buffer.AddRange(components);
        } else {
            buffer = new();
        }

        return buffer;
    }

    public void GetAll<C>(ref List<C> buffer) where C : Component {
        if (buffer == null) {
            buffer = new();
        } else {
            buffer.Clear();
        }

        if (TryGetComponentList(typeof(C), out List<Component> components)) {
            buffer.AddRange(components);
        }
    }

    public List<Component> GetAll<C1, C2>()
        where C1 : Component
        where C2 : Component
    {
        List<Component> buffer = new();

        if (TryGetComponentList(typeof(C1), out List<Component> components)) {
            buffer.AddRange(components);
        }

        if (TryGetComponentList(typeof(C2), out components)) {
            buffer.AddRange(components);
        }

        return buffer;
    }

    public void GetAll<C1, C2>(ref List<Component> buffer)
        where C1 : Component
        where C2 : Component
    {
        if (buffer == null) {
            buffer = new();
        } else {
            buffer.Clear();
        }

        if (TryGetComponentList(typeof(C1), out List<Component> components)) {
            buffer.AddRange(components);
        }

        if (TryGetComponentList(typeof(C2), out components)) {
            buffer.AddRange(components);
        }
    }

    public List<Component> GetAll(System.Type componentType) {
        Assert.NotNull(componentType);
        Assert.Is<Component>(componentType);
        List<Component> buffer;

        if (TryGetComponentList(componentType, out List<Component> components)) {
            buffer = new(components);
        } else {
            buffer = new();
        }

        return buffer;
    }

    public void GetAll(System.Type componentType, ref List<Component> buffer) {
        if (buffer == null) {
            buffer = new();
        } else {
            buffer.Clear();
        }

        if (TryGetComponentList(componentType, out List<Component> components)) {
            buffer.AddRange(components);
        }
    }

    public bool Contains(Component component) {
        Assert.NotNull(component);
        if (!TryGetComponentList(component.GetType(), out List<Component> components)) {
            return false;
        }

        return components.Contains(component);
    }

    public void CopyTo(Components other) {
        foreach (KeyValuePair<System.Type, List<Component>> entry in _entries) {
            List<Component> components = other.GetOrCreateComponentList(entry.Key);
            components.AddRange(entry.Value);
        }
    }

    public void Clear() {
        _entries.Clear();
    }

    public ReadOnlyComponents AsReadOnly() {
        return new(this);
    }

    public IEnumerator<Component> GetEnumerator() {
        return GetOrCreateComponentList(typeof(Component)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public override string ToString() {
        string s = "";

        foreach (Component c in GetOrCreateComponentList(typeof(Component))) {
            s += c.GetType().Name + ";";
        }

        return s;
    }

    private List<Component> GetOrCreateComponentList(System.Type componentType) {
        if (!_entries.TryGetValue(componentType, out List<Component> componentList)) {
            componentList = new();
            _entries[componentType] = componentList;
        }

        return componentList;
    }

    private bool TryGetComponentList(System.Type componentType,  out List<Component> componentList) {
        return _entries.TryGetValue(componentType, out componentList);
    }
}
