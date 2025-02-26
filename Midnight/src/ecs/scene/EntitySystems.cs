using System.Collections.Generic;
using Midnight.Diagnostics;
using Midnight.ECS;

namespace Midnight;

public sealed class EntitySystems {
    private List<EntitySystem> _systems = new();
    private Dictionary<System.Type, List<LookupRegistry>> _lookup = new();

    public EntitySystems(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public void Send<E>(E ev) where E : Event {
        if (!_lookup.TryGetValue(typeof(E), out List<LookupRegistry> list)) {
            // there is no systems subscribed
            Logger.DebugLine($"There is no systems subscribed to: {ev.GetType()}");
            return;
        }

        Assert.NotNull(ev);

        Logger.DebugLine($"Sending '{ev.GetType()}':");
        foreach (LookupRegistry registry in list) {
            IList<Component> components = Scene.Components.Query(registry.Contract.ComponentType);
            Logger.DebugLine($" {components.Count} Component(s)");
            foreach (Component component in components) {
                registry.Contract.Call(ev, component);
            }
        }
    }

    public void Send<E>() where E : Event, new() {
        Send(new E());
    }

    public void Register(EntitySystem sys) {
        Assert.NotNull(sys);
        _systems.Add(sys);
        sys.Setup();

        foreach (SystemSubscribeContract contract in sys.Contracts) {
            List<LookupRegistry> list = GetOrCreateLookupList(contract.EventType);
            list.Add(new() {
                Contract = contract,
                System = sys,
            });
        }
    }

    private List<LookupRegistry> GetOrCreateLookupList(System.Type eventType) {
        if (!_lookup.TryGetValue(eventType, out List<LookupRegistry> list)) {
            list = new();
            _lookup[eventType] = list;
        }

        return list;
    }

    private struct LookupRegistry {
        public SystemSubscribeContract Contract;
        public EntitySystem System;
    }
}
