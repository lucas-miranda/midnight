namespace Midnight.ECS;

public class ComponentAddedEvent : ComponentEvent {
    public ComponentAddedEvent(Component context) : base(context) {
    }
}
