using System.Collections.Generic;

namespace Midnight;

/// <summary>
/// Describes how glyphs are loaded and has every data needed to combine them into a renderable text.
/// </summary>
public interface IFontTypesetting : IAsset {
    /// <summary>
    /// It's the distance (em) from baseline to the top-most point.
    /// </summary>
    float Ascender { get; }

    /// <summary>
    /// It's the distance (em) from baseline to the bottom-most point.
    /// </summary>
    float Descender { get; }

    /// <summary>
    /// Size of one em in pixels.
    /// </summary>
    float NominalWidth { get; }

    /// <summary>
    /// Vertical difference between consecutive baselines.
    /// </summary>
    float LineHeight { get; }

    /// <summary>
    /// Has kerning data.
    /// </summary>
    bool HasKerning { get; }

    /// <summary>
    /// Generate every glyph within font data.
    /// </summary>
    /// <returns>
    /// A dictionary keyed by unicode value and it's Glyph representation.
    /// </returns>
    Dictionary<uint, Glyph> GenerateGlyphs();

    /// <summary>
    /// Calculate the kerning between two unicodes.
    /// </summary>
    /// <returns>
    /// A value (in em) representating the distance between the two unicodes.
    /// </returns>
    float Kerning(uint unicodeA, uint unicodeB);
}
