
namespace Midnight;

public readonly record struct DisplayConfig() {
    public DisplayOrientation SupportedOrientations { get; init; } = DisplayOrientation.Default;
}
