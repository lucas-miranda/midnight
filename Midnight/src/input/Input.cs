
namespace Midnight;

public sealed class Input {
    private static Input _instance;

    public static Keyboard Keyboard { get; private set; }
    public static Mouse Mouse { get; private set; }

    internal static void Initialize() {
        if (_instance != null) {
            return;
        }

        _instance = new();
        Keyboard = new();
        Mouse = new();
    }

    internal static void Update(DeltaTime dt) {
        Keyboard.Update(dt);
        Mouse.Update(dt);
    }
}
