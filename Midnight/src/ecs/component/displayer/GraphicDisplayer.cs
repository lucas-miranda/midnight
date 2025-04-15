namespace Midnight;

public abstract class GraphicDisplayer : Component, ISizeable {
    /// <summary>
    /// Draw settings which will be used at this displayer.
    /// </summary>
    public DrawSettings DrawSettings { get; set; } = DrawSettings.Default;
    public abstract Size2 Size { get; }
    public bool Visible { get; set; } = true;

    public abstract void Draw(DeltaTime dt, DrawParams drawParams);
}
