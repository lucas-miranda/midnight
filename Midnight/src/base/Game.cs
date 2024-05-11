using Microsoft.Xna.Framework;

namespace Midnight;

public class Game : Program {
	public Scene Scene { get; private set; }

	public void LoadScene<S>() where S : Scene, new() {
	    Scene = new S();
    }

	public void LoadScene(Scene scene) {
	    Scene = scene;
    }

	protected sealed override void Initialize() {
	    base.Initialize();
    }

	protected override void LoadContent() {
	    base.LoadContent();
        Scene?.Prepare();
        Scene?.Start();
    }

	protected override void UnloadContent() {
	    base.UnloadContent();
    }

	protected sealed override void Update(GameTime gameTime) {
		base.Update(gameTime);
        Scene?.Update(new((float) gameTime.ElapsedGameTime.TotalSeconds));
    }

	protected sealed override void Draw(GameTime gameTime) {
		base.Draw(gameTime);
		Rendering.PreRender();
        Scene?.Render(new((float) gameTime.ElapsedGameTime.TotalSeconds));
        Rendering.Render();
	}
}
