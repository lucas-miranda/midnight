using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public sealed class Keyboard {
    private Xna.Input.KeyboardState _xnaState, _xnaPrevState;
    private List<Key> _pressed = new(),
                      _prevPressed = new();

    internal Keyboard() {
        Pressed = new(_pressed);
    }

    public ReadOnlyCollection<Key> Pressed { get; }

    public void Update(DeltaTime dt) {
        _xnaPrevState = _xnaState;
        _xnaState = Xna.Input.Keyboard.GetState();

        // rotate lists
        List<Key> p = _prevPressed;
        _prevPressed = _pressed;
        _pressed = p;

        // reset current state
        _pressed.Clear();

        // store pressed keys
        Xna.Input.Keys[] pressedKeys = _xnaState.GetPressedKeys();

        foreach (Xna.Input.Keys key in pressedKeys) {
            _pressed.Add((Key) key);
        }
    }

    public bool IsAnyPressed() {
        return !_pressed.IsEmpty();
    }

    public bool IsJustPressed(Key key) {
        return IsDown(key) && WasUp(key);
    }

    public bool IsJustReleased(Key key) {
        return IsUp(key) && WasDown(key);
    }

    public bool IsDown(Key key) {
        return _xnaState.IsKeyDown(key.ToXna());
    }

    public bool IsUp(Key key) {
        return _xnaState.IsKeyUp(key.ToXna());
    }

    public bool WasDown(Key key) {
        return _xnaPrevState.IsKeyDown(key.ToXna());
    }

    public bool WasUp(Key key) {
        return _xnaPrevState.IsKeyUp(key.ToXna());
    }
}
