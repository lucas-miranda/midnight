
namespace Midnight;

public class App : Program {
    public App(MidnightConfig? config = null) : base(config) {
        Background = 0x8C7AA1FF;
        IsMouseVisible = true;
    }

    public static new App Current => (App) Program.Current;
	public Scene Scene { get; private set; }

	public void LoadScene<S>() where S : Scene, new() {
	    LoadScene(new S());
    }

	public void LoadScene(Scene scene) {
        Scene?.End();
	    Scene = scene;
        Scene?.Begin();
    }

	protected override void DeviceInit() {
    }

	protected override void GraphicsReady() {
        Scene?.Prepare();
        Scene?.Start();
    }

	protected override void ResourceRelease() {
        Scene?.End();
	    //Scene?.Dispose();
    }

	protected override void Update(DeltaTime dt) {
        Scene?.Update(dt);
    }

	protected override void Render(DeltaTime dt) {
	    // game rendering on MainCanvas
        RenderingServer.Target.Push(RenderingServer.MainCanvas);
	    RenderingServer.Clear(Background);
		RenderingServer.PrepareRender();
        Scene?.Render(dt);
        RenderingServer.Flush();
        RenderingServer.Target.Pop(); // MainCanvas
	}
}
