using System.Collections.Generic;
using System.Collections;

namespace Midnight;

/// <summary>
/// Holds information about <see cref="Component"/> query.
/// </summary>
public abstract class ComponentQuery {
    /// <summary>
    /// Runs a query over <paramref name="components"/> and retrieve a result.
    /// </summary>
    /// <returns>True, if query succeeded, otherwise, false.</returns>
    public abstract bool Execute(Components components);
}

/// <summary>
/// Holds information about a single <see cref="Component"/> query.
/// </summary>
public class Query<C> : ComponentQuery where C : Component {
    private C _entry;

    public C Entry => _entry;

    /// <inheritdoc/>
    public override bool Execute(Components components) {
        _entry = components.Get<C>();
        return _entry != null;
    }

    public static implicit operator C(Query<C> query) {
        return query.Entry;
    }
}

/// <summary>
/// Holds information about a multiple <see cref="Component"/> query.
/// </summary>
public class MultiQuery<C> : ComponentQuery, IEnumerable<C>
    where C : Component
{
    private List<C> _entries = new();

    public List<C> Entries => _entries;

    /// <inheritdoc/>
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
