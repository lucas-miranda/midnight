using System.Collections.Generic;
using Midnight.Diagnostics;
using Midnight.ECS;

namespace Midnight;

public sealed class EntitySystems {
    /// <summary>
    /// Type marking where we should stop looking hierarchically to events.
    /// </summary>
    private readonly System.Type EventBaseType = typeof(Event).BaseType;

    /// <summary>
    /// Holds EntitySystem by it's type, so we can quickly find it's instance.
    /// <summary>
    private Dictionary<System.Type, EntitySystem> _systems = new();

    /// <summary>
    /// Associate an event with a list of entity system's registry.
    /// So we can quickly get who are subscribed to any event.
    /// </summary>
    private Dictionary<System.Type, List<LookupRegistry>> _lookup = new();

    /// <summary>
    /// It stores every list related to an event.
    /// It's used on Send.
    /// </summary>
    private List<List<LookupRegistry>> _buffer = new();

    /// <summary>
    /// Queue of events waiting to be sended.
    /// </summary>
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
                systemRegistry.Contract.Handle(ev, Scene, entity);
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

    public S Get<S>() where S : EntitySystem {
        return _systems[typeof(S)] as S;
    }

    public void Register(EntitySystem sys) {
        Assert.NotNull(sys);
        _systems.Add(sys.GetType(), sys);
        sys.Setup(Scene);

        // register every contract into a suitable lookup list (by event)
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

    /// <summary>
    /// Entity system and it's contract for quickly lookup.
    /// </summary>
    private struct LookupRegistry {
        public SystemSubscribeContract Contract;
        public EntitySystem System;
    }
}
