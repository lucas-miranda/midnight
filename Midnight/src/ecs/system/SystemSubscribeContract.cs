using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.ECS;

public abstract class SystemSubscribeContract {
    public System.Type EventType { get; protected set; }
    public System.Type[] ComponentTypes { get; protected set; }
    public bool MatchOriginatorOnly { get; set; }

    public abstract void Call(Event ev, Component component);
    public abstract void Call(Event ev, Component component1, Component component2);
    public abstract void Call(Event ev, Component component1, Component component2, Component component3);
    public abstract void Send(Event ev, Scene scene, Entity? entity = null);
}

public class SystemSubscribeContract<E, C> : SystemSubscribeContract
    where E : Event
    where C : Component
{
    private System.Action<E, C> _fn;

    public SystemSubscribeContract(System.Action<E, C> fn) {
        EventType = typeof(E);
        ComponentTypes = new[] { typeof(C) };
        _fn = fn;
    }

    public override void Call(Event ev, Component component) {
        Call(ev as E, component as C);
    }

    public override void Call(Event ev, Component component1, Component component2) {
        throw new System.NotSupportedException();
    }

    public override void Call(Event ev, Component component1, Component component2, Component component3) {
        throw new System.NotSupportedException();
    }

    public void Call(E ev, C component) {
        _fn.Invoke(ev, component);
    }

    public override void Send(Event ev, Scene scene, Entity? entity = null) {
        Assert.Is<E>(ev);
        Send((E) ev, scene, entity);
    }

    public void Send(E ev, Scene scene, Entity? entity = null) {
        IList<C> components;

        if (entity.HasValue) {
            components = scene.Components.QueryAll<C>(entity.Value);
        } else if (MatchOriginatorOnly && ev is IEventOriginator<Entity> eventOriginator) {
            components = scene.Components.QueryAll<C>(eventOriginator.Originator);
        } else {
            components = scene.Components.QueryAll<C>();
        }

        //Logger.DebugLine($" {components.Count} Component(s)");
        foreach (C component in components) {
            Call(ev, component);
        }
    }
}

public class SystemSubscribeContract<E, C1, C2> : SystemSubscribeContract
    where E : Event
    where C1 : Component
    where C2 : Component
{
    private System.Action<E, C1, C2> _fn;

    public SystemSubscribeContract(System.Action<E, C1, C2> fn) {
        EventType = typeof(E);
        ComponentTypes = new[] {
            typeof(C1),
            typeof(C2),
        };

        _fn = fn;
    }

    public override void Call(Event ev, Component component) {
        throw new System.NotSupportedException();
    }

    public override void Call(Event ev, Component component1, Component component2) {
        Call(ev as E, component1 as C1, component2 as C2);
    }

    public override void Call(Event ev, Component component1, Component component2, Component component3) {
        throw new System.NotSupportedException();
    }

    public void Call(E ev, C1 component1, C2 component2) {
        _fn.Invoke(ev, component1, component2);
    }

    public override void Send(Event ev, Scene scene, Entity? entity = null) {
        Assert.Is<E>(ev);
        Send((E) ev, scene, entity);
    }

    public void Send(E ev, Scene scene, Entity? entity = null) {
        if (entity.HasValue) {
            (C1, C2) component = scene.Components.Query<C1, C2>(entity.Value);
            Call(ev, component.Item1, component.Item2);
        } else if (MatchOriginatorOnly && ev is IEventOriginator<Entity> eventOriginator) {
            (C1, C2) component = scene.Components.Query<C1, C2>(eventOriginator.Originator);
            Call(ev, component.Item1, component.Item2);
        } else {
            IList<(C1, C2)> components = scene.Components.QueryAll<C1, C2>();

            //Logger.DebugLine($" {components.Count} Component(s)");
            foreach ((C1, C2) component in components) {
                Call(ev, component.Item1, component.Item2);
            }
        }
    }
}

public class SystemSubscribeContract<E, C1, C2, C3> : SystemSubscribeContract
    where E : Event
    where C1 : Component
    where C2 : Component
    where C3 : Component
{
    private System.Action<E, C1, C2, C3> _fn;

    public SystemSubscribeContract(System.Action<E, C1, C2, C3> fn) {
        EventType = typeof(E);
        ComponentTypes = new[] {
            typeof(C1),
            typeof(C2),
            typeof(C3),
        };

        _fn = fn;
    }

    public override void Call(Event ev, Component component) {
        throw new System.NotSupportedException();
    }

    public override void Call(Event ev, Component component1, Component component2) {
        throw new System.NotSupportedException();
    }

    public override void Call(Event ev, Component component1, Component component2, Component component3) {
        Call(ev as E, component1 as C1, component2 as C2, component3 as C3);
    }

    public void Call(E ev, C1 component1, C2 component2, C3 component3) {
        _fn.Invoke(ev, component1, component2, component3);
    }

    public override void Send(Event ev, Scene scene, Entity? entity = null) {
        Assert.Is<E>(ev);
        Send((E) ev, scene, entity);
    }

    public void Send(E ev, Scene scene, Entity? entity = null) {
        if (entity.HasValue) {
            (C1, C2, C3) component = scene.Components.Query<C1, C2, C3>(entity.Value);
            Call(ev, component.Item1, component.Item2, component.Item3);
        } else if (MatchOriginatorOnly && ev is IEventOriginator<Entity> eventOriginator) {
            (C1, C2, C3) component = scene.Components.Query<C1, C2, C3>(eventOriginator.Originator);
            Call(ev, component.Item1, component.Item2, component.Item3);
        } else {
            IList<(C1, C2, C3)> components = scene.Components.QueryAll<C1, C2, C3>();

            //Logger.DebugLine($" {components.Count} Component(s)");
            foreach ((C1, C2, C3) component in components) {
                Call(ev, component.Item1, component.Item2, component.Item3);
            }
        }
    }
}
