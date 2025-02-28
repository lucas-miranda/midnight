namespace Midnight;

public class Game : Program {
    public Game(MidnightConfig? config = null) : base(config) {
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
        Scene?.Begin();
    }

	protected override void ResourceRelease() {
        Scene?.End();
	    //Scene?.Dispose();
    }

	protected override void Update(DeltaTime dt) {
        Scene?.Update(dt);
    }

	protected override void Render(DeltaTime dt) {
	    RenderingServer.Clear(Background);
		RenderingServer.PrepareRender();
        Scene?.Render(dt);
        RenderingServer.Flush();
	}
}
