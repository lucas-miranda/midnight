using System.Collections;
using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public class ReadOnlyComponents : IEnumerable<Component> {
    private Components _components;

    public ReadOnlyComponents(Components components) {
        Assert.NotNull(components);
        _components = components;
    }

    public int Count => _components.Count;

    public C Get<C>() where C : Component {
        return _components.Get<C>();
    }

    public List<C> GetAll<C>() where C : Component {
        return _components.GetAll<C>();
    }

    public void GetAll<C>(ref List<C> buffer) where C : Component {
        _components.GetAll<C>(ref buffer);
    }

    public List<Component> GetAll(System.Type componentType) {
        return _components.GetAll(componentType);
    }

    public void GetAll(System.Type componentType, ref List<Component> buffer) {
        _components.GetAll(componentType, ref buffer);
    }

    public IEnumerator<Component> GetEnumerator() {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
