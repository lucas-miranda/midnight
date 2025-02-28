
namespace Midnight;

public class MouseButtonEvent : MouseEvent {
    public MouseButtonEvent(
        Vector2I position,
        MouseButton button,
        ButtonState state,
        ButtonState prevState
    ) : base(position)
    {
        Button = button;
        State = state;
        PreviousState = prevState;
    }

    public MouseButton Button { get; }
    public ButtonState State { get; }
    public ButtonState PreviousState { get; }

    public override string ToString() {
        return $"MouseButtonEvent  Pos: {Position}; Button: {Button}; State: {State}; Prev State: {PreviousState}";
    }
}
