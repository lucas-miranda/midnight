using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public class Display {
    internal Display(Xna.GraphicsDeviceManager xnaGDM) {
        XnaGDM = xnaGDM;
    }

    public DisplayOrientation SupportedOrientations {
        get => (DisplayOrientation) XnaGDM.SupportedOrientations;
        set => XnaGDM.SupportedOrientations = value.ToXna();
    }

    internal Xna.GraphicsDeviceManager XnaGDM { get; }

    public override string ToString() {
        return $"SupportedOrientations: {SupportedOrientations}";
    }

    internal void LoadConfig(DisplayConfig config) {
        SupportedOrientations = config.SupportedOrientations;
    }
}
