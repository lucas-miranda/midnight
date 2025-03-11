using System.Collections.Generic;
using Midnight.Diagnostics;
using Midnight.ECS;

namespace Midnight;

public sealed class EntitySystems {
    private readonly System.Type EventBaseType = typeof(Event).BaseType;
    private List<EntitySystem> _systems = new();
    private Dictionary<System.Type, List<LookupRegistry>> _lookup = new();
    private List<List<LookupRegistry>> _buffer = new();
    private Queue<(Event, Entity?)> _eventQueue = new();
    private bool _isHandlingEvent, _isHandlingEventQueue;

    public EntitySystems(Scene scene) {
        Scene = scene;
    }

    public Scene Scene { get; }

    public void Send(Event ev, Entity? entity = null) {
        Assert.NotNull(ev);

        if (_isHandlingEvent) {
            _eventQueue.Enqueue((ev, entity));
            return;
        }

        List<List<LookupRegistry>> lists = RetrieveLists(ev.GetType());

        if (lists.IsEmpty()) {
            // there is no systems subscribed
            //Logger.DebugLine($"There is no systems subscribed to: {ev.GetType()}");
            return;
        }

        _isHandlingEvent = true;

        /*
        if (ev is not EngineEvent) {
            Logger.DebugLine($"Sending '{ev.GetType()}':");
            Logger.DebugLine($" {ev}");
            Logger.DebugLine($" It'll be handled by {lists.Count} lists");

            foreach (List<LookupRegistry> list in lists) {
                Logger.DebugLine($" > {list.Count} registries");
            }
        }
        */

        foreach (List<LookupRegistry> list in lists) {
            foreach (LookupRegistry systemRegistry in list) {
                systemRegistry.Contract.Send(ev, Scene, entity);
            }
        }

        /*
        if (ev is not EngineEvent) {
            Logger.DebugLine($"Complete Sending '{ev.GetType()}':");
            Logger.DebugLine($" {ev}");
        }
        */

        _isHandlingEvent = false;
        HandleEventQueue();
    }

    public void Send<E>(E ev, Entity? entity = null) where E : Event {
        Send((Event) ev, entity);
    }

    public void Send<E>(Entity? entity = null) where E : Event, new() {
        Send((Event) new E(), entity);
    }

    public void Register(EntitySystem sys) {
        Assert.NotNull(sys);
        _systems.Add(sys);
        sys.Setup(Scene);

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

    private void HandleEventQueue() {
        if (_isHandlingEventQueue) {
            return;
        }

        _isHandlingEventQueue = true;
        while (!_eventQueue.IsEmpty()) {
            (Event ev, Entity? entity) = _eventQueue.Dequeue();
            Send(ev, entity);
        }
        _isHandlingEventQueue = false;
    }

    private struct LookupRegistry {
        public SystemSubscribeContract Contract;
        public EntitySystem System;
    }
}
