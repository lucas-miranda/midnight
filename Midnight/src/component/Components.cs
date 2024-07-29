using System.Collections.Generic;
using System.Collections;
using Midnight.Diagnostics;

namespace Midnight;

public class Components : IEnumerable<Component> {
    private Dictionary<System.Type, List<Component>> _entries = new();

    public Components(Entity entity) {
        Entity = entity;
    }

    public Entity Entity { get; }

    public C Create<C>() where C : Component, new() {
        return Add(new C());
    }

    public C Add<C>(C component) where C : Component {
        List<Component> storage = GetOrCreateStorage(typeof(C));
        Assert.NotNull(storage, "Component storage must exist at this point.");
        storage.Add(component);
        ComponentAdded(component);
        return component;
    }

    public bool Remove<C>(C component) where C : Component {
        if (!TryGetStorage(typeof(C), out List<Component> storage)) {
            return false;
        }

        storage.Remove(component);
        return true;
    }

    public C Get<C>() where C : Component {
        if (!TryGetStorage(typeof(C), out List<Component> storage) || storage.Count == 0) {
            return null;
        }

        return storage[0] as C;
    }

    public void Clear() {
        foreach (List<Component> storage in _entries.Values) {
            foreach (Component component in storage) {
                // TODO  call methods on Component
            }

            storage.Clear();
        }
    }

    public List<Component> GetStorage<C>() where C : Component {
        if (!TryGetStorage(typeof(C), out List<Component> storage)) {
            return null;
        }

        return storage;
    }

    public IEnumerator<C> Iter<C>() where C : Component {
        if (!TryGetStorage(typeof(C), out List<Component> storage)) {
            yield break;
        }

        foreach (Component component in storage) {
            yield return component as C;
        }
    }

    public IEnumerator<Component> GetEnumerator() {
        foreach (List<Component> storage in _entries.Values) {
            foreach (Component component in storage) {
                yield return component;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    private void ComponentAdded(Component component) {
        Entity.ComponentAdded(component);
    }

    private void ComponentRemoved(Component component) {
        Entity.ComponentRemoved(component);
    }

    private bool TryGetStorage(System.Type type, out List<Component> storage) {
        return _entries.TryGetValue(type, out storage);
    }

    private List<Component> GetOrCreateStorage(System.Type type) {
        if (!_entries.TryGetValue(type, out List<Component> storage)) {
            storage = new();
            _entries.Add(type, storage);
        }

        return storage;
    }
}
