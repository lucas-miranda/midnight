using System.Collections.Generic;
using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public sealed class RenderTarget {
    private Stack<Canvas> _targets = new();
    private Canvas _current;

    internal RenderTarget(XnaGraphics.GraphicsDevice xnaDevice) {
        XnaGraphicsDevice = xnaDevice;
    }

    public Canvas Current => _current;

    internal XnaGraphics.GraphicsDevice XnaGraphicsDevice { get; }

    public void Push(Canvas canvas) {
        Assert.NotNull(canvas);
        _targets.Push(canvas);
        ChangeRenderTarget(canvas);
    }

    public Canvas Pop() {
        Canvas c = _targets.Pop();

        if (!_targets.TryPeek(out _current)) {
            _current = null;
        }

        ChangeRenderTarget(_current);
        return c;
    }

    public Canvas Peek() {
        return _targets.Peek();
    }

    public void Clear() {
        _targets.Clear();
        ChangeRenderTarget(null);
    }

    private void ChangeRenderTarget(Canvas c) {
        _current = c;
        XnaGraphicsDevice.SetRenderTarget(_current?.Underlying);
    }
}
