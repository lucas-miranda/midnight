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

        Embedded.Resources.Initialize();
        Random.Init();
        AssetManager.Initialize();

        // Graphics
        GraphicsServer.Initialize(this);

        if (config.HasValue) {
            GraphicsServer.LoadConfig(config.Value);
        } else {
            GraphicsServer.LoadConfig(GraphicsConfig.Default);
        }

		System.Console.WriteLine($"Initial graphics:\n{GraphicsServer.AsString()}");
	}

    public static Program Current { get; private set; }

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
        RenderingServer.Initialize(GraphicsDevice);
		base.Initialize();
		DeviceInit();
		System.Console.WriteLine($"At Initialize, graphics:\n{GraphicsServer.AsString()}");
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
		Embedded.Resources.GraphicsReady();
		Debug.GraphicsReady();
        RenderingServer.GraphicsReady();
		GraphicsReady();
		System.Console.WriteLine($"At LoadContent, graphics:\n{GraphicsServer.AsString()}");
	}

	protected sealed override void UnloadContent() {
		base.UnloadContent();
		ResourceRelease();
		RenderingServer.ResourceRelease();
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
		Render(deltaTime);
		Debug.Render(deltaTime);

        //System.Console.WriteLine("\nRendering Layers Begin");
        // render canvas layes to backbuffer
	    RenderingServer.Target.Clear();
	    RenderingServer.Clear(Background);
	    RenderingServer.Layers.Render(deltaTime);
        //System.Console.WriteLine("Rendering Layers End\n");
	}

    protected abstract void DeviceInit();
    protected abstract void GraphicsReady();
    protected abstract void ResourceRelease();
    protected abstract void Update(DeltaTime dt);
    protected abstract void Render(DeltaTime dt);
}
