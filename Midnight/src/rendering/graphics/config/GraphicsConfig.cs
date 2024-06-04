
namespace Midnight;

public readonly record struct GraphicsConfig() {
    public static readonly GraphicsConfig Default = new() {
            BackBuffer = new(),
            Display = new(),
            Profile = GraphicsProfile.Reach,
        };

    public BackBufferConfig BackBuffer { get; init; }
    public DisplayConfig Display { get; init; }
    public GraphicsProfile Profile { get; init; } = GraphicsProfile.Reach;
}
