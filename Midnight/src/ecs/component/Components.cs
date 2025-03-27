using System.Collections;
using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

public class Components : IEnumerable<Component> {
    private List<Component> _entries = new();

    public int Count => _entries.Count;

    public C Get<C>() where C : Component {
        foreach (Component component in _entries) {
            if (component is C c) {
                return c;
            }
        }

        return null;
    }

    public (C1, C2)? Get<C1, C2>()
        where C1 : Component
        where C2 : Component
    {
        (C1, C2) result = default;

        foreach (Component component in _entries) {
            if (component is C1 c1) {
                result = (c1, result.Item2);
            } else if (component is C2 c2) {
                result = (result.Item1, c2);
            }
        }

        if (result.Item1 == null || result.Item2 == null) {
            return null;
        }

        return result;
    }

    public (C1, C2, C3)? Get<C1, C2, C3>()
        where C1 : Component
        where C2 : Component
        where C3 : Component
    {
        (C1, C2, C3) result = default;

        foreach (Component component in _entries) {
            if (component is C1 c1) {
                result = (c1, result.Item2, result.Item3);
            } else if (component is C2 c2) {
                result = (result.Item1, c2, result.Item3);
            } else if (component is C3 c3) {
                result = (result.Item1, result.Item2, c3);
            }
        }

        if (result.Item1 == null || result.Item2 == null || result.Item3 == null) {
            return null;
        }

        return result;
    }

    public C Add<C>(C c) where C : Component {
        _entries.Add(c);
        return c;
    }

    public bool Remove<C>(C c) where C : Component {
        return _entries.Remove(c);
    }

    public List<C> GetAll<C>() where C : Component {
        List<C> buffer = new();

        foreach (Component component in _entries) {
            if (component is C c) {
                buffer.Add(c);
            }
        }

        return buffer;
    }

    public void GetAll<C>(ref List<C> buffer) where C : Component {
        if (buffer == null) {
            buffer = new();
        }

        buffer.Clear();
        foreach (Component component in _entries) {
            if (component is C c) {
                buffer.Add(c);
            }
        }
    }

    public List<Component> GetAll<C1, C2>()
        where C1 : Component
        where C2 : Component
    {
        List<Component> buffer = new();

        foreach (Component component in _entries) {
            if (component is C1 || component is C2) {
                buffer.Add(component);
            }
        }

        return buffer;
    }

    public void GetAll<C1, C2>(ref List<Component> buffer)
        where C1 : Component
        where C2 : Component
    {
        if (buffer == null) {
            buffer = new();
        }

        buffer.Clear();
        foreach (Component component in _entries) {
            if (component is C1 || component is C2) {
                buffer.Add(component);
            }
        }
    }

    public List<Component> GetAll(System.Type componentType) {
        Assert.NotNull(componentType);
        Assert.Is<Component>(componentType);
        List<Component> buffer = new();

        foreach (Component component in _entries) {
            if (componentType.IsAssignableFrom(component.GetType())) {
                buffer.Add(component);
            }
        }

        return buffer;
    }

    public void GetAll(System.Type componentType, ref List<Component> buffer) {
        if (buffer == null) {
            buffer = new();
        }

        buffer.Clear();
        foreach (Component component in _entries) {
            if (componentType.IsAssignableFrom(component.GetType())) {
                buffer.Add(component);
            }
        }
    }

    public bool Contains(Component component) {
        return _entries.Contains(component);
    }

    public void CopyTo(Components components) {
        components._entries.AddRange(_entries);
    }

    public void Clear() {
        _entries.Clear();
    }

    public ReadOnlyComponents AsReadOnly() {
        return new(this);
    }

    public IEnumerator<Component> GetEnumerator() {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public override string ToString() {
        string s = "";

        foreach (Component c in _entries) {
            s += c.GetType().Name + ";";
        }

        return s;
    }
}
