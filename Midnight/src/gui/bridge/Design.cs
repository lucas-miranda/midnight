namespace Midnight.GUI;

/// <summary>
/// UI design data.
/// </summary>
public class Design {
    public DesignBuilder Builder { get; } = new();
    public Entity Root => Builder.Result;
}
