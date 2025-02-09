
namespace Midnight;

public class App : Program {
    public App(GraphicsConfig? config = null) : base(config) {
        Background = 0x8C7AA1FF;
    }

    public static new App Current => (App) Program.Current;
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
	    // game rendering on MainCanvas
        r.Target.Push(r.MainCanvas);
	    r.Clear(Background);
		r.PrepareRender();
        Scene?.Render(dt, r);
        r.Flush();
        r.Target.Pop(); // MainCanvas
	}
}
