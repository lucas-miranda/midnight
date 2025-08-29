
namespace Midnight;

public abstract class MouseEvent : InputEvent {
    public MouseEvent(Vector2I position) {
        Position = position;
    }

    public Vector2I Position { get; }

    public override string ToString() {
        return $"MouseEvent  Pos: {Position};";
    }
}


