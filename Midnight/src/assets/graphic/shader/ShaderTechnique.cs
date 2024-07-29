using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class ShaderTechnique {
    internal ShaderTechnique(XnaGraphics.EffectTechnique xnaTechnique, int id) {
        Assert.NotNull(xnaTechnique);
        Underlying = xnaTechnique;
        Id = id;

        Passes = new ShaderPass[Underlying.Passes.Count];
        int i = 0;
        foreach (XnaGraphics.EffectPass xnaPass in Underlying.Passes) {
            Passes[i] = new(xnaPass, i);
            i += 1;
        }
    }

    public string Name { get => Underlying.Name; }
    public int Id { get; }
    public ShaderPass[] Passes { get; }

    internal XnaGraphics.EffectTechnique Underlying { get; }

    public override string ToString() {
        return $"#{Id}: {Name}";
    }
}
