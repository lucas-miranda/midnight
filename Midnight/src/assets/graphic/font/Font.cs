using System.Collections.Generic;
using Midnight.Diagnostics;

namespace Midnight;

/// <summary>
/// A font has everything needed to prepare a text to be rendered.
/// </summary>
/// <remarks>
/// It uses a <see cref="FontTypesetting"/> to describe glyphs and it's where the most important data are.
/// </remarks>
public abstract class Font : IAsset {
    public Font(IFontTypesetting typesetting) {
        Assert.NotNull(typesetting);
        Typesetting = typesetting;
        Glyphs = Typesetting.GenerateGlyphs();
    }

    public string Name { get; set; }
    public string[] Filepaths => Typesetting.Filepaths;
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Describes glyphs to build a text.
    /// </summary>
    public virtual IFontTypesetting Typesetting { get; }

    /// <summary>
    /// Font size in pixels.
    /// </summary>
    public float Size { get; set; } = 32.0f;

    /// <summary>
    /// When a glyph isn't found, this Grapheme will be used instead.
    /// </summary>
    /// <remarks>
    /// If defined as null, it'll use <see cref="Grapheme.Replacement"/>.
    /// If a font file doesn't have it, in this case, the first found glyph will be used instead.
    /// </remarks>
    public Grapheme? Replacement { get; set; } = new('?');

    /// <summary>
    /// All glyph data extracted from <see cref="Typesetting"/>.
    /// </summary>
    private Dictionary<uint, Glyph> Glyphs { get; }

    /// <summary>
    /// Get a Glyph by it's unicode.
    /// </summary>
    /// <remarks>
    /// It'll throw an <see cref="KeyNotFoundException"/> if there is no glyph registered at that unicode.
    /// </remarks>
    public Glyph Glyph(uint unicode) {
        return Glyphs[unicode];
    }

    /// <summary>
    /// Try to get a Glyph by it's unicode.
    /// </summary>
    public bool TryGetGlyph(uint unicode, out Glyph glyph) {
        return Glyphs.TryGetValue(unicode, out glyph);
    }

    /// <summary>
    /// Builds a text to be rendered, compiling every information in a <see cref="TextTypesetting"/>.
    /// An optional settings can be provided to configure the final result.
    /// </summary>
    public TextTypesetting Build(string text, FontBuildSettings? settings = null) {
        if (text == null) {
            return null;
        }

        // use provided settings or a default one
        FontBuildSettings s = settings.GetValueOrDefault();

        // prepare graphemes based on input text
        Graphemes graphemes = new(text);

        // use a DynamicTextTypesetting as we don't know exactly how much space we'll need
        DynamicTextTypesetting output = new(text.Length);

        // text size (em)
        Size2 size = Size2.Empty;

        // where current grapheme will be placed (using em)
        Vector2 pen = new(0.0f, Math.Abs(Typesetting.Ascender));

        // grapheme index
        int i = -1;

        foreach (Grapheme grapheme in graphemes) {
            i += 1;

            if (grapheme.IsNewLine) {
                // go to the next line
                pen.Y += Typesetting.LineHeight;
                pen.X = 0.0f;
                continue;
            } else if (grapheme.IsCarriageReturn) {
                // do nothing, just ignore
                // TODO: maybe add an option to detect when carriage return handling is needed
                continue;
            }

            (uint unicode, int repeat) = grapheme.Render(s);

            if (!TryGetGlyph(unicode, out Glyph glyph)) {
                // glyph not found, just render default symbol

                if (!TryGetGlyph(Replacement.GetValueOrDefault(Grapheme.Replacement).Unicode, out glyph)) {
                    // failed to retrieve repĺacement glyph
                    // use first found glyph
                    foreach (Glyph g in Glyphs.Values) {
                        glyph = g;
                        break;
                    }

                    Assert.NotNull(glyph, "There is no glyph available.");
                }
            }

            int k = 0;
            while (k < repeat) {
                //
                // underrun
                //

                if (pen.X == 0.0f) {
                    pen.X -= glyph.Bearing.X;
                }

                //

                output.Append(pen + glyph.Bearing, unicode, glyph);
                pen += glyph.Advance;//

                //
                // kerning with next repeated character
                //

                // adjust for kerning between this character
                // and the next (if it'll repeat)
                if (Typesetting.HasKerning && !grapheme.IsEOL && repeat > 1) {
                    pen.X += Typesetting.Kerning(unicode, unicode);
                }

                //
                // kerning with next character
                //

                if (Typesetting.HasKerning && !grapheme.IsEOL) {
                    (uint nextUnicode, _) = graphemes[i + 1].Render(s);
                    pen.X += Typesetting.Kerning(unicode, nextUnicode);
                }

                //

                if (grapheme.IsEOL) {
                    if (pen.X > size.Width) {
                        size.Width = pen.X;
                    }

                    pen.X = 0.0f;
                }

                k += 1;
            }
        }

        size.Height = Math.Abs(pen.Y) + Typesetting.Descender;
        output.Size = size;
        return output.ToReadOnly();
    }

    public void Reload() {
        Typesetting.Reload();
    }

    public void Reload(System.IO.Stream stream) {
        Typesetting.Reload(stream);
    }

    public void Dispose() {
        Typesetting.Dispose();
    }
}


/// <inheritdoc/>
public class Font<T> : Font where T : class, IFontTypesetting {
    public Font(T typesetting) : base(typesetting) {
    }

    /// <inheritdoc/>
    public override T Typesetting => (T) base.Typesetting;
}
