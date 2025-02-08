using Xna = Microsoft.Xna.Framework;
using Midnight.Diagnostics;

namespace Midnight;

public abstract class Program : Xna.Game {
    private int _framesRendered;

	public Program(GraphicsConfig? config = null) {
	    Assert.Null(Current, "Only one Program can exists.");
	    Current = this;

	    // Debug
#if DEBUG
        Debug = new();
#endif

	    // Modules

        Resources = new();
        Resources.LoadAll();
        Random.Init();
        AssetManager = new();

        // Graphics

        Graphics = new GraphicsServer(this);

        if (config.HasValue) {
            Graphics.LoadConfig(config.Value);
        } else {
            Graphics.LoadConfig(GraphicsConfig.Default);
        }

		System.Console.WriteLine($"Initial graphics:\n{Graphics}");
	}

    public static Program Current { get; private set; }
    public static RenderingServer Rendering { get; private set; }
    public static GraphicsServer Graphics { get; private set; }
    public static Embedded.Resources Resources { get; private set; }
    public static AssetManager AssetManager { get; private set; }

    public static Debug Debug {
#if DEBUG
        get;
        private set;
#else
        get { return null; }
#endif
    }

    public Color Background { get; set; } = new(0x834EBFFF);
    public FPS FPS { get; } = new();

	protected sealed override void Initialize() {
	    Debug.Initialize();
        Rendering = new(GraphicsDevice);
		base.Initialize();
		DeviceInit();
		System.Console.WriteLine($"At Initialize, graphics:\n{Graphics}");
	}

    /*
     *DeviceReady
InitResources
ReadyToLoad
PrepareGraphics
LoadReady
DeviceInit

SetupGraphics

LoadGraphics
ResourcesReady


-> GraphicsReady
     */
	protected sealed override void LoadContent() {
		base.LoadContent();
		Resources.GraphicsReady();
		Debug.GraphicsReady();
        Rendering.GraphicsReady();
		GraphicsReady();
		System.Console.WriteLine($"At LoadContent, graphics:\n{Graphics}");
	}

	protected sealed override void UnloadContent() {
		base.UnloadContent();
		ResourceRelease();
		Rendering.ResourceRelease();
		Debug.ResourceRelease();
	}

	protected sealed override void Update(Xna.GameTime gameTime) {
        DeltaTime deltaTime = new(gameTime);
        FPS.Update(deltaTime);
		Update(deltaTime);
	}

	protected sealed override void Draw(Xna.GameTime gameTime) {
        DeltaTime deltaTime = new(gameTime);
        FPS.PreFrameRendered();
		Render(deltaTime, Rendering);
		Debug.Render(deltaTime, Rendering);

        //System.Console.WriteLine("\nRendering Layers Begin");
        // render canvas layes to backbuffer
	    Rendering.Target.Clear();
	    Rendering.Clear(Background);
	    Rendering.Layers.Render(deltaTime, Rendering);
        //System.Console.WriteLine("Rendering Layers End\n");
	}

    protected abstract void DeviceInit();
    protected abstract void GraphicsReady();
    protected abstract void ResourceRelease();
    protected abstract void Update(DeltaTime dt);
    protected abstract void Render(DeltaTime dt, RenderingServer r);
}
