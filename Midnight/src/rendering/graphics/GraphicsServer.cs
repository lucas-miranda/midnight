using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public class GraphicsServer {
    internal GraphicsServer(Program program) {
		XnaGDM = new Xna.GraphicsDeviceManager(program);
		Display = new(XnaGDM);
		BackBuffer = new(XnaGDM);
    }

    public GraphicsProfile Profile { get => (GraphicsProfile) XnaGDM.GraphicsProfile; }
    public Display Display { get; }
    public BackBuffer BackBuffer { get; }

    internal Xna.GraphicsDeviceManager XnaGDM { get; }

    public void ApplyConfig(GraphicsConfig config) {
        LoadConfig(config);
        ApplyChanges();
    }

    public void LoadConfig(GraphicsConfig config) {
        Display.ApplyConfig(config.Display);
        BackBuffer.ApplyConfig(config.BackBuffer);
    }

    public void ApplyChanges() {
        XnaGDM.ApplyChanges();
    }

    public override string ToString() {
        return $"Profile: {Profile}; Display: ({Display}); BackBuffer: ({BackBuffer});";
    }
}
