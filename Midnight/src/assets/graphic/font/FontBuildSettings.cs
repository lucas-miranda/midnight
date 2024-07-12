
namespace Midnight;

/// <summary>
/// Configurations when building a text from a font.
/// </summary>
public record struct FontBuildSettings {
    public FontBuildSettings() {
    }

    /// <summary>
    /// Amount of whitespaces to use when replacing tabulation at the final result.
    /// </summary>
    public int TabWhitespaceSize { get; init; } = 4;
}
