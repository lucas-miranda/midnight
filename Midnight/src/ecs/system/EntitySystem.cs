using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midnight.ECS;

namespace Midnight;

public abstract class EntitySystem {
    private List<SystemSubscribeContract> _contracts = new();
    private Dictionary<System.Type, SystemSubscribeContractBuilder> _builders = new();

    public EntitySystem() {
        Contracts = _contracts.AsReadOnly();
    }

    internal ReadOnlyCollection<SystemSubscribeContract> Contracts { get; }

    public abstract void Setup(Scene scene);

    protected void Emit<E>(E ev) where E : Event {
        Scene.Current.Systems.Send(ev);
    }

    protected void Emit<E>(E ev, Entity entity) where E : Event {
        Scene.Current.Systems.Send(ev, entity);
    }

    protected SystemSubscribeContractBuilder<E> Subscribe<E>() where E : Event {
        return GetContractBuilder<SystemSubscribeContractBuilder<E>>();
    }

    /*
    protected SystemSubscribeContract<E, C> Subscribe<E, C>(System.Action<E, C> fn, bool matchOriginatorOnly = false)
        where E : Event
        where C : Component
    {
        Logger.DebugLine($"Subscribe to event '{typeof(E)}', expecting component '{typeof(C)}'");
        SystemSubscribeContract<E, C> contract = new(fn) {
            MatchOriginatorOnly = matchOriginatorOnly,
        };

        _contracts.Add(contract);
        return contract;
    }

    protected SystemSubscribeContract<E, C1, C2> Subscribe<E, C1, C2>(System.Action<E, C1, C2> fn)
        where E : Event
        where C1 : Component
        where C2 : Component
    {
        Logger.DebugLine($"Subscribe to event '{typeof(E)}', expecting component '{typeof(C1)}' and '{typeof(C2)}'");
        SystemSubscribeContract<E, C1, C2> contract = new(fn);
        _contracts.Add(contract);
        return contract;
    }

    protected SystemSubscribeContract<E, C1, C2, C3> Subscribe<E, C1, C2, C3>(System.Action<E, C1, C2, C3> fn)
        where E : Event
        where C1 : Component
        where C2 : Component
        where C3 : Component
    {
        Logger.DebugLine($"Subscribe to event '{typeof(E)}', expecting component '{typeof(C1)}', '{typeof(C2)}' and '{typeof(C3)}'");
        SystemSubscribeContract<E, C1, C2, C3> contract = new(fn);
        _contracts.Add(contract);
        return contract;
    }
    */

    protected C Query<C>(Entity entity) where C : Component {
        return Scene.Current.Components.Query<C>(entity);
    }

    protected (C1, C2) Query<C1, C2>(Entity entity)
        where C1 : Component
        where C2 : Component
    {
        return Scene.Current.Components.Query<C1, C2>(entity);
    }

    internal B GetContractBuilder<B>() where B : SystemSubscribeContractBuilder, new() {
        if (!_builders.TryGetValue(typeof(B), out var builder)) {
            // create builder if it doesn't exists
            builder = new B();
            builder.System = this;
            _builders[typeof(B)] = builder;
        }

        return (B) builder;
    }

    internal T RegisterContract<T>(T contract) where T : SystemSubscribeContract {
        _contracts.Add(contract);
        return contract;
    }
}
