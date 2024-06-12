
namespace Midnight.GUI;

public class Design {
    public DesignBuilder Builder { get; } = new();
    public GUI.Object Root { get => Builder.Result; }

    public T Create<T>() where T : GUI.Object, new() {
        return new T();
    }
}
