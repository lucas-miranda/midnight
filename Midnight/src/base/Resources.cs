using System.IO;

namespace Midnight.Embedded;

public class Resources {
    public const string ManifestNamespace = "Midnight.Embedded";

    internal void LoadAll() {
        Shaders.Sprite = Load("SpriteShader");
        Fonts.Shaders.MTSDF = Load("MTSDFShader");
        Fonts.AccidentalPresident.Data = Load("AccidentalPresidentFontData");
        Fonts.AccidentalPresident.Texture = Load("AccidentalPresidentFontTexture");
    }

    private byte[] Load(string name) {
        using (Stream stream = GetType().Assembly.GetManifestResourceStream(string.Join('.', ManifestNamespace, name))) {
            using (MemoryStream ms = new()) {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    public static class Fonts {
        public static class Shaders {
            public static byte[] MTSDF { get; internal set; }
        }

        public static class AccidentalPresident {
            public static byte[] Data { get; internal set; }
            public static byte[] Texture { get; internal set; }
        }
    }

    public static class Shaders {
        public static byte[] Sprite { get; internal set; }
    }
}

