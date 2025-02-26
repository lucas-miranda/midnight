namespace Midnight.ECS;

public abstract class SystemSubscribeContract {
    public System.Type EventType { get; protected set; }
    public System.Type ComponentType { get; protected set; }

    public abstract void Call(Event ev, Component component);
}

public class SystemSubscribeContract<E, C> : SystemSubscribeContract
    where E : Event
    where C : Component
{
    private System.Action<E, C> _fn;

    public SystemSubscribeContract(System.Action<E, C> fn) {
        EventType = typeof(E);
        ComponentType = typeof(C);
        _fn = fn;
    }

    public override void Call(Event ev, Component component) {
        Call(ev as E, component as C);
    }

    public void Call(E ev, C component) {
        _fn.Invoke(ev, component);
    }
}
