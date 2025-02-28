using System.Collections.ObjectModel;
using System.Collections.Generic;
using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public sealed class Keyboard {
    private Xna.Input.KeyboardState _xnaState, _xnaPrevState;
    private List<Key> _pressed = new(),
                      _prevPressed = new(),
                      _buffer = new();

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

        _pressed.Sort();

        // send events
        bool hasPressedKeysChanged = _pressed.Count != _prevPressed.Count;

        if (!hasPressedKeysChanged && _pressed.Count > 0) {
            // verify if pressed keys changed
            for (int i = 0; i < _pressed.Count; i++) {
                Key key = _pressed[i],
                    prevKey = _prevPressed[i];

                if (key != prevKey) {
                    hasPressedKeysChanged = true;
                    break;
                }
            }
        }

        if (hasPressedKeysChanged) {
            // just pressed
            _buffer.Clear();
            _buffer.AddRange(_pressed);

            foreach (Key prevPressedKey in _prevPressed) {
                _buffer.Remove(prevPressedKey);
            }

            if (!_buffer.IsEmpty()) {
                foreach (Key key in _buffer) {
                    Scene.Current.Systems.Send<KeyboardKeyEvent>(new(key, ButtonState.JustPressed));
                }
            }

            // down
            _buffer.Clear();

            foreach (Key prevPressedKey in _prevPressed) {
                foreach (Key pressedKey in _pressed) {
                    if (pressedKey == prevPressedKey) {
                        _buffer.Add(pressedKey);
                        break;
                    }
                }
            }

            if (!_buffer.IsEmpty()) {
                foreach (Key key in _buffer) {
                    Scene.Current.Systems.Send<KeyboardKeyEvent>(new(key, ButtonState.Down));
                }
            }

            // just released
            _buffer.Clear();
            _buffer.AddRange(_prevPressed);

            foreach (Key pressedKey in _pressed) {
                _buffer.Remove(pressedKey);
            }

            if (!_buffer.IsEmpty()) {
                foreach (Key key in _buffer) {
                    Scene.Current.Systems.Send<KeyboardKeyEvent>(new(key, ButtonState.JustReleased));
                }
            }
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
