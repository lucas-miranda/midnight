using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.ECS;

public abstract class SystemSubscribeContract {
    public System.Type EventType { get; protected set; }
    //public System.Type[] ComponentTypes { get; protected set; }
    public bool MatchOriginatorOnly { get; set; }

    public abstract void Send(Event ev, Scene scene, Entity? entity = null);
}

public abstract class SystemSubscribeContract<E> : SystemSubscribeContract where E : Event {
    public sealed override void Send(Event ev, Scene scene, Entity? entity = null) {
        Assert.Is<E>(ev);
        Send((E) ev, scene, entity);
    }

    public void Send(E ev, Scene scene, Entity? entity = null) {
        if (entity.HasValue) {
            Execute(ev, scene, entity.Value);
        } else if (MatchOriginatorOnly && ev is IEventOriginator<Entity> eventOriginator) {
            Execute(ev, scene, eventOriginator.Originator);
        } else {
            foreach (KeyValuePair<Entity, Components> entry in scene.Components.EntityComponents()) {
                Execute(ev, entry.Value);
            }
        }
    }

    protected abstract void Execute(E ev, Components entityComponents);

    protected void Execute(E ev, Scene scene, Entity entity) {
        Execute(ev, scene.Components.Get(entity));
    }
}

public class SystemSubscribeContract<E, Q> : SystemSubscribeContract<E>
    where E : Event
    where Q : ComponentQuery, new()
{
    private System.Action<E, Q> _fn;

    public SystemSubscribeContract(System.Action<E, Q> fn) {
        EventType = typeof(E);
        //ComponentTypes = new[] { typeof(C) };
        _fn = fn;
    }

    public Q Query { get; } = new();

    protected override void Execute(E ev, Components entityComponents) {
        if (Query.Execute(entityComponents)) {
            _fn.Invoke(ev, Query);
        }
    }
}

public class SystemSubscribeContract<E, Q1, Q2> : SystemSubscribeContract<E>
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
{
    private System.Action<E, Q1, Q2> _fn;

    public SystemSubscribeContract(System.Action<E, Q1, Q2> fn) {
        EventType = typeof(E);
        /*
        ComponentTypes = new[] {
            typeof(C1),
            typeof(C2),
        };
        */

        _fn = fn;
    }

    public Q1 QueryA { get; } = new();
    public Q2 QueryB { get; } = new();

    protected override void Execute(E ev, Components entityComponents) {
        if (QueryA.Execute(entityComponents)
         && QueryB.Execute(entityComponents)
        ) {
            _fn.Invoke(ev, QueryA, QueryB);
        }
    }
}

public class SystemSubscribeContract<E, Q1, Q2, Q3> : SystemSubscribeContract<E>
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
    where Q3 : ComponentQuery, new()
{
    private System.Action<E, Q1, Q2, Q3> _fn;

    public SystemSubscribeContract(System.Action<E, Q1, Q2, Q3> fn) {
        EventType = typeof(E);
        /*
        ComponentTypes = new[] {
            typeof(C1),
            typeof(C2),
            typeof(C3),
        };
        */

        _fn = fn;
    }

    public Q1 QueryA { get; } = new();
    public Q2 QueryB { get; } = new();
    public Q3 QueryC { get; } = new();

    protected override void Execute(E ev, Components entityComponents) {
        if (QueryA.Execute(entityComponents)
         && QueryB.Execute(entityComponents)
         && QueryC.Execute(entityComponents)
        ) {
            _fn.Invoke(ev, QueryA, QueryB, QueryC);
        }
    }
}

public class SystemSubscribeContract<E, Q1, Q2, Q3, Q4> : SystemSubscribeContract<E>
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
    where Q3 : ComponentQuery, new()
    where Q4 : ComponentQuery, new()
{
    private System.Action<E, Q1, Q2, Q3, Q4> _fn;

    public SystemSubscribeContract(System.Action<E, Q1, Q2, Q3, Q4> fn) {
        EventType = typeof(E);
        /*
        ComponentTypes = new[] {
            typeof(C1),
            typeof(C2),
            typeof(C3),
        };
        */

        _fn = fn;
    }

    public Q1 QueryA { get; } = new();
    public Q2 QueryB { get; } = new();
    public Q3 QueryC { get; } = new();
    public Q4 QueryD { get; } = new();

    protected override void Execute(E ev, Components entityComponents) {
        if (QueryA.Execute(entityComponents)
         && QueryB.Execute(entityComponents)
         && QueryC.Execute(entityComponents)
         && QueryD.Execute(entityComponents)
        ) {
            _fn.Invoke(ev, QueryA, QueryB, QueryC, QueryD);
        }
    }
}
