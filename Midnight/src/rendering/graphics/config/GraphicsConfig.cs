
namespace Midnight;

public readonly record struct GraphicsConfig() {
    public static readonly GraphicsConfig Default = new() {
            BackBuffer = new(),
            Display = new()
        };

    public BackBufferConfig BackBuffer { get; init; }
    public DisplayConfig Display { get; init; }
}
