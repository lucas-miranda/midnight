using Xna = Microsoft.Xna.Framework;

namespace Midnight;

public class GraphicsServer {
    private static GraphicsServer _instance;

    private GraphicsServer(Program program) {
		XnaGDM = new Xna.GraphicsDeviceManager(program);
		Display = new(XnaGDM);
		BackBuffer = new(XnaGDM);
    }

    public static Display Display { get; private set; }
    public static BackBuffer BackBuffer { get; private set; }

    public static GraphicsProfile Profile {
        get => (GraphicsProfile) XnaGDM.GraphicsProfile;
        set => XnaGDM.GraphicsProfile = value.ToXna();
    }

    internal static Xna.GraphicsDeviceManager XnaGDM { get; private set; }

    public static void ApplyConfig(GraphicsConfig config) {
        LoadConfig(config);
        ApplyChanges();
    }

    public static void LoadConfig(GraphicsConfig config) {
        Display.LoadConfig(config.Display);
        BackBuffer.LoadConfig(config.BackBuffer);
        Profile = config.Profile;
    }

    public static void ApplyChanges() {
        XnaGDM.ApplyChanges();
    }

    public static string AsString() {
        return _instance.ToString();
    }

    public override string ToString() {
        return $"Profile: {Profile};\nDisplay: ({Display});\nBackBuffer: ({BackBuffer});";
    }

    internal static void Initialize(Program program) {
        if (_instance != null) {
            return;
        }

        _instance = new(program);
        _instance.Initialized();
    }

    private void Initialized() {
    }
}
