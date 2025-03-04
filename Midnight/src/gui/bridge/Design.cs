
namespace Midnight.GUI;

public class Design {
    public DesignBuilder Builder { get; } = new();
    public Entity Root => Builder.Result;
}
