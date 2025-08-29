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
