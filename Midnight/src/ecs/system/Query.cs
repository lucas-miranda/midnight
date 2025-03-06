using System.Collections.Generic;
using System.Collections;

namespace Midnight;

public abstract class ComponentQuery {
    public abstract bool Execute(Components components);
}

public class Query<C> : ComponentQuery where C : Component {
    private C _entry;

    public C Entry => _entry;

    public override bool Execute(Components components) {
        _entry = components.Get<C>();
        return _entry != null;
    }

    public static implicit operator C(Query<C> query) {
        return query.Entry;
    }
}

public class MultiQuery<C> : ComponentQuery, IEnumerable<C>
    where C : Component
{
    private List<C> _entries = new();

    public List<C> Entries => _entries;

    public override bool Execute(Components components) {
        _entries.Clear();
        components.GetAll<C>(ref _entries);
        return !_entries.IsEmpty();
    }

    public IEnumerator<C> GetEnumerator() {
        return _entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public static implicit operator List<C>(MultiQuery<C> query) {
        return query.Entries;
    }
}
