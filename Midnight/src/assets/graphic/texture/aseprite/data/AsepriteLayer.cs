using System.Collections.Generic;
using System.Collections.ObjectModel;
using Midnight.Diagnostics;

namespace Midnight.Assets.Aseprite;

public abstract class AsepriteLayer {
    public AsepriteLayer() {
        Cels = CelsEntries.AsReadOnly();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public ushort ChildLevel { get; set; }
    public byte Opacity { get; set; }
    public AsepriteLayerFlags Flags { get; set; }
    public AsepriteBlendMode BlendMode { get; set; }
    public List<AsepriteLayer> Children { get; } = new();
    public ReadOnlyCollection<AsepriteCel> Cels { get; }
    protected List<AsepriteCel> CelsEntries { get; } = new();

    public void Add(int frameIndex, AsepriteCel cel) {
        Assert.True(frameIndex >= CelsEntries.Count, $"Frame index: {frameIndex}, Cels: {CelsEntries.Count}");
        for (int i = CelsEntries.Count - 1; i < frameIndex - 1; i++) {
            CelsEntries.Add(null);
        }

        CelsEntries.Add(cel);
    }

    public override string ToString() {
        return $"{GetType()}  Name: {Name}; ChildLevel: {ChildLevel}; Opacity: {Opacity}; Flags: {new BitTag(Flags)}; BlendMode: {BlendMode}; Children: {Children.Count}";
    }
}
