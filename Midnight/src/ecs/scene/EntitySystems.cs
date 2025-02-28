using System.Collections.Generic;
using Midnight.Diagnostics;
using Midnight.ECS;

namespace Midnight;

public sealed class EntitySystems {
    private readonly System.Type EventBaseType = typeof(Event).BaseType;
    private List<EntitySystem> _systems = new();
    private Dictionary<System.Type, List<LookupRegistry>> _lookup = new();
    private List<List<LookupRegistry>> _buffer = new();

    public EntitySystems(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public void Send<E>(E ev) where E : Event {
        Assert.NotNull(ev);
        List<List<LookupRegistry>> lists = RetrieveLists(typeof(E));

        if (lists.IsEmpty()) {
            // there is no systems subscribed
            //Logger.DebugLine($"There is no systems subscribed to: {ev.GetType()}");
            return;
        }

        /*
        if (ev is not EngineEvent) {
            Logger.DebugLine($"Sending '{ev.GetType()}':");
            Logger.DebugLine($" {ev}");
        }
        */

        foreach (List<LookupRegistry> list in lists) {
            foreach (LookupRegistry registry in list) {
                IList<Component> components = Scene.Components.Query(registry.Contract.ComponentType);
                //Logger.DebugLine($" {components.Count} Component(s)");
                foreach (Component component in components) {
                    registry.Contract.Call(ev, component);
                }
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
            GetOrCreateLookupList(contract.EventType)
                .Add(new() {
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

    private List<List<LookupRegistry>> RetrieveLists(System.Type targetType) {
        _buffer.Clear();

        while (targetType != EventBaseType) {
            if (_lookup.TryGetValue(targetType, out List<LookupRegistry> list)) {
                _buffer.Add(list);
            }

            targetType = targetType.BaseType;
        }

        return _buffer;
    }

    private struct LookupRegistry {
        public SystemSubscribeContract Contract;
        public EntitySystem System;
    }
}
