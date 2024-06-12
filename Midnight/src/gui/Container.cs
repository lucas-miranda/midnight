using System.Collections.Generic;
using System.Collections;

namespace Midnight.GUI;

public class Container : IEnumerable<Object> {
    private List<Object> _entries = new();

    public void Add(Object obj) {
        _entries.Add(obj);
    }

    public bool Remove(Object obj) {
        return _entries.Remove(obj);
    }

    public T Find<T>() where T : Object {
        foreach (Object entry in _entries) {
            if (entry is T t) {
                return t;
            } else if (entry is IContainer holder) {
                T result = holder.Container.Find<T>();
                if (result != null) {
                    return result;
                }
            }
        }

        return null;
    }

    public IEnumerator<Object> GetEnumerator() {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
