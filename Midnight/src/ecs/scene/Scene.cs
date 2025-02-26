namespace Midnight;

public class Scene {
    public Scene() {
        Entities = new(this);
        Systems = new(this);
        Components = new();
    }

    public Entities Entities { get; }
    public EntitySystems Systems { get; }
    public SceneComponents Components { get; }

    /// <summary>
    /// Prepare anything before Start().
    /// </summary>
    public virtual void Prepare() {
        Systems.Register(new RenderSystem());
    }

    /// <summary>
    /// Start anything which was prepared at Prepare().
    /// </summary>
    public virtual void Start() {
    }

    public virtual void Update(DeltaTime dt) {
        Systems.Send<UpdateStepEvent>(new(dt));
    }

    public virtual void Render(DeltaTime dt) {
        Systems.Send<RenderStepEvent>(new(dt));
    }
}
