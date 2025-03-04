using Midnight.Diagnostics;

namespace Midnight.ECS;

public abstract class ComponentEvent : Event, IEventOriginator<Entity> {
    public ComponentEvent(Component context) {
        Assert.NotNull(context);
        Context = context;
    }

    public Entity Originator => Context.Entity;
    public Component Context { get; }
}
