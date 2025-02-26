using System.Collections.Generic;

namespace Midnight;

public sealed class SceneComponents {
    private List<Component> _components = new(),
                            _buffer = new();

    public IList<C> Query<C>() where C : Component {
        List<C> buffer = new();

        buffer.Clear();
        foreach (Component component in _components) {
            if (component is C c) {
                buffer.Add(c);
            }
        }

        return buffer;
    }

    public IList<Component> Query(System.Type componentType) {
        _buffer.Clear();
        foreach (Component component in _components) {
            if (componentType.IsAssignableFrom(component.GetType())) {
                _buffer.Add(component);
            }
        }

        return _buffer;
    }

    internal void Register(Component component) {
        _components.Add(component);
    }
}
