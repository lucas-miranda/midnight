
namespace Midnight;

public class MouseButtonEvent : MouseMoveEvent {
    public MouseButtonEvent(
        Vector2I position,
        Vector2I movement,
        MouseButton button,
        ButtonState state,
        ButtonState prevState
    ) : base(position, movement)
    {
        Button = button;
        State = state;
        PreviousState = prevState;
    }

    public MouseButton Button { get; }
    public ButtonState State { get; }
    public ButtonState PreviousState { get; }

    public override string ToString() {
        if (Position == EndPosition) {
            return $"MouseButtonEvent  Pos: {Position}; Button: {Button}; State: {State}; Prev State: {PreviousState}";
        }

        return $"MouseButtonEvent  Pos: {Position} -> {EndPosition} (Move: {Movement}); Button: {Button}; State: {State}; Prev State: {PreviousState}";
    }
}
