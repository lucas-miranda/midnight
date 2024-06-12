
namespace Midnight;

public class App : Program {
    public App(GraphicsConfig? config = null) : base(config) {
        Background = 0x8C7AA1FF;
    }

    public static new App Current { get => (App) Program.Current; }
	public Scene Scene { get; private set; }

	public void LoadScene<S>() where S : Scene, new() {
	    Scene = new S();
    }

	public void LoadScene(Scene scene) {
	    Scene = scene;
    }

	protected override void Setup() {
    }

	protected override void Load() {
        Scene?.Prepare();
        Scene?.Start();
    }

	protected override void Unload() {
    }

	protected override void Update(DeltaTime dt) {
        Scene?.Update(dt);
    }

	protected override void Render(DeltaTime dt, RenderingServer r) {
		Rendering.PrepareRender();
        Scene?.Render(dt, Rendering);
        Rendering.Flush();
	}
}
