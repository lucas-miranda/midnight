
namespace Midnight;

public class MouseScrollWheelEvent : MouseEvent {
    public MouseScrollWheelEvent(Vector2I position, int scroll) : base(position) {
        Scroll = scroll;
    }

    public int Scroll { get; }
}


