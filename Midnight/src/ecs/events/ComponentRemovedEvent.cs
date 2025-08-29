namespace Midnight.ECS;

public class ComponentRemovedEvent : ComponentEvent {
    public ComponentRemovedEvent(Component context) : base(context) {
    }
}
