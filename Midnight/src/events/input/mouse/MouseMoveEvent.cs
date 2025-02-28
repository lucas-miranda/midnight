
namespace Midnight;

public class MouseMoveEvent : MouseEvent {
    public MouseMoveEvent(Vector2I position, Vector2I movement) : base(position) {
        Movement = movement;
    }

    public Vector2I Movement { get; }
    public Vector2I EndPosition => Position + Movement;
}


