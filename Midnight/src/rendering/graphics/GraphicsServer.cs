using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public class GraphicsServer {
    internal GraphicsServer(Program program) {
		XnaGDM = new Xna.GraphicsDeviceManager(program);
		Display = new(XnaGDM);
		BackBuffer = new(XnaGDM);
    }

    public Display Display { get; }
    public BackBuffer BackBuffer { get; }

    public GraphicsProfile Profile {
        get => (GraphicsProfile) XnaGDM.GraphicsProfile;
        set => XnaGDM.GraphicsProfile = value.ToXna();
    }

    internal Xna.GraphicsDeviceManager XnaGDM { get; }

    public void ApplyConfig(GraphicsConfig config) {
        LoadConfig(config);
        ApplyChanges();
    }

    public void LoadConfig(GraphicsConfig config) {
        Display.LoadConfig(config.Display);
        BackBuffer.LoadConfig(config.BackBuffer);
        Profile = config.Profile;
    }

    public void ApplyChanges() {
        XnaGDM.ApplyChanges();
    }

    public override string ToString() {
        return $"Profile: {Profile};\nDisplay: ({Display});\nBackBuffer: ({BackBuffer});";
    }
}
