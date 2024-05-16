namespace Midnight;

public abstract class GraphicDisplayer : Component, IRenderable {
    public abstract void Render(DeltaTime dt, RenderingServer r);
}
