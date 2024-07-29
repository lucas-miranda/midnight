using Xna = Microsoft.Xna.Framework;
using Midnight.Diagnostics;

namespace Midnight;

public abstract class Program : Xna.Game {
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

    public static Debug Debug {
#if DEBUG
        get;
        private set;
#else
        get { return null; }
#endif
    }

    public Color Background { get; set; } = new(0x834EBFFF);

	protected sealed override void Initialize() {
	    Debug.Initialize();
        Rendering = new(GraphicsDevice);
		base.Initialize();
		Setup();
		System.Console.WriteLine($"At Initialize, graphics:\n{Graphics}");
	}

    /*
     *DeviceReady
LoadGraphics
InitResources
ReadyToLoad
GraphicsReady
PrepareGraphics
ResourcesReady
LoadReady
DeviceInit
SetupGraphics
     */
	protected sealed override void LoadContent() {
		base.LoadContent();
		Debug.LoadContent();
        Rendering.LoadContent();
		Load();
		System.Console.WriteLine($"At LoadContent, graphics:\n{Graphics}");
	}

	protected sealed override void UnloadContent() {
		base.UnloadContent();
		Unload();
		Rendering.UnloadContent();
		Debug.UnloadContent();
	}

	protected sealed override void Update(Xna.GameTime gameTime) {
		base.Update(gameTime);
		Update(new((float) gameTime.ElapsedGameTime.TotalSeconds));
	}

	protected sealed override void Draw(Xna.GameTime gameTime) {
		base.Draw(gameTime);
		Render(
            new((float) gameTime.ElapsedGameTime.TotalSeconds),
            Rendering
		);
	}

    protected abstract void Setup();
    protected abstract void Load();
    protected abstract void Unload();
    protected abstract void Update(DeltaTime dt);
    protected abstract void Render(DeltaTime dt, RenderingServer r);
}
