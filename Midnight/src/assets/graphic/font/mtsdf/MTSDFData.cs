using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Midnight.Assets.Font.MTSDF;

[JsonConverter(typeof(JsonStringEnumConverter<YOrigin>))]
public enum YOrigin {
    Bottom,
    Top,
}

public record struct MTSDFData {
    public Atlas Atlas = new();
    public Metrics Metrics = new();

    [JsonConverter(typeof(GlyphsConverter))]
    public Dictionary<uint, Glyph> Glyphs = new();

    [JsonConverter(typeof(KerningConverter))]
    public Dictionary<uint, Dictionary<uint, Kerning>> Kerning = new();

    public MTSDFData() {
    }
}

public record struct Atlas {
    public float DistanceRange, Size, Width, Height;
    public YOrigin YOrigin;

    public Atlas() {
    }
}

public record struct Metrics {
    public double SizeEm;
    public float LineHeight, Ascender, Descender, UnderlineY, UnderlineThickness;

    public Metrics() {
    }
}

public record struct Glyph {
    public uint Unicode;
    public float Advance;

    public GlyphBounds PlaneBounds, AtlasBounds;
}

public record struct GlyphBounds {
    public float Top, Right, Bottom, Left;

    public Rectangle ToRect() {
        return new(new Vector2(Left, Top), new Vector2(Right, Bottom));
    }
}

public record struct Kerning {
    public uint UnicodeA, UnicodeB;
    public float Advance;
}

public class GlyphsConverter : JsonConverter<Dictionary<uint, Glyph>> {
    public override Dictionary<uint, Glyph> Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartArray) {
            throw new JsonException();
        }

        Dictionary<uint, Glyph> value = new();
        JsonConverter<Glyph> glyphConverter = (JsonConverter<Glyph>) JsonSerializerOptions.Default.GetConverter(typeof(Glyph));

        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndArray) {
                return value;
            }

            Glyph glyph = glyphConverter.Read(ref reader, typeof(Glyph), options);
            value[glyph.Unicode] = glyph;
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<uint, Glyph> value, JsonSerializerOptions options) {
        throw new System.NotImplementedException();
    }
}

public class KerningConverter : JsonConverter<Dictionary<uint, Dictionary<uint, Kerning>>> {
    public override Dictionary<uint, Dictionary<uint, Kerning>> Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartArray) {
            throw new JsonException();
        }

        Dictionary<uint, Dictionary<uint, Kerning>> value = new();
        JsonConverter<Kerning> kerningConverter = (JsonConverter<Kerning>) JsonSerializerOptions.Default.GetConverter(typeof(Kerning));

        while (reader.Read()) {
            if (reader.TokenType == JsonTokenType.EndArray) {
                return value;
            }

            Kerning kerning = kerningConverter.Read(ref reader, typeof(Kerning), options);

            if (!value.TryGetValue(kerning.UnicodeA, out Dictionary<uint, Kerning> nextEntries)) {
                value[kerning.UnicodeA] = new();
            }

            nextEntries[kerning.UnicodeB] = kerning;
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<uint, Dictionary<uint, Kerning>> value, JsonSerializerOptions options) {
        throw new System.NotImplementedException();
    }
}
