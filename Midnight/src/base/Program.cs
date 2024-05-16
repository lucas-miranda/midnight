using Xna = Microsoft.Xna.Framework;
using Midnight.Diagnostics;

namespace Midnight;

public abstract class Program : Xna.Game {
	public Program(GraphicsConfig? config = null) {
	    Debug.AssertIsNull(Current, "Only one Program can exists.");
	    Current = this;

	    // Modules

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
    public Color Background { get; set; } = new(Xna.Color.CornflowerBlue);
    public Canvas MainCanvas { get; protected set; }

	protected sealed override void Initialize() {
		/* This is a nice place to start up the engine, after
		 * loading configuration stuff in the constructor
		 */
        Rendering = new(GraphicsDevice);
		base.Initialize();
		Setup();
		System.Console.WriteLine($"At Initialize, graphics:\n{Graphics}");
	}

	protected sealed override void LoadContent() {
		// Load textures, sounds, and so on in here...
		base.LoadContent();
		Load();
		System.Console.WriteLine($"At LoadContent, graphics:\n{Graphics}");
	}

	protected sealed override void UnloadContent() {
		// Clean up after yourself!
		base.UnloadContent();
		Unload();
	}

	protected sealed override void Update(Xna.GameTime gameTime) {
		// Run game logic in here. Do NOT render anything here!
		base.Update(gameTime);
		Update(new((float) gameTime.ElapsedGameTime.TotalSeconds));
	}

	protected sealed override void Draw(Xna.GameTime gameTime) {
		// Render stuff in here. Do NOT run game logic in here!
		GraphicsDevice.Clear(Background.ToXna());
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
