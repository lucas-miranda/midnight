
namespace Midnight;

public class KeyboardKeyEvent : InputEvent {
    public KeyboardKeyEvent(Key key, ButtonState state) {
        Key = key;
        State = state;
    }

    public Key Key { get; }
    public ButtonState State { get; }
}
