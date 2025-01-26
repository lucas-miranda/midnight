
namespace Midnight.Diagnostics;

public class DebugIndicator {
    private Entity _entity;
    private Transform2D _transform;

    public DebugIndicator() {
        _entity = new();
        _transform = _entity.Components.Create<Transform2D>();
    }

    public void GraphicsReady() {
    }
}
