namespace Midnight;

public class Scene {
    public Scene() {
        Entities = new(this);
        Systems = new(this);
        Components = new();
    }

    public static Scene Current { get; private set; }

    public Entities Entities { get; }
    public EntitySystems Systems { get; }
    public SceneComponents Components { get; }

    /// <summary>
    /// Prepare anything before Start().
    /// </summary>
    public virtual void Prepare() {
        Systems.Register(new RenderSystem());
        Systems.Register(new GUISystem());
    }

    /// <summary>
    /// Start anything which was prepared at Prepare().
    /// </summary>
    public virtual void Start() {
    }

    /// <summary>
    /// Scene begun, it'll be the current scene.
    /// </summary>
    public virtual void Begin() {
        Current = this;
    }

    /// <summary>
    /// Scene ended.
    /// </summary>
    public virtual void End() {
        if (Current != this) {
            return;
        }

        Current = null;
    }

    public virtual void Update(DeltaTime dt) {
        Systems.Send<UpdateStepEvent>(new(dt));
    }

    public virtual void Render(DeltaTime dt) {
        Systems.Send<RenderStepEvent>(new(dt));
    }
}
