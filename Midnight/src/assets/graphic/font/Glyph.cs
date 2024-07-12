
namespace Midnight;


/// <summary>
/// Data related to an unicode.
/// Generated at <see cref="FontTypesetting"/> and handled by <see cref="Font{T}"/>.
/// </summary>
/// <param name="SourceArea">Area where glyph is located at atlas texture.</param>
/// <param name="Bearing">
/// Left and top bearings.
/// Left bearing is the horizontal distance from the cursor to the leftmost border of the glyph's bounding box.
/// Top bearing is the vertical distance from the cursor (on the baseline) to the topmost border of the glyph's bounding box.
/// </param>
/// <param name="Size">Width and height of glyph bounding box.</param>
/// <param name="Advance">Distance, on both axis, to move pen position when this glyph is drawn.</param>
public readonly record struct Glyph(Rectangle SourceArea, Vector2 Bearing, Size2 Size, Vector2 Advance) {
}
