namespace Midnight;

public abstract class GraphicDisplayer : Component, IUpdatable, IRenderable {
    /// <summary>
    /// Draw settings which will be used at this displayer.
    /// </summary>
    public DrawSettings DrawSettings { get; set; } = DrawSettings.Default;

    public abstract void Update(DeltaTime dt);

    public abstract void Render(DeltaTime dt, RenderingServer r);
}
