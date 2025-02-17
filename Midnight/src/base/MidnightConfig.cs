
namespace Midnight;

public readonly record struct MidnightConfig() {
    public static readonly MidnightConfig Default = new();

    public string Qualifier { get; init; } = "com";
    public string Organization { get; init; } = "Unknown";
    public string Application { get; init; } = "Midnight Program";
    public GraphicsConfig Graphics { get; init; } = GraphicsConfig.Default;
}
