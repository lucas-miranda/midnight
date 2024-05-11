using Microsoft.Xna.Framework;

namespace Midnight;

public abstract class Program : Microsoft.Xna.Framework.Game {
	public Program() {
        Random.Init();

        //

		GraphicsDeviceManager = new GraphicsDeviceManager(this);

        /*
		// Typically you would load a config here...
		gdm.PreferredBackBufferWidth = 1280;
		gdm.PreferredBackBufferHeight = 720;
		gdm.IsFullScreen = false;
		gdm.SynchronizeWithVerticalRetrace = true;
		*/
        System.Console.WriteLine("=> Midnight created");
	}

    public static RenderingServer Rendering { get; private set; }
    public GraphicsDeviceManager GraphicsDeviceManager { get; }

	protected override void Initialize() {
		/* This is a nice place to start up the engine, after
		 * loading configuration stuff in the constructor
		 */
        System.Console.WriteLine("=> Midnight Initialize");
        Rendering = new(GraphicsDevice);
		base.Initialize();
	}

	protected override void LoadContent() {
		// Load textures, sounds, and so on in here...
        System.Console.WriteLine("=> Midnight LoadContent");
		base.LoadContent();
	}

	protected override void UnloadContent() {
		// Clean up after yourself!
        System.Console.WriteLine("=> Midnight UnloadContent");
		base.UnloadContent();
	}

	protected override void Update(GameTime gameTime) {
		// Run game logic in here. Do NOT render anything here!
        System.Console.WriteLine($"=> Midnight Update, dt: {gameTime.ElapsedGameTime.TotalMilliseconds}ms");
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		// Render stuff in here. Do NOT run game logic in here!
        System.Console.WriteLine($"=> Midnight Render, dt: {gameTime.ElapsedGameTime.TotalMilliseconds}ms");
		GraphicsDevice.Clear(Color.CornflowerBlue);
		base.Draw(gameTime);
	}
}
