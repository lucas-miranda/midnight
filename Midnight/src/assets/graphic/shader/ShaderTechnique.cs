using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class ShaderTechnique {
    internal ShaderTechnique(XnaGraphics.EffectTechnique xnaTechnique, int id) {
        Debug.AssertNotNull(xnaTechnique);
        Underlying = xnaTechnique;
        Id = id;
    }

    public string Name { get => Underlying.Name; }
    public int Id { get; }
    internal XnaGraphics.EffectTechnique Underlying { get; }

    public override string ToString() {
        return $"#{Id}: {Name}";
    }
}
