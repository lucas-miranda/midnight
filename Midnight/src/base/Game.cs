namespace Midnight;

public class Game : Program {
    public Game(GraphicsConfig? config = null) : base(config) {
    }

    public static new Game Current => (Game) Program.Current;
	public Scene Scene { get; private set; }

	public void LoadScene<S>() where S : Scene, new() {
	    Scene = new S();
    }

	public void LoadScene(Scene scene) {
	    Scene = scene;
    }

	protected override void DeviceInit() {
    }

	protected override void GraphicsReady() {
        Scene?.Prepare();
        Scene?.Start();
    }

	protected override void ResourceRelease() {
	    //Scene?.Dispose();
    }

	protected override void Update(DeltaTime dt) {
        Scene?.Update(dt);
    }

	protected override void Render(DeltaTime dt, RenderingServer r) {
	    r.Clear(Background);
		r.PrepareRender();
        Scene?.Render(dt, r);
        r.Flush();
	}
}
