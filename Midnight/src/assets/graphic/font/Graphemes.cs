namespace Midnight;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A collection of <see cref="Grapheme"/>.
/// </summary>
public readonly struct Graphemes : IEnumerable<Grapheme> {
    public Graphemes(string text) {
        ReadOnlySpan<char> buffer = text.AsSpan();
        Entries = new(buffer.Length);

        foreach (Rune rune in buffer.EnumerateRunes()) {
            System.Console.WriteLine($"{rune}; is meaningful? {IsMeaningful(rune)} (category: {Rune.GetUnicodeCategory(rune)})");

            if (!IsMeaningful(rune)) {
                // can be ignored
                continue;
            }

            // prepare a new grapheme
            Grapheme grapheme = new(rune);

            if (!Entries.IsEmpty()) {
                // additional preprocessing

                if (grapheme.IsNewLine) {
                    // mark previous grapheme as eol
                    // [.., __, \n]
                    //      /\

                    Grapheme last = Entries[^1];
                    last.IsEOL = !last.IsCarriageReturn;
                } else if (grapheme.IsCarriageReturn) {
                    // mark previous grapheme as eol
                    // [.., __, \r]
                    //      /\

                    Grapheme last = Entries[^1];
                    last.IsEOL = true;
                }
            }

            Entries.Add(grapheme);
        }

        if (!Entries.IsEmpty()) {
            // always make last grapheme as eol (if possible)
            // [.., __]
            //      /\
            Grapheme last = Entries[^1];
            last.IsEOL = !(last.IsCarriageReturn || last.IsNewLine);
        }
    }

    public int Length => Entries.Count;
    public Grapheme this[int i] => Entries[i];

    private List<Grapheme> Entries { get; }

    /// <summary>
    /// Verify if a rune is a symbol, letter, digit or separator.
    /// Anything which is meaningful to be rendered.
    /// </summary>
    private bool IsMeaningful(in Rune rune) {
        return Rune.IsLetterOrDigit(rune)
            || Rune.IsSeparator(rune)
            || Rune.IsSymbol(rune)
            || Rune.IsControl(rune);
    }

    public IEnumerator<Grapheme> GetEnumerator() {
        return Entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

/// <summary>
/// It represents an unicode character.
/// A safe representation which encompass more than a single byte (up to four).
/// </summary>
public struct Grapheme {
    public static readonly Grapheme NewLine = new('\n'),
                                    CarriageReturn = new('\r'),
                                    Tab = new('\t'),
                                    WhiteSpace = new(' '),
                                    Replacement = new(Rune.ReplacementChar);

    public Grapheme(Rune rune) {
        Rune = rune;
    }

    public Grapheme(char c) : this(new Rune(c)) {
    }

    /// <summary>
    /// <see cref="Rune"/> represented by this <see cref="Grapheme"/>.
    /// </summary>
    public Rune Rune { get; }

    /// <summary>
    /// A scalar value, an identification.
    /// </summary>
    public uint Unicode => (uint) Rune.Value;

    /// <summary>
    /// Is this newline (\n) control character?
    /// </summary>
    public bool IsNewLine => Rune == NewLine.Rune;

    /// <summary>
    /// Is this carriage return (\r) control character?
    /// </summary>
    public bool IsCarriageReturn => Rune == CarriageReturn.Rune;

    /// <summary>
    /// Is this tabulation (\t) control character?
    /// </summary>
    public bool IsTab => Rune == Tab.Rune;

    /// <summary>
    /// Is this grapheme at the end of a line?
    /// </summary>
    public bool IsEOL { get; set; }

    /// <summary>
    /// Prepares the correct representation of this Grapheme to be rendered.
    /// </summary>
    /// <remarks>
    /// Example: A tabulation (\t) is represented by whitespaces ( ) when rendered.
    /// </remarks>
    public (uint Unicode, int Repeat) Render(FontBuildSettings s) {
        if (IsTab) {
            return (Grapheme.WhiteSpace.Unicode, s.TabWhitespaceSize);
        }

        return (Unicode, 1);
    }

    public override string ToString() {
        return Rune.ToString();
    }
}
