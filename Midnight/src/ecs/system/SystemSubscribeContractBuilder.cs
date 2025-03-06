namespace Midnight.ECS;

public abstract class SystemSubscribeContractBuilder {
    internal SystemSubscribeContractBuilder() {
    }

    internal EntitySystem System { get; set; }
}

public class SystemSubscribeContractBuilder<E> : SystemSubscribeContractBuilder
    where E : Event
{
    public SystemSubscribeContractBuilder() {
    }

    public SystemSubscribeContractBuilder<E, Query<C>> With<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, Query<C>>>();
    }

    public SystemSubscribeContractBuilder<E, MultiQuery<C>> WithMultiple<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, MultiQuery<C>>>();
    }
}

public class SystemSubscribeContractBuilder<E, Q> : SystemSubscribeContractBuilder
    where E : Event
    where Q : ComponentQuery, new()
{
    public SystemSubscribeContractBuilder() {
    }

    public SystemSubscribeContractBuilder<E, Q, Query<C>> With<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, Q, Query<C>>>();
    }

    public SystemSubscribeContractBuilder<E, Q, MultiQuery<C>> WithMultiple<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, Q, MultiQuery<C>>>();
    }

    public SystemSubscribeContract<E, Q> Submit(System.Action<E, Q> fn) {
        return System.RegisterContract<SystemSubscribeContract<E, Q>>(new(fn));
    }
}

public class SystemSubscribeContractBuilder<E, Q1, Q2> : SystemSubscribeContractBuilder
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
{
    public SystemSubscribeContractBuilder() {
    }

    public SystemSubscribeContractBuilder<E, Q1, Q2, Query<C>> With<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, Q1, Q2, Query<C>>>();
    }

    public SystemSubscribeContractBuilder<E, Q1, Q2, MultiQuery<C>> WithMultiple<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, Q1, Q2, MultiQuery<C>>>();
    }

    public SystemSubscribeContract<E, Q1, Q2> Submit(System.Action<E, Q1, Q2> fn) {
        return System.RegisterContract<SystemSubscribeContract<E, Q1, Q2>>(new(fn));
    }
}

public class SystemSubscribeContractBuilder<E, Q1, Q2, Q3> : SystemSubscribeContractBuilder
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
    where Q3 : ComponentQuery, new()
{
    public SystemSubscribeContractBuilder() {
    }

    public SystemSubscribeContractBuilder<E, Q1, Q2, Q3, Query<C>> With<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, Q1, Q2, Q3, Query<C>>>();
    }

    public SystemSubscribeContractBuilder<E, Q1, Q2, Q3, MultiQuery<C>> WithMultiple<C>() where C : Component {
        return System.GetContractBuilder<SystemSubscribeContractBuilder<E, Q1, Q2, Q3, MultiQuery<C>>>();
    }

    public SystemSubscribeContract<E, Q1, Q2, Q3> Submit(System.Action<E, Q1, Q2, Q3> fn) {
        return System.RegisterContract<SystemSubscribeContract<E, Q1, Q2, Q3>>(new(fn));
    }
}

public class SystemSubscribeContractBuilder<E, Q1, Q2, Q3, Q4> : SystemSubscribeContractBuilder
    where E : Event
    where Q1 : ComponentQuery, new()
    where Q2 : ComponentQuery, new()
    where Q3 : ComponentQuery, new()
    where Q4 : ComponentQuery, new()
{
    public SystemSubscribeContractBuilder() {
    }

    public SystemSubscribeContract<E, Q1, Q2, Q3, Q4> Submit(System.Action<E, Q1, Q2, Q3, Q4> fn) {
        return System.RegisterContract<SystemSubscribeContract<E, Q1, Q2, Q3, Q4>>(new(fn));
    }
}
