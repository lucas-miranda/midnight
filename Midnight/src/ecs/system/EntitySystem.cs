using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midnight.ECS;

namespace Midnight;

public abstract class EntitySystem {
    private List<SystemSubscribeContract> _contracts = new();

    public EntitySystem() {
        Contracts = _contracts.AsReadOnly();
    }

    internal ReadOnlyCollection<SystemSubscribeContract> Contracts { get; }

    public abstract void Setup();

    protected SystemSubscribeContract<E, C> Subscribe<E, C>(System.Action<E, C> fn)
        where E : Event
        where C : Component
    {
        SystemSubscribeContract<E, C> contract = new(fn);
        _contracts.Add(contract);
        return contract;
    }
}
