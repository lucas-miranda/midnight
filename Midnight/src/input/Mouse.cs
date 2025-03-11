using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public sealed class Mouse {
    private ButtonState[] _buttons, _prevButtons;
    private Xna.Input.ButtonState[] _clicked;
    private Vector2I _position;

    internal Mouse() {
        Xna.Input.Mouse.ClickedEXT += XnaMouseClicked;

        // initialize buttons state
        MouseButton[] mouseButtons = System.Enum.GetValues<MouseButton>();
        _buttons = new ButtonState[mouseButtons.Length];
        _prevButtons = new ButtonState[mouseButtons.Length];
        _clicked = new Xna.Input.ButtonState[mouseButtons.Length];
    }

    public Vector2I PreviousPosition { get; private set; }

    public Vector2I Position {
        get => _position;
        set {
            _position = value;
            Xna.Input.Mouse.SetPosition(value.X, value.Y);
        }
    }
    public int PreviousTotalScrollWheel { get; private set; }
    public int TotalScrollWheel { get; private set; }
    public int ScrollWheel { get; private set; }

    public bool LockOnCenter {
        get => Xna.Input.Mouse.IsRelativeMouseModeEXT;
        set => Xna.Input.Mouse.IsRelativeMouseModeEXT = value;
    }

    public void Update(DeltaTime dt) {
        // rotate arrays
        ButtonState[] b = _prevButtons;
        _prevButtons = _buttons;
        _buttons = b;

        // reset current state
        for (int i = 0; i < _buttons.Length; i++) {
            _buttons[i] = ButtonState.Up;
        }

        // update state
        Xna.Input.MouseState xnaState = Xna.Input.Mouse.GetState();

        // position
        PreviousPosition = Position;
        _position = new(xnaState.X, xnaState.Y);

        // scroll wheel
        PreviousTotalScrollWheel = TotalScrollWheel;
        TotalScrollWheel = xnaState.ScrollWheelValue;
        ScrollWheel = TotalScrollWheel - PreviousTotalScrollWheel;

        // buttons
        UpdateState(MouseButton.Left, xnaState.LeftButton);
        UpdateState(MouseButton.Right, xnaState.RightButton);
        UpdateState(MouseButton.Middle, xnaState.MiddleButton);
        UpdateState(MouseButton.Button4, xnaState.XButton1);
        UpdateState(MouseButton.Button5, xnaState.XButton2);

        // reset clicked
        for (int i = 0; i < _clicked.Length; i++) {
            _clicked[i] = Xna.Input.ButtonState.Released;
        }

        // send events

        // position
        if (Position != PreviousPosition) {
            Scene.Current.Systems.Send<MouseMoveEvent>(new(PreviousPosition, _position - PreviousPosition));
        }

        // scroll wheel
        if (TotalScrollWheel != PreviousTotalScrollWheel) {
            Scene.Current.Systems.Send<MouseScrollWheelEvent>(new(Position, ScrollWheel));
        }

        // buttons
        for (int i = (int) MouseButton.Left; i < _buttons.Length; i++) {
            ButtonState state = _buttons[i],
                        prevState = _prevButtons[i];

            if (state != prevState || state == ButtonState.Down) {
                Scene.Current.Systems.Send<MouseButtonEvent>(new(
                    PreviousPosition,
                    _position - PreviousPosition,
                    (MouseButton) i,
                    state,
                    prevState
                ));
            }
        }
    }

    public ButtonState GetButton(MouseButton button) {
        return _buttons[(int) button];
    }

    public bool IsJustPressed(MouseButton button) {
        return _buttons[(int) button] == ButtonState.JustPressed;
    }

    public bool IsJustReleased(MouseButton button) {
        return _buttons[(int) button] == ButtonState.JustReleased;
    }

    public bool IsDown(MouseButton button) {
        return _buttons[(int) button] == ButtonState.Down;
    }

    public bool IsUp(MouseButton button) {
        return _buttons[(int) button] == ButtonState.Up;
    }

    public bool WasDown(MouseButton button) {
        return _prevButtons[(int) button] == ButtonState.Down;
    }

    public bool WasUp(MouseButton button) {
        return _prevButtons[(int) button] == ButtonState.Up;
    }

    private void UpdateState(MouseButton button, Xna.Input.ButtonState buttonState) {
        buttonState |= _clicked[(int) button];
        ButtonState state,
                    prevState = _prevButtons[(int) button];

        if (prevState == ButtonState.Up || prevState == ButtonState.JustReleased) {
            // Xna.Released (0): Up (0)
            // Xna.Pressed (1): JustPressed (1)
            state = (ButtonState) (int) buttonState;
        } else {
            // Xna.Released (0): JustReleased (3) == JustReleased (3) - Xna.Released (0)
            // Xna.Pressed (1): Down (2) == JustReleased (3) - Xna.Pressed (1)
            state = ButtonState.JustReleased - (int) buttonState;
        }

        _buttons[(int) button] = state;
    }

    private void XnaMouseClicked(int button) {
        _clicked[button] = Xna.Input.ButtonState.Pressed;
    }
}
