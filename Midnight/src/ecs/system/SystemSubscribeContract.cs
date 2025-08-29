using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight.ECS;

/// <summary>
/// Event subscription contract, holding every information needed to dispatch it.
/// </summary>
public abstract class SystemSubscribeContract {
    protected SystemSubscribeContract() {
    }

    /// <summary>
    /// To which event type this contract is about.
    /// </summary>
    public System.Type EventType { get; protected set; }

    /// <summary>
    /// Only handle event for the entity which triggered it.
    /// </summary>
    /// <remarks>
    /// If `MatchOriginatorOnly` is true, event is expected to implement <see cref="IEventOriginator{T}"/>.
    /// </remarks>
    public bool IsHandlingOnce { get; set; }

    /// <summary>
    /// Handle provided event.
    /// </summary>
    /// <param name="ev">Event to handle, expecting it to have type <see cref="EventType"/>.</param>
    /// <param name="scene">At which scene it happens.</param>
    /// <param name="entity">Optionally, it can be forced to handle event only for a single entity.</param>
    public abstract void Handle(Event ev, Scene scene, Entity? entity = null);

    public SystemSubscribeContract HandleOnce(bool value = true) {
        IsHandlingOnce = value;
        return this;
    }

    public SystemSubscribeContract HandleAll(bool value = true) {
        IsHandlingOnce = !value;
        return this;
    }
}

/// <summary>
/// Event subscription contract, holding every information needed to dispatch it.
/// </summary>
/// <typeparam name="E">Event type which contract is expecting.</typeparam>
public class SystemSubscribeContract<E> : SystemSubscribeContract where E : Event {
    /// <summary>Method which will handle event.</summary>
    private System.Action<E> _fn;

    protected SystemSubscribeContract() {
    }

    /// <summary>
    /// </summary>
    /// <param name="fn">Method to handle event <typeparamref name="E">E</typeparamref>.</param>
    public SystemSubscribeContract(System.Action<E> fn) {
        EventType = typeof(E);
        _fn = fn;
    }

    /// <inheritdoc/>
    public sealed override void Handle(Event ev, Scene scene, Entity? entity = null) {
        Assert.Is<E>(ev);
        Handle((E) ev, scene, entity);
    }

    /// <summary>
    /// Handle provided event <typeparamref name="E">E</typeparamref>.
    /// It could handle event for one or more entities.
    /// </summary>
    public void Handle(E ev, Scene scene, Entity? entity = null) {
        if (entity.HasValue) {
            // handle event only for an entity
            Execute(ev, entity.Value);
        } else if (IsHandlingOnce) {
            if (ev is IEventOriginator<Entity> eventOriginator) {
                // handle event only for the entity which triggered the event, it's originator.
                // if MatchOriginatorOnly is used, event is expected to implement IEventOriginator
                //Assert.Is<IEventOriginator<Entity>>(ev, $"Expected to have event originator, but event '{ev.GetType()}' doesn't have it implemented.");
                Execute(ev, eventOriginator.Originator);
            } else {
                // handle event without an entity
                Execute(ev, null);
            }
        } else if (scene != null) {
            // handle event for every entity in scene
            foreach (KeyValuePair<Entity, Components> entry in scene.Components.EntityComponents()) {
                Execute(ev, entry.Value);
            }
        }
    }

    /// <summary>
    /// Calls registered method to handle an event with an entity's components.
    /// </summary>
    protected virtual void Execute(E ev, Components entityComponents) {
        _fn.Invoke(ev);
    }

    /// <summary>
    /// Calls registered method to handle an event with an entity.
    /// </summary>
    protected void Execute(E ev, Entity entity) {
        Execute(ev, entity.Components);
    }
}

/// <summary>
/// Event subscription contract, holding every information needed to dispatch it.
/// </summary>
/// <typeparam name="E">Event type which contract is expecting.</typeparam>
/// <typeparam name="Q">A <see cref="ComponentQuery"/> to filter components.</typeparam>
public class SystemSubscribeContract<E, Q> : SystemSubscribeContract<E>
    where E : Event
    where Q : ComponentQuery, new()
{
    /// <summary>Method which will handle event.</summary>
    private System.Action<E, Q> _fn;

    /// <param name="fn">Method to handle event <typeparamref name="E">E</typeparamref> with <see cref="ComponentQuery"/> <typeparamref name="Q">Q</typeparamref>.</param>
    public SystemSubscribeContract(System.Action<E, Q> fn) {
        EventType = typeof(E);
        _fn = fn;
    }

    public Q Query { get; } = new();

    /// <inheritdoc/>
    protected override void Execute(E ev, Components entityComponents) {
        if (Query.Execute(entityComponents)) {
            _fn.Invoke(ev, Query);
        }
    }
}

/// <summary>
/// Event subscription contract, holding every information needed to dispatch it.
/// </summary>
/// <typeparam name="E">Event type which contract is expecting.</typeparam>
/// <typeparam name="Q1">First <see cref="ComponentQuery"/> to filter components.</typeparam>
/// <typeparam name="Q2">Second <see cref="ComponentQuery"/> to filter components.</typeparam>
public class SystemSubscribeContract<E, Q1, Q2> : SystemSubscribeContract<E>
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
{
    /// <summary>Method which will handle event.</summary>
    private System.Action<E, Q1, Q2> _fn;

    /// <param name="fn">Method to handle event <typeparamref name="E">E</typeparamref> with <see cref="ComponentQuery"/> <typeparamref name="Q1">Q1</typeparamref> and <typeparamref name="Q2">Q2</typeparamref>.</param>
    public SystemSubscribeContract(System.Action<E, Q1, Q2> fn) {
        EventType = typeof(E);
        _fn = fn;
    }

    public Q1 QueryA { get; } = new();
    public Q2 QueryB { get; } = new();

    /// <inheritdoc/>
    protected override void Execute(E ev, Components entityComponents) {
        if (QueryA.Execute(entityComponents)
         && QueryB.Execute(entityComponents)
        ) {
            _fn.Invoke(ev, QueryA, QueryB);
        }
    }
}

/// <summary>
/// Event subscription contract, holding every information needed to dispatch it.
/// </summary>
/// <typeparam name="E">Event type which contract is expecting.</typeparam>
/// <typeparam name="Q1">First <see cref="ComponentQuery"/> to filter components.</typeparam>
/// <typeparam name="Q2">Second <see cref="ComponentQuery"/> to filter components.</typeparam>
/// <typeparam name="Q3">Third <see cref="ComponentQuery"/> to filter components.</typeparam>
public class SystemSubscribeContract<E, Q1, Q2, Q3> : SystemSubscribeContract<E>
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
    where Q3 : ComponentQuery, new()
{
    /// <summary>Method which will handle event.</summary>
    private System.Action<E, Q1, Q2, Q3> _fn;

    /// <param name="fn">Method to handle event <typeparamref name="E">E</typeparamref> with <see cref="ComponentQuery"/> <typeparamref name="Q1">Q1</typeparamref>, <typeparamref name="Q2">Q2</typeparamref> and <typeparamref name="Q3">Q3</typeparamref>.</param>
    public SystemSubscribeContract(System.Action<E, Q1, Q2, Q3> fn) {
        EventType = typeof(E);
        _fn = fn;
    }

    public Q1 QueryA { get; } = new();
    public Q2 QueryB { get; } = new();
    public Q3 QueryC { get; } = new();

    /// <inheritdoc/>
    protected override void Execute(E ev, Components entityComponents) {
        if (QueryA.Execute(entityComponents)
         && QueryB.Execute(entityComponents)
         && QueryC.Execute(entityComponents)
        ) {
            _fn.Invoke(ev, QueryA, QueryB, QueryC);
        }
    }
}

/// <summary>
/// Event subscription contract, holding every information needed to dispatch it.
/// </summary>
/// <typeparam name="E">Event type which contract is expecting.</typeparam>
/// <typeparam name="Q1">First <see cref="ComponentQuery"/> to filter components.</typeparam>
/// <typeparam name="Q2">Second <see cref="ComponentQuery"/> to filter components.</typeparam>
/// <typeparam name="Q3">Third <see cref="ComponentQuery"/> to filter components.</typeparam>
/// <typeparam name="Q4">Fourth <see cref="ComponentQuery"/> to filter components.</typeparam>
public class SystemSubscribeContract<E, Q1, Q2, Q3, Q4> : SystemSubscribeContract<E>
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
    where Q3 : ComponentQuery, new()
    where Q4 : ComponentQuery, new()
{
    /// <summary>Method which will handle event.</summary>
    private System.Action<E, Q1, Q2, Q3, Q4> _fn;

    /// <param name="fn">Method to handle event <typeparamref name="E">E</typeparamref> with <see cref="ComponentQuery"/> <typeparamref name="Q1">Q1</typeparamref>, <typeparamref name="Q2">Q2</typeparamref>, <typeparamref name="Q3">Q3</typeparamref> and <typeparamref name="Q4">Q4</typeparamref>.</param>
    public SystemSubscribeContract(System.Action<E, Q1, Q2, Q3, Q4> fn) {
        EventType = typeof(E);
        _fn = fn;
    }

    public Q1 QueryA { get; } = new();
    public Q2 QueryB { get; } = new();
    public Q3 QueryC { get; } = new();
    public Q4 QueryD { get; } = new();

    /// <inheritdoc/>
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
