using System.Collections.Generic;

namespace Midnight.Assets.Aseprite;

public class AsepritePalette {
    public AsepritePalette() {
        Entries = new();
    }

    public AsepritePalette(int colorCount) {
        Entries = new(colorCount);
    }

    public List<Entry> Entries { get; }
    public Entry this[int n] { get => Entries[n]; }

    public void Add(Color c) {
        Entries.Add(new(c));
    }

    public override string ToString() {
        return $"{GetType()}  Colors: {Entries.Count}";
    }

    public class Entry {
        public Entry(Color c) {
            Color = c;
        }

        public Color Color { get; }
    }
}
