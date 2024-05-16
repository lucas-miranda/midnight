namespace Midnight;

public class Game : Program {
    public Game(GraphicsConfig? config = null) : base(config) {
    }

    public static new Game Current { get => (Game) Program.Current; }
	public Scene Scene { get; private set; }

	public void LoadScene<S>() where S : Scene, new() {
	    Scene = new S();
    }

	public void LoadScene(Scene scene) {
	    Scene = scene;
    }

	protected override void Setup() {
	    //MainCanvas = new Canvas();
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
