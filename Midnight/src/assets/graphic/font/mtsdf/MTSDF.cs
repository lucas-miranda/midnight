using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Midnight.Assets.Font.MTSDF;
using Midnight.Diagnostics;

namespace Midnight;

/// <summary>
/// Multi-channel signed distance field font.
/// Uses multiple color channels to reconstruct almost perfectly a glyph at varied sizes without losing quality.
/// </summary>
/// <remarks>
/// It's an interface to use data from Viktor Chlumský incredible work, licensed under the MIT license.
/// Using his generator to generate: a tight packed PNG and a JSON data, both to be consumed by this interface.
///
/// Original repo: <see cref="https://github.com/Chlumsky/msdfgen"/>
/// Generator repo: <see cref="https://github.com/Chlumsky/msdf-atlas-gen"/>
/// </remarks>
public class MTSDF : IFontTypesetting {
    public MTSDF(Texture2D texture, string dataFilepath) {
        Assert.NotNull(texture);
        Assert.NotNull(dataFilepath);

        Texture = texture;

        TextureFilepath = texture.Filepath;
        DataFilepath = dataFilepath;

        Filepaths = new[] {
            TextureFilepath,
            DataFilepath
        };

        LoadData();
    }

    public MTSDF(Texture2D texture, Stream dataStream) {
        Assert.NotNull(texture);
        Assert.NotNull(dataStream);

        Texture = texture;
        Filepaths = new string[0];

        LoadData(dataStream);
    }

    /// <summary>
    /// Glyphs atlas texture.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    /// MTSDF data.
    /// </summary>
    public MTSDFData Data { get; private set; }

    public string Name { get; }
    public string[] Filepaths { get; }
    public string TextureFilepath { get; }
    public string DataFilepath { get; }
    public float Ascender => Data.Metrics.Ascender;
    public float Descender => Data.Metrics.Descender;
    public float NominalWidth => Data.Atlas.Size;
    public float LineHeight => Data.Metrics.LineHeight;
    public bool HasKerning => !Data.Kerning.IsEmpty();
    public bool IsDisposed { get; private set; }

    public static Font<MTSDF> LoadFont(Texture2D texture, string dataFilepath) {
        return new(new(texture, dataFilepath));
    }

    public static Font<MTSDF> LoadFont(Texture2D texture, Stream dataStream) {
        return new(new(texture, dataStream));
    }

    public Dictionary<uint, Glyph> GenerateGlyphs() {
        Dictionary<uint, Glyph> glyphs = new();

        foreach (KeyValuePair<uint, Assets.Font.MTSDF.Glyph> entry in Data.Glyphs) {
            glyphs[entry.Key] = new(
                // source area
                entry.Value.AtlasBounds.ToRect(),

                // bearing
                new(entry.Value.PlaneBounds.Left, entry.Value.PlaneBounds.Top),

                // size
                new(
                    entry.Value.PlaneBounds.Right - entry.Value.PlaneBounds.Left,
                    entry.Value.PlaneBounds.Bottom - entry.Value.PlaneBounds.Top
                ),

                // advance
                new(entry.Value.Advance, 0.0f)
            );
        }

        return glyphs;
    }

    public float Kerning(uint unicodeA, uint unicodeB) {
        if (!Data.Kerning.TryGetValue(unicodeA, out Dictionary<uint, Kerning> nextEntries)) {
            // there is no kerning info for unicodeA
            return 0.0f;
        }

        if (!nextEntries.TryGetValue(unicodeB, out Kerning kerning)) {
            // there is no kerning for unicodeA and unicodeB
            return 0.0f;
        }

        return kerning.Advance;
    }

    public void Reload() {
        Texture.Reload();
        LoadData();
    }

    public void Reload(Stream stream) {
        throw new System.NotImplementedException();
    }

    public void Dispose() {
        IsDisposed = true;
        Texture?.Dispose();
    }

    private void LoadData(Stream dataStream) {
        Assert.NotNull(dataStream);

        Data = JsonDocument.Parse(dataStream).Deserialize<MTSDFData>(
            new JsonSerializerOptions() {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true,
            }
        );
    }

    private void LoadData() {
        if (string.IsNullOrEmpty(DataFilepath)) {
            return;
        }

        using (FileStream dataStream = File.OpenRead(DataFilepath)) {
            LoadData(dataStream);
        }
    }
}
