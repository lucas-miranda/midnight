using XnaGraphics = Microsoft.Xna.Framework.Graphics;
using Midnight.Diagnostics;

namespace Midnight;

public class ShaderPass {
    internal ShaderPass(XnaGraphics.EffectPass xnaPass, int id) {
        Assert.NotNull(xnaPass);
        Underlying = xnaPass;
        Id = id;
    }

    public string Name { get => Underlying.Name; }
    public int Id { get; }
    internal XnaGraphics.EffectPass Underlying { get; }

    public void Apply() {
        Underlying.Apply();
    }
}
